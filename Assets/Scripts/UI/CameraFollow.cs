using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform focus1;
    public Transform focus2;
    public float distance;
    public float smoothing;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    void FixedUpdate()
    {
		Vector2 position = focus1.position + Vector3.ClampMagnitude(focus2.position - focus1.position, distance);
		transform.position = Vector2.Lerp(transform.position, position, smoothing);
    }
}
