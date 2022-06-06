using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private int dash_cooldown;
	[SerializeField] private int dash_count;
	[SerializeField] private float dash_input_cooldown;

	public CursorControll cursor;
	public AimScript weapon;

	private Rigidbody2D rb2d;
	private PhysicsInterface p;

	private float x = 0, y = 0;
	private int dash_recovery;
	private int dash_stored;
	private float dash_input_timer;
	private Vector2 dash_input_last;

	// Start is called before the first frame update
	void Awake()
    {
		rb2d = GetComponent<Rigidbody2D>();
		p = GetComponent<PhysicsInterface>();
		dash_recovery = 0;
		dash_stored = dash_count;
    }

    // Update is called once per frame
    void FixedUpdate()
	{
		if (dash_recovery == 0)
		{
			if (dash_stored < dash_count)
			{
				dash_stored++;
				dash_recovery = dash_cooldown;
			}
		}else
		{
			dash_recovery--;
		}

		rb2d.velocity = p.Move(x, y);
	}

	void OnDash(InputValue v)
	{
		Vector2 input = v.Get<Vector2>();
		float time = Time.time;
		if (input.Equals(Vector2.zero)) return;
		if (!dash_input_last.Equals(input) || time - dash_input_timer >= dash_input_cooldown)
		{
			dash_input_last = input;
			dash_input_timer = Time.time;
		}else
		{
			if (dash_stored > 0)
			{
				//Debug.Log("dash to " + dash_input_last);
				rb2d.velocity = p.ForceSpeed(dash_input_last);
				dash_stored--;
				dash_input_last = Vector2.zero;
			}
		}
	}

	void OnJumpOn()
	{
		y = 1;
	}

	void OnJumpOff()
	{
		y = 0;
	}

	void OnMove(InputValue v)
	{
		x = v.Get<Vector2>().x;
	}

	void OnLook(InputValue v)
	{
		cursor.OnLook(v.Get<Vector2>());
	}

	void OnFire()
	{
		weapon.Shoot();
	}
}
