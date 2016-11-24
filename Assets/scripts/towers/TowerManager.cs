using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class TowerManager : Singleton<TowerManager> {

	public TowerButton towerButtonPressed { get; set; }
	private SpriteRenderer spriteRenderer;

	private void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update () {
		if (Input.GetMouseButton(0)) {
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
			if (hit.collider.tag == "buildSite") {
				hit.collider.tag = "buildSiteFull";
				PlaceTower(hit);
			}
		}
		if (spriteRenderer.enabled) {
			FollowMouse();
		}
	}

	public void SelectedTower (TowerButton towerSelected) {
		if (towerSelected.TowerPrice <= GameManager.Instance.TotalMoney) {
			towerButtonPressed = towerSelected;
			EnableDragSprite(towerButtonPressed.DragSprite);
		}
	}

	public void PlaceTower (RaycastHit2D hit) {
		if (!EventSystem.current.IsPointerOverGameObject() && towerButtonPressed != null) {
			GameObject newTower = Instantiate(towerButtonPressed.TowerObject);
			newTower.transform.position = hit.transform.position;
			BuyTower(towerButtonPressed.TowerPrice);
			DisableDragSprite();
		}
	}

	public void BuyTower (int price) {
		GameManager.Instance.SubtractMoney(price);
	}

	private void FollowMouse() {
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector2(transform.position.x, transform.position.y);
	}

	private void EnableDragSprite (Sprite sprite) {
		spriteRenderer.enabled = true;
		spriteRenderer.sprite = sprite;
	}

	public void DisableDragSprite () {
		spriteRenderer.enabled = false;
	}
}
