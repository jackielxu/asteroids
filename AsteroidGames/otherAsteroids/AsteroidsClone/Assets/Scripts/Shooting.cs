using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour
{
	
	public Rigidbody2D projectile;
	public float bulletSpeed = 20f;

    private float bulletLifeTime = 0.4f;
    private GameManager gameManager;
	private RockProducer rockProducer;
	private AudioSource shootSource;

	void Start ()
	{
		shootSource = GetComponent<AudioSource> ();
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		rockProducer = GameObject.Find ("RockProducer").GetComponent<RockProducer> ();
	}
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space) && !gameManager.CheckFinal () && !rockProducer.IsFinished ()) {
            shootSource.Play();
            Rigidbody2D clone;
            if (!rockProducer.autoShootingMode())
            {
                clone = Instantiate(projectile, transform.position, transform.rotation) as Rigidbody2D;
                clone.velocity = transform.TransformDirection(Vector2.up * bulletSpeed);
                Destroy(clone.gameObject, bulletLifeTime);
            }
            else
            {
                float autoShootingAngle = rockProducer.autoShootingAngle();
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bulletSpeed * bulletLifeTime);
                if (colliders.Length > 1)
                {
                    Collider2D nearestCollider = null;
                    float nearestDistance = float.MaxValue;
                    foreach (Collider2D currentCollider in colliders){
                        if (!currentCollider.tag.Equals("Player") && !currentCollider.tag.Equals("Bullet") && Vector2.Angle(currentCollider.transform.position - transform.position, transform.up) < autoShootingAngle / 2)
                        {
                            float dist = Vector2.Distance(transform.position, currentCollider.transform.position);
                            if (dist < nearestDistance)
                            {
                                nearestDistance = dist;
                                nearestCollider = currentCollider;
                            }
                            
                        }
                    }

                    if (nearestCollider != null)
                    {
                        GameObject target = nearestCollider.gameObject; // Target

                        Vector2 targetVel = nearestCollider.GetComponent<Rigidbody2D>().velocity; // The velocity of target

                        Vector2 dirToTarget = (target.transform.position - transform.position).normalized; // The direction from bullet to target

                        // Decompose the target's velocity into the part parallel to the
                        // direction to the cannon and the part tangential to it.
                        // The part towards the cannon is found by projecting the target's
                        // velocity on dirToTarget using a dot product.
                        Vector2 targetVelOrth =
                        Vector2.Dot(targetVel, dirToTarget) * dirToTarget; 

                        // The tangential part is then found by subtracting the
                        // result from the target velocity.
                        Vector2 targetVelTang = targetVel - targetVelOrth;

                        // The tangential component of the velocities should be the same
                        // (or there is no chance to hit)
                        // THIS IS THE MAIN INSIGHT!
                        Vector2 shotVelTang = targetVelTang;

                        Vector2 shotVel = Vector2.zero;

                        float shotVelSpeed = shotVelTang.magnitude;
                        if (shotVelSpeed > bulletSpeed)
                        {
                            // Shot is too slow to intercept target, it will never catch up.
                            // Do our best by aiming in the direction of the targets velocity.
                            shotVel  = targetVel.normalized * bulletSpeed;
                        }
                        else
                        {
                            // We know the shot speed, and the tangential velocity.
                            // Using pythagoras we can find the orthogonal velocity.
                            float shotSpeedOrth =
                            Mathf.Sqrt(bulletSpeed * bulletSpeed - shotVelSpeed * shotVelSpeed);
                            Vector2 shotVelOrth = dirToTarget * shotSpeedOrth;

                            // Finally, add the tangential and orthogonal velocities.
                            shotVel =  shotVelOrth + shotVelTang;
                        }

                        clone = Instantiate(projectile, transform.position, transform.rotation) as Rigidbody2D;
                        clone.velocity =shotVel;
                        Destroy(clone.gameObject, bulletLifeTime);
                    }
                    else
                    {
                        clone = Instantiate(projectile, transform.position, transform.rotation) as Rigidbody2D;
                        clone.velocity = transform.TransformDirection(Vector2.up * bulletSpeed);
                        Destroy(clone.gameObject, bulletLifeTime);
                    }
                    
                }
            }
		}
	}
}
