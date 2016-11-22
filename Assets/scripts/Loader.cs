using UnityEngine;

public class Loader : MonoBehaviour {

	public GameObject gameManager;

	private void Awake () {
		MakeSingleton();
	}
	
	private void MakeSingleton () {
		if (GameManager.instance == null) {
			Instantiate(gameManager);
		}
	}
}
