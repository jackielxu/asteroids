using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour
{
	public Texture2D fadeOutTexture;
	public float fadeSpeed = 0.8f;
	
	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private int fadeDir = -1;

	public GameObject clickSoundObject;
	private AudioSource clickSound;

	void Start ()
	{
		clickSound = clickSoundObject.GetComponent<AudioSource> ();
	}

	void OnGUI ()
	{
		// fade out
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01 (alpha);
		
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture);
	}
	
	public float BeginFade (int direction)
	{
		fadeDir = direction;
		return(fadeSpeed);
	}
	
	void OnLevelWasLoaded ()
	{
		BeginFade (-1);
	}

	// Use this for initialization
	public IEnumerator LoadScene (int levelIndex)
	{
		float fadeTime = gameObject.GetComponent<Fading> ().BeginFade (1);
		Time.timeScale = 1;
		yield return new WaitForSeconds (fadeTime);
		Application.LoadLevel (levelIndex);
	}
	
	public void LoadSceneWrapperWrapper (int levelIndex)
	{
		clickSound.Play ();
		StartCoroutine (LoadScene (levelIndex));
	}
}
