using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorControll : MonoBehaviour
{

    private Animator anim;

	public Camera cam;
	public AimScript gun;

	private Vector2 cursor_pos;
	private float cam_height;
	private float cam_width;

    void Start()
    {
		cam_height = 2f * cam.orthographicSize - 1;
		cam_width = cam_height * cam.aspect + 0.5f;
		cursor_pos = Vector2.zero;
        Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
        anim = GetComponent<Animator>();
    }
	
    void Update()
    {

    }

    private void FixedUpdate()
    {
		cursor_pos.x = Mathf.Clamp(cursor_pos.x, cam.transform.position.x - cam_width / 2, cam.transform.position.x + cam_width / 2);
		cursor_pos.y = Mathf.Clamp(cursor_pos.y, cam.transform.position.y - cam_height / 2, cam.transform.position.y + cam_height / 2);
		transform.position = cursor_pos;
	}

	public void OnLook(Vector2 v)
	{
		cursor_pos += v;
	}

	public void OnFire()
	{
		anim.SetBool("click", true);
	}

	public void OnSpecial()
	{
		anim.SetBool("special", !anim.GetBool("special"));
	}

	public void OutFire()
	{
		anim.SetBool("click", false);
	}
}
