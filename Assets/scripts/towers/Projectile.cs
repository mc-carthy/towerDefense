using UnityEngine;

public enum projectileType { 
	rock, 
	arrow, 
	fireball 
};

public class Projectile : MonoBehaviour {

	[SerializeField]
	private int attackStrength;
	[SerializeField]
	private projectileType pType;

	public int AttackStrength {
		get {
			return attackStrength;
		}
	}

	public projectileType PType {
		get {
			return pType;
		}
	}
}
