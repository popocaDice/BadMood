using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    private SaveManager global;

    private GameObject curLevel;

    void Awake()
    {
        global = GameObject.FindGameObjectWithTag("Save").GetComponent<SaveManager>();
        curLevel = GameObject.Instantiate(global.unlockedLevels[0], transform);
    }

    void Update()
    {
        global.curLevel = curLevel.GetComponent<LevelData>().firstPhase;
    }

	private void OnEnable()
	{
        if (global.unlockedLevels.Count <= 1)
		{
            global.curLevel = curLevel.GetComponent<LevelData>().firstPhase;
            WeaponSelect();
		}
	}

	public void WeaponSelect()
	{
        if (!global.unlockedQuickLevels.Contains(curLevel))
		{
            global.curWeapon = curLevel.GetComponent<LevelData>().defaultWeapon;
            global.LoadScene(global.curLevel);
		}
		else
		{

		}
	}
}
