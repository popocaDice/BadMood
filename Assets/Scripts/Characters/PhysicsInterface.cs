using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PhysicsInterface
{
	bool Dead { get; set; }

	Vector2 Move(float x, float y);

	Vector2 Move(Vector2 v);

	Vector2 ForceSpeed(float x, float y, float speed);

	Vector2 ForceSpeed(Vector2 v, float speed);

	Vector2 ForceSpeed(float x, float y);

	Vector2 ForceSpeed(Vector2 v);

	float GetVertSpeed();

	bool Damage(float d);

	bool Damage(float d, Vector2 direction, float intensity);

	void GroundCheckIn();

	void GroundCheckOut();

	void CeilingCheckIn();

	void CeilingCheckOut();

	void Invoke(string name);

	bool Control();

	float GetHealth();

	float GetMaxHealth();
}
