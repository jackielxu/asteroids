using UnityEngine;
using System.Collections;

public class ExplosionSound : MonoBehaviour
{
	private AudioSource explosionAudio;
	// Use this for initialization
	void Start ()
	{
		explosionAudio = GetComponent<AudioSource> ();
		explosionAudio.Play ();
		Destroy (gameObject, explosionAudio.clip.length);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
