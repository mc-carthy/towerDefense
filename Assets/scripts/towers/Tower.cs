using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour {

	[SerializeField]
	private float timeBetweenAttacks;
	[SerializeField]
	private float attackRadius;
	[SerializeField]
	private Projectile projectile;
	private Enemy targetEnemy = null;
	private float attackCounter;
	private bool isAttacking;

	private void Update () {
		attackCounter -= Time.deltaTime;

		if (targetEnemy == null || targetEnemy.IsDead) {
			Enemy nearestEnemy = GetNearestEnemyInRange();
			if (nearestEnemy != null && Vector2.Distance(transform.localPosition, nearestEnemy.transform.localPosition) <= attackRadius) {
				if (attackCounter < 0) {
					targetEnemy = nearestEnemy;
				}
			}
		} else {
			if (attackCounter <= 0) {
				isAttacking = true;
				attackCounter = timeBetweenAttacks;
			} else {
				isAttacking = false;
			}

			if (Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadius) {
				targetEnemy = null;
			}
		}
	}

	private void FixedUpdate () {
		if (isAttacking) {
			Attack();
		}
	}

	public void Attack () {
		isAttacking = false;
		
		Projectile newProjectile = Instantiate(projectile) as Projectile;
		newProjectile.transform.localPosition = transform.localPosition;

		if (targetEnemy == null) {
			Destroy(newProjectile);
		} else {
			StartCoroutine(MoveProjectile(newProjectile));
		}
	}

	private List<Enemy> GetAllEnemiesInRange () {
		List<Enemy> enmiesInRange = new List<Enemy>();
		foreach (Enemy enemy in GameManager.Instance.enemyList) {
			if (Vector2.Distance(enemy.transform.localPosition, transform.localPosition) < attackRadius) {
				enmiesInRange.Add(enemy);
			}
		}
		return enmiesInRange;
	}
	
	private Enemy GetNearestEnemyInRange() {
		Enemy nearestEnemy = null;

		float smallestDistance = float.PositiveInfinity;

		foreach (Enemy enemy in GetAllEnemiesInRange()) {
			if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance) {
				smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
				nearestEnemy = enemy;
			}
		}

		return nearestEnemy;
	}
	
	private IEnumerator MoveProjectile (Projectile projectile) {
		while (GetTargetDistance(targetEnemy) > 0.2f && projectile != null && targetEnemy != null) {
			Vector2 direction = targetEnemy.transform.localPosition - transform.localPosition;
			float angleDirection = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			projectile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
			projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);
			yield return null;
		}
		if (projectile != null || targetEnemy == null) {
			Destroy(projectile);
		}
	}

	private float GetTargetDistance (Enemy enemy) {
		if (enemy == null) {
			enemy = GetNearestEnemyInRange();
			if (enemy == null) {
				return 0f;
			}
		}
		return Mathf.Abs(Vector2.Distance(transform.localPosition, enemy.transform.localPosition));
	}
}
