using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Singleton<TowerManager> {

	private TowerButton towerButtonPressed;

	private void Update () {
		if (Input.GetMouseButton(0)) {
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
			if (hit.collider.tag == "buildSite") {
				PlaceTower(hit);
			}
		}
	}

	public void SelectedTower (TowerButton towerSelected) {
		towerButtonPressed = towerSelected;
	}

	public void PlaceTower (RaycastHit2D hit) {
		if (!EventSystem.current.IsPointerOverGameObject() && towerButtonPressed != null) {
			GameObject newTower = Instantiate(towerButtonPressed.TowerObject);
			newTower.transform.position = hit.transform.position;
		}
	}
}
