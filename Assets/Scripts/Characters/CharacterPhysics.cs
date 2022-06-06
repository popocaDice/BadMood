using System.Collections;
using UnityEngine;

public class CharacterPhysics : MonoBehaviour, PhysicsInterface
{
	public float max_speed;
	public float stillness_speed;
	public float acceleration;
	public float air_acceleration;
	public float max_fall_speed;
	public float jump_speed;
	public float jump_sustain;
	public float gravity;
	public float dash_mult;
	public float max_health;
	public float invul_duration;
	public float invul_blink_duration;

	private bool grounded;
	private bool ceiling;
	private bool jump;
	private float jump_sustained;
	private bool control = true;
	private float health;
	private bool invulnerable;

	public Transform body;

	private Vector2 v;

	void Awake()
	{
		health = max_health;
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

		jump = y>0 && (grounded || jump);
		if (jump && jump_sustained < jump_sustain)
		{
			v.y = jump_speed - (jump_sustained * gravity / 5);
			jump_sustained++;
		}else if (!grounded)
		{
			if (v.y > 0)
			{
				v.y -= gravity / 2;
			}
			else if (v.y > -max_fall_speed)
			{
				v.y -= gravity;
			}
		}

	}

	public Vector2 Move(Vector2 v)
	{
		return Move(v.x, v.y);
	}

	private void Move(float x)
	{
		if (control)
		{
			if (grounded && v.x <= max_speed)
			{
				v.x = Mathf.Lerp(v.x, x * max_speed, acceleration);
			}
			else
			{
				v.x = Mathf.Lerp(v.x, x*max_speed, air_acceleration);
			}
		}
		else
		{
			v.x = Mathf.Lerp(v.x, 0, air_acceleration);
			if (v.magnitude <= max_speed / stillness_speed)
			{
				control = true;
			}
		}

		if (v.x > 0)
		{
			body.localScale = new Vector3(-1, 1, 1);
		}
		else if (v.x < 0)
		{
			body.localScale = new Vector3(1, 1, 1);
		}
	}

	public Vector2 ForceSpeed(float x, float y, float speed)
	{
		v.x = x*speed;
		if (y == 0) y = v.y + y*speed;
		else v.y = y*speed;
		return v;
	}

	public Vector2 ForceSpeed(Vector2 v, float speed)
	{
		return ForceSpeed(v.x, v.y, speed);
	}

	public Vector2 ForceSpeed(float x, float y)
	{
		return ForceSpeed(x, y, max_speed*dash_mult);
	}

	public Vector2 ForceSpeed(Vector2 v)
	{
		return ForceSpeed(v.x, v.y, max_speed*dash_mult);
	}

	public void Damage(float d)
	{
		if (invulnerable) return;
		health -= d;
	}

	public void Damage(float d, Vector2 direction, float intensity)
	{
		if (invulnerable) return;
		health -= d;
		jump = false;
		control = false;
		invulnerable = true;
		foreach (SpriteRenderer s in GetComponentsInChildren<SpriteRenderer>()) StartCoroutine(Blink(s));
		ForceSpeed(direction, max_speed * intensity);
		Debug.Log("Tomou dano! Vida total: " + health);
	}

	public void GroundCheckIn()
	{
		grounded = true;
		jump_sustained = 0;
		if (control) v.y = 0;
		else v.y = -v.y/2;
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
}
