using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform focus1;
    public Transform focus2;
    public float distance;
    public float smoothing;

	private Vector2 position;
	private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
		rb2d = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
		position = focus1.position + Vector3.ClampMagnitude(focus2.position - focus1.position, distance);

		rb2d.MovePosition(Vector2.Lerp(transform.position, position, smoothing));
    }
}
