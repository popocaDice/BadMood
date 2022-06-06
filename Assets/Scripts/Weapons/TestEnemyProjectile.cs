using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyProjectile : MonoBehaviour, ProjectileInterface
{
	private float life;

	public float angle { get; set; }
	public float max_life;
	public float knockback;

	void Awake()
	{
		life = 0;
	}

	private void FixedUpdate()
	{
		transform.position += Trajectory(life);
	}

	public Vector3 Trajectory(float d)
	{
		return Vector3.zero;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			collision.gameObject.GetComponent<CharacterPhysics>().Damage(1,
				Vector3.ClampMagnitude(collision.transform.position - transform.position, 1), knockback);
			//Colide();
		}
		else if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Obstacle")
		{
			Collide();
		}
	}

	public void Collide()
	{
		Destroy(gameObject);
	}
}
