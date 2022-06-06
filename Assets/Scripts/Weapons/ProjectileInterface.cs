using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ProjectileInterface
{
	float angle { get; set; }

	Vector3 Trajectory(float d);

	void Collide();
}
