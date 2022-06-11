using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour, ProjectileInterface
{
	private float life;

	public float angle { get; set; }
	public float max_life;
	public float speed;
	public float knockback;

	void Awake()
	{
		life = 0;
	}

	private void FixedUpdate()
	{
		transform.position += Trajectory(life);
		life++;
	}

	public Vector3 Trajectory(float d)
	{
		if (d < max_life)
		{
			return Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.left * speed;
		}
		else
		{
			Collide();
			return Vector3.zero;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			if (collision.gameObject.GetComponent<CharacterPhysics>().Damage(1,
				Vector3.ClampMagnitude(collision.transform.position - transform.position, 1), knockback))
				collision.gameObject.GetComponent<PlayerController>().AddFury(500);
			Collide();
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
