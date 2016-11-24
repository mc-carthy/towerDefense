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
	[SerializeField]
	private int rewardAmount;
	private float navigationTime = 0;
	private Transform enemy;
	private Collider2D enemyCollider;
	private Animator anim;

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
		anim = GetComponent<Animator>();
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
			GameManager.Instance.RoundEscaped++;
			GameManager.Instance.TotalEscaped++;
			GameManager.Instance.UnregisterEnemy(this);
			GameManager.Instance.IsWaveOver();
		} else if (other.CompareTag("projectile")) {
			Projectile thisProjectile = other.gameObject.GetComponent<Projectile>();
			EnemyHit(thisProjectile.AttackStrength);
			Destroy(other.gameObject);
		}
	}

	public void EnemyHit(int hitPoints) {
		healthPoints -= hitPoints;
		anim.Play("hurt");
		GameManager.Instance.Source.PlayOneShot(SoundManager.Instance.Hit);
		if (healthPoints <= 0) {
			Die();
		}
	}

	private void Die() {
		isDead = true;
		GameManager.Instance.TotalKilled++;
		GameManager.Instance.IsWaveOver();
		GameManager.Instance.AddMoney(rewardAmount);
		GameManager.Instance.Source.PlayOneShot(SoundManager.Instance.Death);
		anim.SetTrigger("didDie");
		enemyCollider.enabled = false;
	}
}
