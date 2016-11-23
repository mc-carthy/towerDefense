using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {

	public List<Enemy> enemyList = new List<Enemy>();

	[SerializeField]
	private GameObject spawnPoint;
	[SerializeField]
	private GameObject[] enemies;
	[SerializeField]
	private int maxEnemiesOnScreen;
	[SerializeField]
	private int totalEnemies;
	[SerializeField]
	private int enemiesPerSpawn;
	[SerializeField]
	private float spawnDelay;

	private void Start () {
		StartCoroutine(Spawn());
	}

	public void RegisterEnemy (Enemy enemy) {
		enemyList.Add(enemy);
	}

	public void UnregisterEnemy (Enemy enemy) {
		enemyList.Remove(enemy);
		Destroy(enemy.gameObject);
	}

	public void DestroyAllEnemies () {
		foreach (Enemy enemy in enemyList) {
			Destroy(enemy.gameObject);
		}
		enemyList.Clear();
	}

	private IEnumerator Spawn () {
		if (enemiesPerSpawn > 0 && enemyList.Count < totalEnemies) {
			for (int i = 0; i < enemiesPerSpawn; i++) {
				if (enemyList.Count < maxEnemiesOnScreen) {
					GameObject newEnemy = Instantiate(enemies[Random.Range(0, enemies.Length)]) as GameObject;
					newEnemy.transform.position = spawnPoint.transform.position;
				}
			}
		}
		yield return new WaitForSeconds(spawnDelay);
		StartCoroutine(Spawn());
	}
}
