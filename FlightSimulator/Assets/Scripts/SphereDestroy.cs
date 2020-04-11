using UnityEngine;
using System;

public class SphereDestroy : MonoBehaviour
{
	public GameObject Explosion;
	public int TimeToReAppear = 5;

	public bool isHit;
	private DateTime? hitTime;
	private float timer;

	private void Start()
	{
		hitTime = null;
		isHit = false;
		timer = 0;
	}

	private void Update()
	{
		if (!isHit)
		{
			return;
		}

		var timeDiff = DateTime.UtcNow - hitTime;
		timer += Time.deltaTime;
		// Debug.Log("Time diff: " + timeDiff?.Seconds);

		//if (timeDiff?.Seconds > TimeToReAppear)
		if (timer > TimeToReAppear)
		{
			GetComponent<MeshRenderer>().enabled = true;
			transform.GetComponent<SphereCollider>().enabled = true;
			isHit = false;
		}
	}

	void OnTriggerEnter(Collider impactObject)
	{
		if (impactObject.tag == "Bullet")
		{
			// Debug.Log("Object collided");
			Instantiate(Explosion, transform.position, transform.rotation);
			//Destroy(gameObject);
			hitTime = DateTime.Now;
			if (!isHit)
			{
				ScoreCount.Score++;
				timer = 0;
				isHit = true;
				transform.GetComponent<MeshRenderer>().enabled = false;
				transform.GetComponent<SphereCollider>().enabled = false;
			}
		}
	}
}