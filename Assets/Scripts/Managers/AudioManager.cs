using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	private float master, music, sfx;

	public AudioSource menu;

	private void Awake()
	{
		GameObject.FindGameObjectWithTag("Save").GetComponent<SaveManager>().OnLoadPref();
		GameObject.DontDestroyOnLoad(this);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		//Debug.Log("started");

		GameObject[] music = GameObject.FindGameObjectsWithTag("Music");
		//Debug.Log("found " + music.Length + " music objects");
		GameObject[] sfx = GameObject.FindGameObjectsWithTag("SFX");
		//Debug.Log("found " + sfx.Length + " sound objects");
		float m = this.music * master;
		float s = this.sfx * master;
		foreach (GameObject g in music)
		{
			g.GetComponent<AudioSource>().volume = m;
		}
		foreach (GameObject g in sfx)
		{
			g.GetComponent<AudioSource>().volume = s;
		}
	}

	public void setMaster(float v)
	{
		master = v/100;
	}

	public int getMaster()
	{
		return (int) master * 100;
	}

	public void setMusic(float v)
	{
		music = v/100;
	}

	public int getMusic()
	{
		return (int) music * 100;
	}

	public void setSFX(float v)
	{
		sfx = v/100;
	}

	public int getSFX()
	{
		return (int) sfx * 100;
	}
}
