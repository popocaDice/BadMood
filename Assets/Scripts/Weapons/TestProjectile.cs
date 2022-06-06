using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectile : MonoBehaviour, ProjectileInterface
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
		}else
		{
			Collide();
			return Vector3.zero;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		life = max_life;
	}

	public void Collide()
	{
		Destroy(gameObject);
	}
}
