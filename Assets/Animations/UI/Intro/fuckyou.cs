using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fuckyou : MonoBehaviour
{
	private void OnEnable()
	{
		GameObject.FindGameObjectWithTag("Save").GetComponent<SaveManager>().LoadScene("MainMenu");
	}
}
