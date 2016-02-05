using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{

	public GameObject mainPanel;
	public GameObject controlPanel;
	public GameObject highScorePanel;

	private Animator mainAnimator;
	private Animator controlAnimator;
	private Animator highScoreAnimator;

	private AudioSource switchSoundPlayer;

	private AudioSource clickSound;
	public GameObject clickSoundObject;
	// Use this for initialization
	void Start ()
	{
		clickSound = clickSoundObject.GetComponent<AudioSource> ();
		// Initialise main animator
		mainAnimator = mainPanel.GetComponent<Animator> ();
		controlAnimator = controlPanel.GetComponent<Animator> ();
		highScoreAnimator = highScorePanel.GetComponent<Animator> ();
		switchSoundPlayer = gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void mianPanelToControl ()
	{
		clickSound.Play ();
		mainAnimator.SetBool ("isHidden", true);
		StartCoroutine (waitASecondControl ());
	}

	public void mianPanelToHighScore ()
	{
		clickSound.Play ();
		mainAnimator.SetBool ("isHidden", true);
		StartCoroutine (waitASecondHighScore ());
	}

	public void controlPanelFadeOut ()
	{
		clickSound.Play ();
		controlAnimator.SetBool ("isHidden", true);
		StartCoroutine (waitASecondMain ());
	}

	public void highScorePanelFadeOut ()
	{
		clickSound.Play ();
		highScoreAnimator.SetBool ("isHidden", true);
		StartCoroutine (waitASecondMain ());
	}

	IEnumerator waitASecondControl ()
	{
		yield return new WaitForSeconds (0.5f);
		switchSoundPlayer.Play ();
		controlAnimator.SetBool ("isHidden", false);
	}

	IEnumerator waitASecondHighScore ()
	{
		yield return new WaitForSeconds (0.5f);
		switchSoundPlayer.Play ();
		highScoreAnimator.SetBool ("isHidden", false);
	}

	IEnumerator waitASecondMain ()
	{
		yield return new WaitForSeconds (0.5f);
		switchSoundPlayer.Play ();
		mainAnimator.SetBool ("isHidden", false);
	}

}