using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
	public Image img;
	public Animator head;

	private CharacterPhysics pc;

    // Start is called before the first frame update
    void Start()
    {
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterPhysics>();
    }

    // Update is called once per frame
    void Update()
    {
		head.SetInteger("health", (int)pc.GetHealth());
		if (pc.GetHealth() == 3)
		{
			img.fillAmount = 1;
		}
		else if (pc.GetHealth() == 2)
		{
			img.fillAmount = 0.66f;
		}
		else if (pc.GetHealth() == 1)
		{
			img.fillAmount = 0.3f;
		}
		else if (pc.GetHealth() == 0)
		{
			img.fillAmount = 0;
		}
    }
}
