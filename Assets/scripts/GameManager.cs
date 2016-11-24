using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum gameStatus {
	next,
	play,
	gameOver,
	win
}

public class GameManager : Singleton<GameManager> {

	public List<Enemy> enemyList = new List<Enemy>();

	[SerializeField]
	private int totalWaves = 10;
	[SerializeField]
	private Text currentWaveLabel;
	[SerializeField]
	private Text totalEscapedLabel;
	[SerializeField]
	private Text totalMoneyLabel;
	[SerializeField]
	private Text playButtonLabel;
	[SerializeField]
	private Button playButton;

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

	private int waveNumber;
	private int roundEscaped;
	private int totalEscaped;
	private int totalKilled;
	private int whichEnemiesToSpawn = 0;
	private gameStatus currentState = gameStatus.play;

	private int totalMoney = 10;
	public int TotalMoney {
		get {
			return totalMoney;
		}
		set {
			totalMoney = value;
			totalMoneyLabel.text = totalMoney.ToString();
		}
	}

	private void Start () {
		playButton.gameObject.SetActive(false);
		ShowMenu();
	}

	private void Update () {
		HandleEscape();
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

	public void AddMoney (int value) {
		TotalMoney += value;
	}

	public void SubtractMoney (int value) {
		TotalMoney -= value;
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

	private void ShowMenu () {
		switch (currentState) {
			case gameStatus.gameOver:
				playButtonLabel.text = "Play Again!";
				// Add gameOver sfx
				break;
			case gameStatus.next:
				playButtonLabel.text = "Next Wave!";
				break;
			case gameStatus.play:
				playButtonLabel.text = "Start Game!";
				break;
			case gameStatus.win:
				playButtonLabel.text = "Start Game!";
				break;
		}
		playButton.gameObject.SetActive(true);
	}

	private void HandleEscape () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			TowerManager.Instance.DisableDragSprite();
			TowerManager.Instance.towerButtonPressed = null;
		}
	}
}
