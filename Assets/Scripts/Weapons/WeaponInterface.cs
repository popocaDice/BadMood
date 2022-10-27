using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WeaponInterface
{
	float Shoot(float r);

	float SpecialShoot(float r);

	void idle();

	void shot();

	bool CanShoot();
}
