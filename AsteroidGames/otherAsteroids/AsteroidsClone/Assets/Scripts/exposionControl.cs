using UnityEngine;
using System.Collections;

public class exposionControl : MonoBehaviour
{
	private ParticleSystem particle;
	// Use this for initialization
	void Start ()
	{
		particle = gameObject.GetComponent<ParticleSystem> ();
		particle.Play ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Destroy (gameObject, 0.5f);
	}
}
