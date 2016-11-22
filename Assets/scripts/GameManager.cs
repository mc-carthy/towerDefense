using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public GameObject spawnPoint;
	public GameObject[] enemies;
	public int maxEnemiesOnScreen;
	public int totalEnemies;
	public int enemiesPerSpawn;

	private int enemiesOnScreen = 0;

	private void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	private void Start () {
		SpawnEnemy();
	}

	public void RemoveEnemyFromScreen () {
		if (enemiesOnScreen > 0) {
			enemiesOnScreen--;
		}
	}

	private void SpawnEnemy () {
		if (enemiesPerSpawn > 0 && enemiesOnScreen < totalEnemies) {
			for (int i = 0; i < enemiesPerSpawn; i++) {
				if (enemiesOnScreen < maxEnemiesOnScreen) {
					GameObject newEnemy = Instantiate(enemies[Random.Range(0, enemies.Length)]) as GameObject;
					newEnemy.transform.position = spawnPoint.transform.position;
					enemiesOnScreen++;
				}
			}
		}
	}
}
