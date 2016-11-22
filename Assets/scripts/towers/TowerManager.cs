using UnityEngine;

public class TowerManager : Singleton<TowerManager> {

	private TowerButton towerButtonPressed;

	public void SelectedTower (TowerButton towerSelected) {
		Debug.Log(towerSelected);
		towerButtonPressed = towerSelected;
	}
}
