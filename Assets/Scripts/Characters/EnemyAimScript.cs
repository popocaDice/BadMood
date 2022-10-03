using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAimScript : MonoBehaviour
{
	public Transform center;
	public float recover_speed;

	private WeaponInterface weapon;
	private Transform target;
	private SaveManager global;

	private float angle;
	private float recoil;
	private bool back;

    void Awake()
	{
		global = GameObject.FindGameObjectWithTag("Save").GetComponent<SaveManager>();
		weapon = GetComponentInChildren<WeaponInterface>();
		target = center;
		recoil = 0;
    }

	private void FixedUpdate()
	{
		if (global.pause) return;
		angle = (Mathf.Rad2Deg * Mathf.Atan2((center.position.y - target.position.y), (center.position.x - target.position.x)));
		angle += Mathf.Abs(angle) > 90 ? recoil : -recoil;
		recoil = recoil > 0 ? recoil - recover_speed : 0;
	}

	private void Update()
    {
		if (global.pause) return;
		transform.eulerAngles = Vector3.forward * angle;
    }

	public void Shoot()
	{
		recoil = weapon.Shoot(recoil);
	}

	public void SpecialShoot()
	{
		recoil = weapon.SpecialShoot(recoil);
	}

	public void See(Transform t)
	{
		target = t;
	}

	public void Unsee()
	{
		target = center;
	}
}
