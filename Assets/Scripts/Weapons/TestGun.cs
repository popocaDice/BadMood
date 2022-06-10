using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGun : MonoBehaviour, WeaponInterface
{
	public float recoil;
	public float special_recoil;
	public float cooldown;
	public Transform projectile_spawn_position;
	public Sprite[] sprites;
	public Sprite[] sprites_shoot;

	public GameObject projectile;
	public GameObject special_projectile;

	private CharacterPhysics chph;
	private Animator anim;
	private Transform angle;
	private SpriteRenderer r;

	private int side;
	private float cool_countdown;

	private void Start()
	{
		angle = GetComponentInParent<Transform>();
		r = GetComponentInChildren<SpriteRenderer>();
		chph = GetComponentInParent<CharacterPhysics>();
		anim = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		cool_countdown -= cool_countdown > 0 ? 1 : 0;
		if ((transform.eulerAngles.z < 90 || transform.eulerAngles.z > 270))
		{
			side = 0;
			r.flipY = false;
		}
		else
		{
			side = 1;
			r.flipY = true;
		}
		//Debug.Log(side==0 ? "left" : "right");
	}

	public float Shoot(float r)
	{
		if (cool_countdown > 0) return r;
		anim.SetBool("shoot", true);
		GameObject p = Instantiate(projectile, projectile_spawn_position.position, Quaternion.identity) as GameObject;
		p.GetComponent<ProjectileInterface>().angle = GetComponentInParent<Transform>().rotation.eulerAngles.z;
		cool_countdown = cooldown;
		return recoil + r;
	}

	public float SpecialShoot(float r)
	{
		if (cool_countdown > 0) return r;
		anim.SetBool("shoot", true);
		GameObject p = Instantiate(projectile, projectile_spawn_position.position, Quaternion.identity) as GameObject;
		p.GetComponent<ProjectileInterface>().angle = GetComponentInParent<Transform>().rotation.eulerAngles.z;
		cool_countdown = cooldown;
		return special_recoil + r;
	}

	public void idle()
	{
		r.sprite = sprites[chph.body.localScale.x>0?side:1-side];
		r.sortingOrder = (int) (chph.body.localScale.x * 5);
		anim.SetBool("shoot", false);
	}

	public void shot()
	{
		r.sprite = sprites_shoot[chph.body.localScale.x > 0 ? side : 1 - side];
	}
}
