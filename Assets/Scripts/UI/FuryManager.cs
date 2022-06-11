using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuryManager : MonoBehaviour
{
	private Image img;
	private Animator bar;
	private PlayerController pc;

	// Start is called before the first frame update
	void Start()
	{
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		bar = GetComponent<Animator>();
		img = GetComponent<Image>();
	}

    // Update is called once per frame
    void Update()
    {
		img.fillAmount = pc.GetFury();
		bar.SetFloat("Fill", pc.GetFury());
    }
}
