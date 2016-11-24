using UnityEngine;

public class SoundManager : Singleton<SoundManager> {

	[SerializeField]
	private AudioClip arrow;	
	[SerializeField]
	private AudioClip death;
	[SerializeField]
	private AudioClip fireball;
	[SerializeField]
	private AudioClip gameOver;
	[SerializeField]
	private AudioClip hit;
	[SerializeField]
	private AudioClip level;
	[SerializeField]
	private AudioClip newGame;
	[SerializeField]
	private AudioClip rock;
	[SerializeField]
	private AudioClip towerBuilt;

	private AudioClip Arrow {
		get {
			return arrow;
		}
	}

	private AudioClip Death {
		get {
			return death;
		}
	}

	private AudioClip Fireball {
		get {
			return fireball;
		}
	}

	private AudioClip GameOver {
		get {
			return gameOver;
		}
	}

	private AudioClip Hit {
		get {
			return hit;
		}
	}

	private AudioClip Level {
		get {
			return level;
		}
	}

	private AudioClip NewGame {
		get {
			return newGame;
		}
	}

	private AudioClip Rock {
		get {
			return rock;
		}
	}

	private AudioClip TowerBuilt {
		get {
			return towerBuilt;
		}
	}
}
