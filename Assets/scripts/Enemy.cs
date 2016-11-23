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
	[SerializeField]
	private int healthPoints;
	private Transform enemy;
	private float navigationTime = 0;
	private Collider2D enemyCollider;
	
	private bool isDead = false;
	public bool IsDead {
		get {
			return isDead;
		}
	}


	private void Start () {
		enemy = GetComponent<Transform>();
		GameManager.Instance.RegisterEnemy(this);
		enemyCollider = GetComponent<Collider2D>();
	}

	private void Update () {
		if (checkpoints != null && !isDead) {
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
			GameManager.Instance.UnregisterEnemy(this);
		} else if (other.CompareTag("projectile")) {
			Projectile thisProjectile = other.gameObject.GetComponent<Projectile>();
			EnemyHit(thisProjectile.AttackStrength);
			Destroy(other.gameObject);
		}
	}

	public void EnemyHit(int hitPoints) {
		healthPoints -= hitPoints;

		if (healthPoints <= 0) {
			Die();
		}
	}

	private void Die() {
		isDead = true;
		enemyCollider.enabled = false;
	}
}
