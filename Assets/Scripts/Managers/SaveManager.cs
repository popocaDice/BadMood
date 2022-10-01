using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveManager : MonoBehaviour
{
	public AudioManager audmanager;

	public GameObject curWeapon;

	private GameObject icon;
	private GameObject player;

	private void Start()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		icon = transform.GetChild(0).gameObject;
	}

	public void OnSavePref()
	{
		icon.SetActive(true);
		PlayerPrefs.SetFloat("AudioMaster", audmanager.getMaster());
		PlayerPrefs.SetFloat("AudioMusic", audmanager.getMusic());
		PlayerPrefs.SetFloat("AudioSounds", audmanager.getSFX());
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

	public void SelectWeapon(GameObject weapon)
	{
		curWeapon = weapon;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		player = GameObject.FindGameObjectWithTag("Player");
		if (player != null) player.GetComponent<PlayerController>().weapon.SpawnWeapon(curWeapon);
	}
}
