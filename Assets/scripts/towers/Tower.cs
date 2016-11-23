using UnityEngine;
using System.Collections.Generic;

public class Tower : MonoBehaviour {

	[SerializeField]
	private float timeBetweenAttacks;
	[SerializeField]
	private float attackRadius;
	private Projectile projectile;
	private Enemy targetEnemy = null;
	private float attackCounter;

	private List<Enemy> GetAllEnemiesInRange () {
		List<Enemy> enmiesInRange = new List<Enemy>();
		foreach (Enemy enemy in GameManager.Instance.enemyList) {
			if (Vector2.Distance(enemy.transform.position, transform.position) < attackRadius) {
				enmiesInRange.Add(enemy);
			}
		}
		return enmiesInRange;
	}
	
	private Enemy GetNearestEnemyInRange() {
		Enemy nearestEnemy = null;

		float smallestDistance = float.PositiveInfinity;

		foreach (Enemy enemy in GetAllEnemiesInRange()) {
			if (Vector2.Distance(transform.position, enemy.transform.position) < smallestDistance) {
				smallestDistance = Vector2.Distance(transform.position, enemy.transform.position);
				nearestEnemy = enemy;
			}
		}

		return nearestEnemy;
	}
	
}
