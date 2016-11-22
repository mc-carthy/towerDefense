using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

	public GameObject spawnPoint;
	public GameObject[] enemies;
	public int maxEnemiesOnScreen;
	public int totalEnemies;
	public int enemiesPerSpawn;
	public float spawnDelay;

	private int enemiesOnScreen = 0;

	private void Start () {
		StartCoroutine(Spawn());
	}

	public void RemoveEnemyFromScreen () {
		if (enemiesOnScreen > 0) {
			enemiesOnScreen--;
		}
	}


	private IEnumerator Spawn () {
		if (enemiesPerSpawn > 0 && enemiesOnScreen < totalEnemies) {
			for (int i = 0; i < enemiesPerSpawn; i++) {
				if (enemiesOnScreen < maxEnemiesOnScreen) {
					GameObject newEnemy = Instantiate(enemies[Random.Range(0, enemies.Length)]) as GameObject;
					newEnemy.transform.position = spawnPoint.transform.position;
					enemiesOnScreen++;
				}
			}
		}
		yield return new WaitForSeconds(spawnDelay);
		StartCoroutine(Spawn());
	}
}
