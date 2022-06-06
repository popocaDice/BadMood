using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimScript : MonoBehaviour
{
	public Transform cursor;
	public Transform center;
	public float recover_speed;

	private WeaponInterface weapon;

	private float angle;
	private float recoil;
	private bool back;

    void Start()
    {
		weapon = GetComponentInChildren<WeaponInterface>();
		recoil = 0;
    }

	private void FixedUpdate()
	{
		angle = (Mathf.Rad2Deg * Mathf.Atan2((center.position.y - cursor.position.y), (center.position.x - cursor.position.x)));
		angle += Mathf.Abs(angle) > 90 ? recoil : -recoil;
		recoil = recoil > 0 ? recoil - recover_speed : 0;
	}

	private void Update()
    {
		transform.eulerAngles = Vector3.forward * angle;
    }

	public void Shoot()
	{
		recoil = weapon.Shoot(recoil);
	}
}
