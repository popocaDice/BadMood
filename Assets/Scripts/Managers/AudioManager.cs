using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	private float master, music, sfx;

	private float fadeSpeed;
	private List<GameObject> m;

	public GameObject MusicObject;

	private void Awake()
	{
		GameObject.FindGameObjectWithTag("Save").GetComponent<SaveManager>().OnLoadPref();
		setMaster(100);
		setMusic(100);
		setSFX(100);
		DontDestroyOnLoad(gameObject);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void UpdateMenu()
	{
		m[0].GetComponent<AudioSource>().volume = (music * master);
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{

		this.m = new List<GameObject>(GameObject.FindGameObjectsWithTag("Music"));
		//Debug.Log("found " + music.Length + " music objects");
		GameObject[] sfx = GameObject.FindGameObjectsWithTag("SFX");
		//Debug.Log("found " + sfx.Length + " sound objects");
		float m = this.music * master;
		float s = this.sfx * master;
		foreach (GameObject g in this.m)
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

	public void FadeMusic(AudioClip next, float speed)
	{
		fadeSpeed = speed;
		GameObject m = Instantiate<GameObject>(MusicObject);
		m.GetComponent<AudioSource>().clip = next;
		m.GetComponent<AudioSource>().volume = 0;
		m.GetComponent<AudioSource>().loop = true;
		m.GetComponent<AudioSource>().Play();
		this.m.Add(m);
	}

	private void FixedUpdate()
	{
		if (m.Count > 1)
		{
			m[0].GetComponent<AudioSource>().volume = 
				Mathf.Clamp(m[0].GetComponent<AudioSource>().volume - (fadeSpeed / 100), 0, master * music);
			m[1].GetComponent<AudioSource>().volume =
				Mathf.Clamp(m[1].GetComponent<AudioSource>().volume + (fadeSpeed / 100), 0, master * music);
			if (m[0].GetComponent<AudioSource>().volume <= 0)
			{
				Destroy(m[0].gameObject);
				m.RemoveAt(0);
			}
		}
	}
}
