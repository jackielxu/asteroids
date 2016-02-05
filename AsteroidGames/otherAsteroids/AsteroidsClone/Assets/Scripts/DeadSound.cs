using UnityEngine;
using System.Collections;

public class DeadSound : MonoBehaviour
{

	private AudioSource deadAudio;
	// Use this for initialization
	void Start ()
	{
		deadAudio = GetComponent<AudioSource> ();
		deadAudio.Play ();
		Destroy (gameObject, deadAudio.clip.length);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
