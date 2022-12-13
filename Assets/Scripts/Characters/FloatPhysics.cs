using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatPhysics : MonoBehaviour, PhysicsInterface
{
	public float max_speed;
	public float stillness_speed;
	public float acceleration;
	public float max_health;
	public float invul_duration;
	public float invul_blink_duration;
	public bool fall_loss_control;
	public bool Dead { get; set; }

	private bool grounded;
	private bool ceiling;
	private bool control = true;
	private float health;
	private bool invulnerable;

	public Transform body;

	private Animator anim;

	private Vector2 v;

	void Awake()
	{
		anim = GetComponent<Animator>();
		health = max_health;
		Dead = false;
	}

	private IEnumerator Blink(SpriteRenderer renderer)
	{

		for (int i = 0; i < invul_duration; i++)
		{
			renderer.enabled = !renderer.enabled;
			yield return new WaitForSeconds(invul_blink_duration);
		}
		renderer.enabled = true;
		invulnerable = false;
	}

	public Vector2 Move(float x, float y)
	{
		Move(x);
		Jump(y);
		return v;
	}

	private void Jump(float y)
	{
		v.y = Mathf.Lerp(v.y, y * max_speed, acceleration);
	}

	public Vector2 Move(Vector2 v)
	{
		return Move(v.x, v.y);
	}

	private void Move(float x)
	{
		v.x = Mathf.Lerp(v.x, x * max_speed, acceleration);
	}

	public Vector2 ForceSpeed(float x, float y, float speed)
	{
		v.x = x * speed;
		if (y == 0) y = v.y + y * speed;
		else v.y = y * speed;
		return v;
	}

	public Vector2 ForceSpeed(Vector2 v, float speed)
	{
		return ForceSpeed(v.x, v.y, speed);
	}

	public Vector2 ForceSpeed(float x, float y)
	{
		return ForceSpeed(x, y, max_speed);
	}

	public Vector2 ForceSpeed(Vector2 v)
	{
		return ForceSpeed(v.x, v.y, max_speed);
	}

	public float GetVertSpeed()
	{
		return v.y;
	}

	public bool Damage(float d)
	{
		if (invulnerable) return false;
		health -= d;
		return true;
	}

	public bool Damage(float d, Vector2 direction, float intensity)
	{
		if (invulnerable) return false;
		health -= d;
		control = false;
		invulnerable = true;
		foreach (SpriteRenderer s in GetComponentsInChildren<SpriteRenderer>()) StartCoroutine(Blink(s));
		ForceSpeed(direction, max_speed * intensity);
		//Debug.Log("Tomou dano! Vida total: " + health);
		return true;
	}

	public void GroundCheckIn()
	{
		grounded = true;
		if (control) v.y = 0;
		else v.y = -v.y / 2;
	}

	public void GroundCheckOut()
	{
		grounded = false;
	}

	public void CeilingCheckIn()
	{
		ceiling = true;
		v.y = 0;
	}

	public void CeilingCheckOut()
	{
		ceiling = false;
	}

	public void Invoke(string name)
	{
		Invoke(name, 0f);
	}

	public float GetHealth()
	{
		return health;
	}

	public float GetMaxHealth()
	{
		return max_health;
	}

	public bool Control()
	{
		return control;
	}

	private void Die()
	{
		anim.SetTrigger("die");
		anim.enabled = false;
		Debug.Log("dead");
		control = false;
		Dead = true;
	}

	public Transform Body()
	{
		return body;
	}

	void Update()
	{
		if (health <= 0) Die();
	}
}
