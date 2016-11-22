using UnityEngine;

public class Enemy : MonoBehaviour {

	[SerializeField]
	private int target = 0;
	[SerializeField]
	private Transform exitPoint;		
	[SerializeField]
	private Transform[] checkpoints;
	[SerializeField]
	private float navigationUpdate;
	private Transform enemy;
	private float navigationTime = 0;

	private void Start () {
		enemy = GetComponent<Transform>();
	}

	private void Update () {
		if (checkpoints != null) {
			navigationTime += Time.deltaTime;
			if (navigationTime > navigationUpdate) {
				if (target < checkpoints.Length) {
					enemy.position = Vector2.MoveTowards(enemy.position, checkpoints[target].position, navigationTime);
				} else {
					enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, navigationTime);
				}
				navigationTime = 0;
			}
		}
	}

	private void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag("checkpoint")) {
			target++;
		} else if (other.CompareTag("Finish")) {
			GameManager.Instance.RemoveEnemyFromScreen();
			Destroy(gameObject);
		}
	}
}
