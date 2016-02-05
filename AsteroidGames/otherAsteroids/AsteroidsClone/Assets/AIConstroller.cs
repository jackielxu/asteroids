using UnityEngine;
using System.Collections;

public class AIConstroller : MonoBehaviour {

	public GameObject player;
	public GameObject shootingPos;

	private PlayerMovement playerMovement;
	private Shooting shooting;

	// Use this for initialization
	void Start () {
		playerMovement = player.GetComponent<PlayerMovement> ();
		shooting = player.GetComponent<Shooting> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Add your code, here.
}
