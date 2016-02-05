using UnityEngine;
using System.Collections;

public class TinyRock : MonoBehaviour
{
	private float speed = 250f;
	private Rigidbody2D rb;
	private Vector2 randomDir;
	private float leftConstraint;
	private float rightConstraint;
	private float upConstraint;
	private float downConstraint;
	public float distanceZ = 10.0f;
	private float buffer = 0.5f;
	private RockProducer rockProducer;

	public ExplosionSound explosionSound;
	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	
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
	
	void FixedUpdate ()
	{
		rb.velocity = randomDir * speed * Time.deltaTime;
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
	}
	
	public void SetDir (Vector2 Dir)
	{
		randomDir = Dir;
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Bullet" || other.tag == "Player") {
			rockProducer = GameObject.Find ("RockProducer").GetComponent<RockProducer> ();
			rockProducer.minusRockNumber ();
			Instantiate (explosionSound, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
	}
	
	
	
}
