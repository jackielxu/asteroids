using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	public GameManager gameManage;
	public RockProducer rockProducer;

	// Use this for initialization
	void Start ()
	{
		gameManage = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.timeScale == 0) {
			Destroy (gameObject);
		}
	
	}
	void OnTriggerEnter2D (Collider2D other)
	{
	
		if (other.tag == "BigRock") {
			gameManage.ShotBigRockBonus ();
			Destroy (gameObject);
		} else if (other.tag == "MiddleRock") {
			gameManage.ShotMiddleRockBonus ();
			Destroy (gameObject);
		} else if (other.tag == "SmallRock") {
			gameManage.ShotSmallRockBonus ();
			Destroy (gameObject);
		} else if (other.tag == "TinyRock") {
			gameManage.ShotTinyRockBonus ();
			Destroy (gameObject);
		} else if (other.tag == "Player") {
			Destroy (gameObject);
		}
	}
}
