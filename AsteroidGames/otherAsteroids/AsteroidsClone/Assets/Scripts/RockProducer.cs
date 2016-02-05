using UnityEngine;
using System.Collections;

public class RockProducer : MonoBehaviour
{
	public GameObject rockTypeBig;
	private int spawnRockQuantity = 1;
	private int currentSpawnRockQuantity;
//	private float resetTime = 5f;
//	private float accumulateTime = 0f;
//	private bool spawnSaucer = false;

	private bool newHighScoreFound = false;

    private bool autoShootingModeOn;

    private float autoShootingDetectAngle = 50f;
    private float autoShootingAngleDecreasingStep = 10f;

	public GUISkin guiSkin;
	public int currentRockNumber;

    public int mode;

	private int[] highScore;
	private int currentScore = 0;

	// bool for deciding the display of clear level
	private bool displayClearLevel = false;

	private GameManager gameManager;
	private PlayerMovement playerMovement;
	// Fading
	public Fading fading;

	private AudioSource completeLevelSound;

    private GoogleAnalyticsManager googleAnalyticsManager;
	// Use this for initialization
	void Start ()
	{
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();

        googleAnalyticsManager = GameObject.Find("GAv3").GetComponent<GoogleAnalyticsManager>();

		completeLevelSound = gameObject.GetComponent<AudioSource> ();
		currentSpawnRockQuantity = spawnRockQuantity;
        if (mode == 0)
        {
            autoShootingModeOn = false;
        }
        else 
        {
            autoShootingModeOn = true;
        }
	}

	// Update is called once per frame
	void Update ()
	{
		// If there are still rocks to be spawn, do it
		while (spawnRockQuantity > 0) {	
			Vector2 spawnPostion = Vector2.zero;
			int spawnSide = Random.Range (1, 5); // decide which side to spwan
			
			if (spawnSide == 1) {	// Spawn along left side 
				spawnPostion.x = -10f;	// Spawn position decided
				spawnPostion.y = Random.Range (-5f, 5f);
			} else if (spawnSide == 2) {	// Spawn along up side 
				spawnPostion.x = Random.Range (-10f, 10f);	// Spawn position decided
				spawnPostion.y = 5f;
			} else if (spawnSide == 3) {	// Spawn along right side 
				spawnPostion.x = 10f;	// Spawn position decided
				spawnPostion.y = Random.Range (-5f, 5f);
			} else if (spawnSide == 4) {	// Spawn along right side 
				spawnPostion.x = Random.Range (-10f, 10f);	// Spawn position decided
				spawnPostion.y = -5f;
			}
			
			GameObject bigRock = Instantiate (rockTypeBig, spawnPostion, Quaternion.identity) as GameObject;
            googleAnalyticsManager.LogEvent("SpawnRock", "Spawn", "Rock_" + spawnRockQuantity.ToString(), spawnRockQuantity);
			Vector2 Dir = new Vector2 (Random.Range (-359, 359), Random.Range (-359, 359));
			bigRock.GetComponent<BigRock> ().SetDir (Dir.normalized);
			spawnRockQuantity--;
			currentRockNumber++;
		}
		
		// If there is no rock left
		if (currentRockNumber == 0 && displayClearLevel == false && gameManager.CheckFinal () == false) {
			// Stop game
			Time.timeScale = 0;
			currentScore = gameManager.GetCurrentScore ();
			newHighScoreFound = gameManager.CompareRecord (currentScore);
			completeLevelSound.Play ();
			displayClearLevel = true;
		}

		/**
		if (!spawnSaucer) {
			accumulateTime += Time.deltaTime;
			if (accumulateTime > resetTime) {
				accumulateTime = 0;
				spawnSaucer = true;
			}
		}
		
		if (spawnSaucer) {
			
		}
         * */
        if (mode == 1)
        {
            if (currentSpawnRockQuantity > 5 && autoShootingModeOn)   // if 5 levels have been finished
            {
                autoShootingModeOn = false; // auto aim is turned off
                currentSpawnRockQuantity = 1; // level is reset to one
            }
        }
	}

	void FixedUpdate ()
	{

	}
	
	void OnGUI ()
	{
		if (displayClearLevel) {
			
			Rect winScreenRect = new Rect ((Screen.width - Screen.width * 0.5f) / 2, 
			                               (Screen.height - Screen.height * 0.5f) / 2, 
			                               Screen.width * 0.5f, Screen.height * 0.5f);	

			GUI.Box (winScreenRect, "Level Accomplished", guiSkin.GetStyle ("box"));

			if (newHighScoreFound) {
				GUI.Label (new Rect (winScreenRect.x + winScreenRect.width * 0.5f - winScreenRect.width * 0.5f / 2, 
				                     winScreenRect.y + winScreenRect.height * 0.25f + 25, 
				                     winScreenRect.width * 0.5f, 
				                     100), 
				           " New Record Reached! :)\n\n" + "Score: " + currentScore, 
				           			guiSkin.GetStyle ("label"));
			} else {
				GUI.Label (new Rect (winScreenRect.x + winScreenRect.width * 0.5f - winScreenRect.width * 0.5f / 2, 
				                     winScreenRect.y + winScreenRect.height * 0.25f + 25,
				                     winScreenRect.width * 0.5f,
				                     100), 
				           			"Score: " + currentScore, 
				           			 guiSkin.GetStyle ("label"));
			}
			if (GUI.Button (new Rect (winScreenRect.x + winScreenRect.width * 0.25f - winScreenRect.width * 0.3f / 2,
			                          winScreenRect.y + winScreenRect.height * 0.75f,
			                          winScreenRect.width * 0.3f,
			                          40), 
			                		"Back to Menu",
			                		guiSkin.GetStyle ("button"))) {
				gameManager.SetHighScore (currentScore);
				fading.LoadSceneWrapperWrapper (0);

			}
			if (GUI.Button (new Rect (winScreenRect.x + winScreenRect.width * 0.25f - winScreenRect.width * 0.3f / 2 + winScreenRect.width * 0.5f,
			                          winScreenRect.y + winScreenRect.height * 0.75f,
			                          winScreenRect.width * 0.3f, 40),
			                "Next Level!",
			                guiSkin.GetStyle ("button"))) {
				currentSpawnRockQuantity++;
                if (mode == 2)
                {
                    if (autoShootingDetectAngle >= 5.0f)
                    {
                        autoShootingDetectAngle -= autoShootingAngleDecreasingStep;
                    }
                    else
                    {
                        autoShootingModeOn = false;
                        currentSpawnRockQuantity = 1;
                    }
                }
				spawnRockQuantity = currentSpawnRockQuantity;
				displayClearLevel = false;
				Time.timeScale = 1;
				playerMovement = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();
				playerMovement.ResetPosition ();
				playerMovement.SetInvincible ();
			}
		}
	}

	public void addRockNumber ()
	{
		currentRockNumber++;
	}

	public void minusRockNumber ()
	{
		currentRockNumber--;
	}

	public int levelSurvived ()
	{
		return currentSpawnRockQuantity - 1;
	}

	public bool IsFinished ()
	{
		return displayClearLevel;
	}

    public bool autoShootingMode()
    {
        return autoShootingModeOn;
    }

    public float autoShootingAngle(){
        return autoShootingDetectAngle;
    }
		
}
