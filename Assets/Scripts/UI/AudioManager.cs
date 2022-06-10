using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	private float master, music, sfx;

	public AudioSource menu;

	private void Start()
	{
		setMaster(100);
		setMusic(100);
		setSFX(100);
		GameObject.DontDestroyOnLoad(this);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Debug.Log("started");

		GameObject[] music = GameObject.FindGameObjectsWithTag("Music");
		Debug.Log("found " + music.Length + " music objects");
		GameObject[] sfx = GameObject.FindGameObjectsWithTag("SFX");
		Debug.Log("found " + sfx.Length + " sound objects");
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
		menu.volume = master * music;
	}

	public void setMusic(float v)
	{
		music = v/100;
		menu.volume = master * music;
	}

	public void setSFX(float v)
	{
		sfx = v/100;
	}
}
