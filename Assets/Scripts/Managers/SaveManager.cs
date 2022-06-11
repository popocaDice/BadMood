using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
	public AudioManager audmanager;

	private GameObject icon;

	private void Awake()
	{
		icon = transform.GetChild(0).gameObject;
		GameObject.DontDestroyOnLoad(this);
	}

	public void OnSavePref()
	{
		icon.SetActive(true);
		PlayerPrefs.SetInt("AudioMaster", audmanager.getMaster());
		PlayerPrefs.SetInt("AudioMusic", audmanager.getMusic());
		PlayerPrefs.SetInt("AudioSounds", audmanager.getSFX());
		icon.SetActive(false);
	}

	public void OnLoadPref()
	{
		audmanager.setMaster(PlayerPrefs.GetInt("AudioMaster"));
		audmanager.setMusic(PlayerPrefs.GetInt("AudioMusic"));
		audmanager.setSFX(PlayerPrefs.GetInt("AudioSounds"));
	}

	public void OnSaveProgress()
	{

	}
}
