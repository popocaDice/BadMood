using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private int dash_cooldown;
	[SerializeField] private int dash_count;

	[SerializeField] private int max_fury;

	public CursorControll cursor;
	public AimScript weapon;

	private Rigidbody2D rb2d;
	private PhysicsInterface p;
	private SaveManager global;

	private float x = 0, y = 0;
	private int dash_recovery;
	private int dash_stored;
	private bool special;

	private int fury;
	private bool berserk;

	// Start is called before the first frame update
	void Awake()
	{
		global = GameObject.FindGameObjectWithTag("Save").GetComponent<SaveManager>();
		rb2d = GetComponent<Rigidbody2D>();
		p = GetComponent<PhysicsInterface>();
		dash_recovery = 0;
		dash_stored = dash_count;
		fury = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
	{
		if (global.pause)
		{
			rb2d.velocity = Vector2.zero;
			return;
		}
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

		if (berserk && GetFury() >= 0.76f) fury -= 1;
		else if (fury < max_fury)
		{
			fury += 1;
			berserk = false;
		}
		else berserk = true;
	}

	public void PlayerDeath()
	{
		p.Dead = true;
	}

	void OnDash()
	{
		if (dash_stored > 0 && p.Control())
		{
			rb2d.velocity = p.ForceSpeed(x, y + 0.1f);
			dash_stored--;
		}
	}

	void OnMove(InputValue v)
	{
		x = v.Get<Vector2>().x;
		y = v.Get<Vector2>().y;
	}

	void OnLook(InputValue v)
	{
		cursor.OnLook(v.Get<Vector2>());
	}

	void OnFire()
	{
		if (global.pause) return;
		if (special) weapon.SpecialShoot();
		else weapon.Shoot();
		cursor.OnFire();
	}

	void OnSpecial()
	{
		if (global.pause) return;
		cursor.OnSpecial();
	}

	public float GetFury()
	{
		return (float)fury/max_fury;
	}

	public void AddFury(int f)
	{
		fury += f;
	}

	public void ResetControls()
	{
		x = 0;
		y = 0;
		rb2d.velocity = p.ForceSpeed(x, y);
	}
}
