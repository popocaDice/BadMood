using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public GameObject pause;

	private SaveManager global;

	private void Awake()
	{
		global = GameObject.FindGameObjectWithTag("Save").GetComponent<SaveManager>();
	}

	public void OnCancel()
	{
		if (global.pause) return;
		global.pause = true;
		pause.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public void PanelEnable(GameObject obj)
	{
		obj.SetActive(true);
	}

	public void PanelDisable(GameObject obj)
	{
		obj.SetActive(false);
	}

	public void Resume()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		global.pause = false;
		pause.SetActive(false);
	}

	public void ReturnToMainMenu()
	{
		global.pause = false;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		global.pause = false;
		SceneManager.LoadScene("MainMenu");
	}

	public void CloseGame()
	{
		Application.Quit();
	}
}
