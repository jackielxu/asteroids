using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	private Vector2 defaultPosition = new Vector2 (0f, 0f);
	private Quaternion defaultDirection = new Quaternion ();
	private float rotateSpeed = 300f;
	private float force = 5f;
	private Rigidbody2D rb;
	private Renderer rend;
	private float leftConstraint;
	private float rightConstraint;
	private float upConstraint;
	private float downConstraint;
	private float buffer = 0.5f; // set this so the spaceship disappears offscreen before re-appearing on other side
	private bool isInvincible = false;
	private float invincibleTime = 3.0f;
	private float timeSpentInvincible = 0;

	public float distanceZ = 10.0f;
	int timer;

	// The game manager object
	private GameManager gameManage;
	public ParticleSystem particle;
	public ParticleSystem explosion;
	public DeadSound deadSound;

	private AudioSource boostSound;
	// Use this for initialization
	void Start ()
	{
		defaultDirection = transform.rotation;
		rend = gameObject.GetComponent<Renderer> ();
		gameManage = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		boostSound = gameObject.GetComponent<AudioSource> ();
		// Set player to the middle at the begining
		transform.position = defaultPosition;
		rb = GetComponent<Rigidbody2D> ();
		particle.Stop ();

		if (gameManage.GetLives () < gameManage.GetTotalLives ()) {
			timeSpentInvincible = 0;
			isInvincible = true;
		}
	}
	
	void Update ()
	{
		if (isInvincible) {
			//2
			timeSpentInvincible += Time.deltaTime;
			
			//3
			if (timeSpentInvincible < invincibleTime) {
				float remainder = timeSpentInvincible % .3f;
				rend.enabled = remainder > .15f; 
			}
			//4
			else {
				rend.enabled = true;
				isInvincible = false;
			}
		}
	

	}

	void Awake ()
	{
		// set Vector3 to ( camera left/right limits, spaceship Y, spaceship Z )
		// this will find a world-space point that is relative to the screen
		
		// using the camera's position from the origin (world-space Vector3(0,0,0)
		//leftConstraint = Camera.main.ScreenToWorldPoint( new Vector3(0.0f, 0.0f, 0 - Camera.main.transform.position.z) ).x;
		//rightConstraint = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width, 0.0f, 0 - Camera.main.transform.position.z) ).x;
		
		// using a specific distance
		leftConstraint = Camera.main.ScreenToWorldPoint (new Vector3 (0.0f, 0.0f, distanceZ)).x;
		upConstraint = Camera.main.ScreenToWorldPoint (new Vector3 (0.0f, Screen.height, distanceZ)).y;
		rightConstraint = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, 0.0f, distanceZ)).x;
		downConstraint = Camera.main.ScreenToWorldPoint (new Vector3 (0.0f, 0.0f, distanceZ)).y;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		
		if (Input.GetKey (KeyCode.LeftArrow)) {
//			Analytics.Get ().AddTrace ("Keypress", "Left", this.transform.position);
			transform.Rotate (0, 0, rotateSpeed * Time.deltaTime);
		}
		
		if (Input.GetKey (KeyCode.RightArrow)) {
//			Analytics.Get ().AddTrace ("Keypress", "Right", this.transform.position);
			
			transform.Rotate (0, 0, -rotateSpeed * Time.deltaTime);
		}
		
		if (Input.GetKey (KeyCode.UpArrow)) {
//			Analytics.Get ().AddTrace ("Keypress", "Forward", this.transform.position);
			
			rb.AddForce (transform.up * force);
			if (!boostSound.isPlaying) {
				boostSound.Play ();
			}

			if (particle.isStopped) {
				particle.Play ();
			}
		} else {
			particle.Stop ();
		}
		
		if (transform.position.x < leftConstraint - buffer) { // ship is past world-space view / off screen
			float newX = rightConstraint + buffer;
			transform.position = new Vector2 (newX, transform.position.y);  // move ship to opposite side
		}
		
		if (transform.position.x > rightConstraint + buffer) {
			float newX = leftConstraint - buffer;
			transform.position = new Vector2 (newX, transform.position.y);
		}
		if (transform.position.y > upConstraint + buffer) { // ship is past world-space view / off screen
			float newY = downConstraint - buffer;
			transform.position = new Vector2 (transform.position.x, newY);  // move ship to opposite side
		}
		
		if (transform.position.y < downConstraint - buffer) {
			float newY = upConstraint + buffer;
			transform.position = new Vector2 (transform.position.x, newY);
		}
		/**	
		if (transform.position.x > 10f || transform.position.x < -10f) {
//			Analytics.Get ().AddTrace ("Off-Screen", "Side", this.transform.position);
			
			Vector2 newPosition = new Vector2 (-transform.position.x, transform.position.y);
			transform.position = newPosition;
		}
		if (transform.position.y > 5f || transform.position.y < -5f) {
//			Analytics.Get ().AddTrace ("Off-Screen", "TopBottom", this.transform.position);
			
			Vector2 newPosition = new Vector2 (transform.position.x, -transform.position.y);
			transform.position = newPosition;
		}**/
		
	}

/*	public void SetInvincible()
	{
		isInvincible = true;
		InvokeRepeating("Blink", 0, 0.4);
		CancelInvoke( "SetDamageable" ); // in case the method has already been invoked
		Invoke( "SetDamageable", invincibleTime );
	}

	void Blink(){

	}
	
	void SetDamageable()
	{
		isInvincible = false;
	}*/

	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (!isInvincible) {
			if (other.tag == "BigRock" || other.tag == "MiddleRock" ||
				other.tag == "SmallRock" || other.tag == "TinyRock") {
				Instantiate (explosion, transform.position, Quaternion.identity);
				Instantiate (deadSound, transform.position, Quaternion.identity);
				Destroy (gameObject);
				gameManage.Dead ();
			}
		}
	}

	public void SetInvincible ()
	{
		isInvincible = true;
	}

	public void ResetPosition ()
	{
		transform.position = defaultPosition;
		transform.rotation = defaultDirection;
		rb.velocity = new Vector2 (0, 0);
	}
	
}
