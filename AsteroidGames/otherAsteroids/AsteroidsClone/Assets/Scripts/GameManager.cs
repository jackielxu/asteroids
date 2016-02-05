using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
	// Current Total Score
	private int currentScore = 0;
	private int bonusScore = 0;

	private int liveBonusThreshold = 1000;

	// Top 10 High Scores
	private int[] highScore = new int[10];

	// Live counts 
	private int totalLives = 5;
	private int lives = 0;	

	// End Display
	private bool isFinal = false;
	private bool newHighScoreFound = false;
	private GameManager gameManager;

	// Lives Picture
	public Texture2D livesTexture;

	// Player Prefabs
	public GameObject playerPrefab;

	// GUI Style
	public GUISkin displaySkin;
	public GUISkin guiSkin;

	// Fading
	public Fading fading;

	public RockProducer rockProducer;

	private AudioSource lifeBonus;

	private bool displayLive = true ;

    private GoogleAnalyticsManager googleAnalyticsManager;

	void Start ()
	{
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
        googleAnalyticsManager = GameObject.Find("GAv3").GetComponent<GoogleAnalyticsManager>();

		lifeBonus = gameObject.GetComponent<AudioSource> ();
		// Initialise total live value
		lives = totalLives;

		// Initialise high score array with 0s 
		if (!PlayerPrefs.HasKey ("HighScore-1")) {
			for (int i=0; i<10; i++) {
				highScore [i] = 0;
			}
		} else {
			for (int i=0; i<10; i++) {
				highScore [i] = PlayerPrefs.GetInt ("HighScore-" + (i + 1).ToString ());
			}
		}
	}

	void Update ()
	{
		if (bonusScore >= liveBonusThreshold) {
			LifeBonus ();
			bonusScore -= liveBonusThreshold;
		}
	}

	void OnGUI ()
	{
		if (Application.loadedLevel > 0) {
			Rect livesRec = new Rect (Screen.width * 0.025f, Screen.height * 0.025f, Screen.width * 0.25f + Screen.width * 0.025f, Screen.height * 0.25f + Screen.height * 0.025f); //Adjust the rectangle position and size for your own needs
			GUILayout.BeginArea (livesRec);
			GUILayout.BeginHorizontal ();

			if (displayLive) {
				if (lives < 4) {
					for (int i = 1; i < lives; i++)
						GUILayout.Label (livesTexture, GUILayout.Width ((float)(Screen.width * 0.04))); //assign your heart image to this texture
				} else {
					GUILayout.Label (livesTexture, displaySkin.GetStyle ("lives"), GUILayout.Width ((float)(Screen.width * 0.04)));
					GUILayout.Label (" X" + (lives - 1).ToString (), displaySkin.GetStyle ("lives"), GUILayout.Width (Screen.width * 0.25f), GUILayout
				                 .Height (Screen.height * 0.25f));
				}
			}
		
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();
			GUILayout.EndArea ();

			Rect scoreRec = new Rect (Screen.width * 0.75f - Screen.width * 0.025f, 0 + Screen.height * 0.025f, Screen.width - Screen.width * 0.025f, Screen.height * 0.25f + Screen.height * 0.025f); //Adjust the rectangle position and size for your own needs
			GUILayout.BeginArea (scoreRec);
			GUILayout.BeginHorizontal ();

			GUILayout.Label ("Score: " + currentScore.ToString (), displaySkin.GetStyle ("score"), GUILayout.Width (Screen.width * 0.25f), GUILayout.Height (Screen.height * 0.25f));
	
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();
			GUILayout.EndArea ();
		}

		if (isFinal) {
			Rect winScreenRect = new Rect ((Screen.width - Screen.width * 0.5f) / 2, 
			                               (Screen.height - Screen.height * 0.5f) / 2, 
			                               Screen.width * 0.5f, Screen.height * 0.5f);	

			if (rockProducer.levelSurvived () > 1)
				GUI.Box (winScreenRect, "You Survived " + rockProducer.levelSurvived ().ToString () + " levels!", guiSkin.GetStyle ("box"));
			else
				GUI.Box (winScreenRect, "You Survived " + rockProducer.levelSurvived ().ToString () + " level!", guiSkin.GetStyle ("box"));
			if (newHighScoreFound) {
				GUI.Label (new Rect (winScreenRect.x + winScreenRect.width * 0.5f - winScreenRect.width * 0.5f / 2, 
				                     winScreenRect.y + winScreenRect.height * 0.25f + 25, 
				                     winScreenRect.width * 0.5f, 
				                     100), 
				           " New High Score Reached! :)\n\n" + "Score: " + currentScore, 
				           guiSkin.GetStyle ("label"));
			} else {
				GUI.Label (new Rect (winScreenRect.x + winScreenRect.width * 0.5f - winScreenRect.width * 0.5f / 2, 
				                     winScreenRect.y + winScreenRect.height * 0.25f + 25,
				                     winScreenRect.width * 0.5f,
				                     100), 
				           "Score: " + currentScore, 
				           guiSkin.GetStyle ("label"));
			}
			if (GUI.Button (new Rect (winScreenRect.x + winScreenRect.width * 0.25f,
			                          winScreenRect.y + winScreenRect.height * 0.75f,
			                          winScreenRect.width * 0.5f,
			                          40), 
			                "Back to Menu",
			                guiSkin.GetStyle ("button"))) {
				SetHighScore (currentScore);
				fading.LoadSceneWrapperWrapper (0);
				
			}
		}
	}


	void FixedUpdate ()
	{
	}

	// For adding bonus score for hitting big rocks
	public void ShotBigRockBonus ()
	{
		currentScore += 25;
		bonusScore += 25;
	}

	// For adding bonus score for hitting middle rocks
	public void ShotMiddleRockBonus ()
	{
		currentScore += 50;
		bonusScore += 50;
	}

	// For adding bonus score for hitting small rocks
	public void ShotSmallRockBonus ()
	{
		currentScore += 75;
		bonusScore += 75;
	}

	// For adding bonus score for hitting tiny rocks
	public void ShotTinyRockBonus ()
	{
		currentScore += 100;
		bonusScore += 100;
	}

	// For adding bonus score for hitting spaceships
	public void ShotSpaceshipBonus ()
	{
	}

	// For dead
	public void Dead ()
	{
		if (lives > 1) {
			lives--;
			Respawn ();
		} else {
			lives = 0;
			Time.timeScale = 0;
			currentScore = gameManager.GetCurrentScore ();
			newHighScoreFound = CompareRecord (currentScore);
			isFinal = true;
		}
	}
	

	void Respawn ()
	{
		Instantiate (playerPrefab, new Vector2 (Screen.width / 2, Screen.height / 2), Quaternion.identity);
	}



	public int GetCurrentScore ()
	{
		return currentScore;
	}


	public int[] GetHighScore ()
	{
		return highScore;
	}

	public void SetHighScore (int newHighScore)
	{
		if (newHighScore > highScore [9]) {
			highScore [9] = newHighScore;
			Array.Sort<int> (highScore,
			                 new Comparison<int> (
				(i1, i2) => i2.CompareTo (i1)
			));
			SaveHighScores ();
		} 
	}

	public bool CompareRecord (int newHighScore)
	{
		if (newHighScore > highScore [0]) {
			return true;
		} else
			return false;
	}

	public int GetLives ()
	{
		return lives;
	}

	public int GetTotalLives ()
	{
		return totalLives;
	}

	void SaveHighScores ()
	{
		for (int i=0; i<10; i++) {
			PlayerPrefs.SetInt ("HighScore-" + (i + 1).ToString (), highScore [i]);
		}
	}

	void LifeBonus ()
	{
		lifeBonus.Play ();
		StartCoroutine (BlinkLife ());
		lives++;
	}

	IEnumerator BlinkLife ()
	{
		displayLive = false;
		yield return new WaitForSeconds (0.25f);
		displayLive = true;
		yield return new WaitForSeconds (0.25f);
		displayLive = false;
		yield return new WaitForSeconds (0.25f);
		displayLive = true;
	}

	public bool CheckFinal ()
	{
		return isFinal;
	}
	

}
