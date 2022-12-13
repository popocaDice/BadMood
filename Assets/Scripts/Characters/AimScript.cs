using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimScript : MonoBehaviour
{
	public Transform cursor;
	public Transform center;
	public float recover_speed;

	public GameObject gunObject;

	private WeaponInterface weapon;
	private SaveManager global;

	private float angle;
	private float recoil;
	private bool back;

    void Awake()
    {
		//weapon = Instantiate<GameObject>(gunObject, transform).GetComponent<WeaponInterface>();
		recoil = 0;
		global = GameObject.FindGameObjectWithTag("Save").GetComponent<SaveManager>();
    }

	private void FixedUpdate()
	{
		if (global.pause) return;
		angle = (Mathf.Rad2Deg * Mathf.Atan2((center.position.y - cursor.position.y), (center.position.x - cursor.position.x)));
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

	public void SpawnWeapon(GameObject gun)
	{
		gunObject = gun;
		weapon = Instantiate<GameObject>(gunObject, transform).GetComponent<WeaponInterface>();
	}

	public void SwitchWeapon(GameObject gun)
	{
		gunObject = gun;
		weapon.Destroy();
		weapon = Instantiate<GameObject>(gunObject, transform).GetComponent<WeaponInterface>();
	}

	public bool CanShoot()
    {
		return weapon.CanShoot();
    }
}
