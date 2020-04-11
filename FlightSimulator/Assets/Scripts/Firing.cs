using UnityEngine;

public class Firing : MonoBehaviour
{
	public GameObject BulletObject;
	public float FireRate;
	public int BulletVelocity  = 500;

	float nextFire = 0.0f;

	void Start()
	{
	}

	/// <summary>
	/// Check whether conditions to fire the gun are satisfied. If they are, consider firing.
	/// </summary>
	void FixedUpdate()
	{
		if (Input.GetButton("Jump"))
			ConsiderFiring();
	}

	/// <summary>
	/// Check whether we have clock permission to fire. If so, fire and reset clock permission.
	/// </summary>
	public void ConsiderFiring()
	{
		if (Time.time > nextFire)
		{
			GameObject newBullet = Instantiate(BulletObject, transform.position, transform.rotation) as GameObject;
			newBullet.GetComponent<Rigidbody>().velocity = -transform.up * BulletVelocity;
			nextFire = Time.time + FireRate;
		}
	}
}