using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuCamera : MonoBehaviour
{
	public GameObject panel;
	public RectTransform background;
	public Camera cam;
	public float smooth;

	public bool blockView;

	private float cam_height, cam_width, bg_width, bg_height;
	private Vector3[] bg_corners = new Vector3[4];
	private Vector2 mouse_pos = Vector2.zero;
	
	// Start is called before the first frame update
	void Start()
    {
		cam_height = cam.orthographicSize * 2;
		cam_width = cam_height * cam.aspect;
		background.GetWorldCorners(bg_corners);
		bg_height = Vector3.Distance(bg_corners[0], bg_corners[1]);
		bg_width = Vector3.Distance(bg_corners[0], bg_corners[3]);
	}

	// Update is called once per frame
	void Update()
	{
		if (blockView) return;
		cam.transform.position = Vector2.Lerp(cam.transform.position, mouse_pos, smooth/10);
		panel.transform.position = new Vector3(-cam.transform.position.x/3, -cam.transform.position.y/3, panel.transform.position.z);
	}

	void OnPoint(InputValue v)
	{
		mouse_pos = cam.ScreenToWorldPoint(v.Get<Vector2>())/10;
		mouse_pos.x = Mathf.Clamp(mouse_pos.x, -((bg_width/2) - (cam_width/2)), ((bg_width/2) - (cam_width/2)));
		mouse_pos.y = Mathf.Clamp(mouse_pos.y, -((bg_height/2) - (cam_height/2)), ((bg_height/2) - (cam_height/2)));
	}

	public void BlockView(bool v)
	{
		blockView = v;
		if (v) cam.transform.position = Vector2.zero;
	}
}
