using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Play()
	{
		SceneManager.LoadScene("TestScene");
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
}
