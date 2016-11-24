using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class TowerManager : Singleton<TowerManager> {

	public TowerButton towerButtonPressed { get; set; }
	
	private SpriteRenderer spriteRenderer;
	private List<Tower> towerList = new List<Tower>();
	private List<Collider2D> buildList = new List<Collider2D>();
	private Collider2D buildTile;

	private void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		buildTile = GetComponent<Collider2D>();
	}

	private void Update () {
		if (Input.GetMouseButton(0)) {
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
			if (hit.collider.tag == "buildSite") {
				buildTile = hit.collider;
				buildTile.tag = "buildSiteFull";
				RegisterBuildSite(buildTile);
				PlaceTower(hit);
			}
		}
		if (spriteRenderer.enabled) {
			FollowMouse();
		}
	}

	public void RegisterBuildSite (Collider2D buildTag) {
		buildList.Add(buildTag);
	}

	public void RegisterTower (Tower tower) {
		towerList.Add(tower);
	}

	public void RenameTagsBuildSites () {
		foreach(Collider2D buildTag in buildList) {
			buildTag.tag = "buildSite";
		}
		buildList.Clear();
	}

	public void DestroyAllTowers () {
		foreach (Tower tower in towerList) {
			Destroy(tower.gameObject);
		}
		towerList.Clear();
	}

	public void SelectedTower (TowerButton towerSelected) {
		if (towerSelected.TowerPrice <= GameManager.Instance.TotalMoney) {
			towerButtonPressed = towerSelected;
			EnableDragSprite(towerButtonPressed.DragSprite);
		}
	}

	public void PlaceTower (RaycastHit2D hit) {
		if (!EventSystem.current.IsPointerOverGameObject() && towerButtonPressed != null) {
			Tower newTower = Instantiate(towerButtonPressed.TowerObject) as Tower;
			newTower.transform.position = hit.transform.position;
			BuyTower(towerButtonPressed.TowerPrice);
			RegisterTower(newTower);
			GameManager.Instance.Source.PlayOneShot(SoundManager.Instance.TowerBuilt);
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
