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
		//Time.timeScale = 1 - Time.timeScale;
		global.pause = !global.pause;
		pause.SetActive(!pause.activeInHierarchy);
		Cursor.lockState = (Cursor.lockState == CursorLockMode.None) ? CursorLockMode.Locked : CursorLockMode.None;
	}

	public void PanelEnable(GameObject obj)
	{
		obj.SetActive(true);
	}

	public void PanelDisable(GameObject obj)
	{
		obj.SetActive(false);
	}

	public void ReturnToMainMenu()
	{
		//Time.timeScale = 1;
		global.pause = false;
		SceneManager.LoadScene("MainMenu");
	}

	public void CloseGame()
	{
		Application.Quit();
	}
}
