using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	private AudioManager aud;
	private SaveManager global;

    // Start is called before the first frame update
    void Awake()
    {
		global = GameObject.FindGameObjectWithTag("Save").GetComponent<SaveManager>();
		aud = global.audmanager;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void PanelEnable(GameObject obj)
	{
		obj.SetActive(true);
	}

	public void PanelDisable(GameObject obj)
	{
		obj.SetActive(false);
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void ChangeMaster (float v)
	{
		aud.setMaster(v);
	}

	public void ChangeMusic(float v)
	{
		aud.setMusic(v);
	}

	public void ChangeSFX(float v)
	{
		aud.setSFX(v);
	}

	public void UpdateMenu()
	{
		aud.UpdateMenu();
	}
}
