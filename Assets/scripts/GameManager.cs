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

	public int RoundEscaped { get; set;	}
	public int TotalEscaped { get; set; }
	public int TotalKilled { get; set; }
	
	private int intialTotalMoney = 10;
	private int totalMoney;
	public int TotalMoney {
		get {
			return totalMoney;
		}
		set {
			totalMoney = value;
			totalMoneyLabel.text = totalMoney.ToString();
		}
	}

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
	private int initialTotalEnemies = 3;
	private int totalEnemies;
	[SerializeField]
	private int enemiesPerSpawn;
	[SerializeField]
	private float spawnDelay;
	[SerializeField]
	private int escapedEnemiesFailureThreshold = 10;

	private int waveNumber;
	private int whichEnemiesToSpawn = 0;
	private gameStatus currentState = gameStatus.play;

	private void Start () {
		totalEnemies = initialTotalEnemies;
		totalMoney = intialTotalMoney;
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

	public void IsWaveOver () {
		totalEscapedLabel.text = "Escaped " + TotalEscaped.ToString() + "/" + escapedEnemiesFailureThreshold.ToString();
		if (RoundEscaped + TotalKilled == totalEnemies) {
			SetCurrentGameState();
			ShowMenu();
		}
	}

	public void SetCurrentGameState () {
		if (TotalEscaped >= escapedEnemiesFailureThreshold) {
			currentState = gameStatus.gameOver;
		} else if (waveNumber == 0 && (TotalKilled + RoundEscaped) == 0) {
			currentState = gameStatus.play;
		} else if (waveNumber >= totalWaves) {
			currentState = gameStatus.win;
		} else {
			currentState = gameStatus.next;
		}
	}

	private IEnumerator Spawn () {
		if (enemiesPerSpawn > 0 && enemyList.Count < totalEnemies) {
			for (int i = 0; i < enemiesPerSpawn; i++) {
				if (enemyList.Count < totalEnemies) {
					GameObject newEnemy = Instantiate(enemies[0]) as GameObject;
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

	public void PlaybuttonPressed () {
		switch (currentState) {
			case gameStatus.next:
				waveNumber++;
				totalEnemies += waveNumber;
				break;
			default:
				totalEnemies = initialTotalEnemies;
				TotalEscaped = 0;
				TotalMoney = 10;
				// destroy all enemies and towers
				totalMoneyLabel.text = TotalMoney.ToString();
				totalEscapedLabel.text = "Escaped " + TotalEscaped.ToString() + "/" + escapedEnemiesFailureThreshold.ToString();
				break;
		}
		DestroyAllEnemies();
		TotalKilled = 0;
		RoundEscaped = 0;
		currentWaveLabel.text = "Wave " + (waveNumber + 1).ToString();
		StartCoroutine(Spawn());
		playButton.gameObject.SetActive(false);
	}

	private void HandleEscape () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			TowerManager.Instance.DisableDragSprite();
			TowerManager.Instance.towerButtonPressed = null;
		}
	}
}
