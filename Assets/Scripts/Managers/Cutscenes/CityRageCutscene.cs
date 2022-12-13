using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityRageCutscene : MonoBehaviour
{
    private EventLoader e;

    private Vector3 targetCameraPosition;
    private float targetCameraSize;
    private bool cameraShake;
    private float followPlayer;
    private SaveManager global;

    public GameObject[] ListOfActors;
    public Vector3[] ListOfPositions;
    public AudioClip[] ListOfAudioClips;

    public GameObject newGun;
    // Start is called before the first frame update
    void Start()
    {
        e = GetComponent<EventLoader>();
        global = GameObject.FindGameObjectWithTag("Save").GetComponent<SaveManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (e.trigger) Play();

        if (followPlayer > 0)
		{
            targetCameraPosition = ListOfActors[1].transform.position + (Vector3.up * followPlayer);
		}

        if (cameraShake)
        {
            ListOfActors[3].transform.position = Vector3.Lerp(ListOfActors[3].transform.position,
                targetCameraPosition + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), 0.05f);
            ListOfActors[3].GetComponent<Camera>().orthographicSize = Mathf.Lerp(ListOfActors[3].GetComponent<Camera>().orthographicSize,
                targetCameraSize + Random.Range(-0.05f, 0.05f), 0.1f);

        }
        else
        {
            ListOfActors[3].transform.position = Vector3.Lerp(ListOfActors[3].transform.position, targetCameraPosition, 0.05f);
            ListOfActors[3].GetComponent<Camera>().orthographicSize = Mathf.Lerp(ListOfActors[3].GetComponent<Camera>().orthographicSize,
                targetCameraSize, 0.1f);
        }
    }

    void Play()
    {
        ListOfActors[0].SetActive(false);
        ListOfActors[2].SetActive(false);
        ListOfActors[3].SetActive(true);
        ListOfActors[3].transform.position = ListOfActors[2].transform.position;
        ListOfActors[3].GetComponent<Camera>().orthographicSize = 10;
        targetCameraPosition = ListOfPositions[0];
        targetCameraSize = 4;
        ListOfActors[4].SetActive(false);
        ListOfActors[5].SetActive(true);

        e.trigger = false;
        Invoke("WalkUp", 5f);
    }

    void WalkUp()
    {
        ListOfActors[1].SetActive(true);
        ListOfActors[6].SetActive(true);
        followPlayer = 2;
        targetCameraSize = 3;

        Invoke("BossBegin", 10);
	}

    void BossBegin()
	{
        followPlayer = 0;
        ListOfActors[6].GetComponent<Animator>().SetTrigger("PlayerArrived");
        ListOfActors[7].SetActive(true);
        ListOfActors[7].GetComponent<ParticleSystem>().Play();

        Invoke("BossGetUp", 3);
	}

    void BossGetUp()
	{
        targetCameraSize = 2;
        targetCameraPosition = ListOfPositions[1];
        ListOfActors[6].GetComponent<Animator>().SetTrigger("GetUp");
        global.audmanager.FadeMusic(ListOfAudioClips[0], 0.1f);

        Invoke("BossEmote", 4);
	}

    void BossEmote()
	{
        targetCameraSize = 1.2f;
        targetCameraPosition = ListOfPositions[2];
        ListOfActors[1].GetComponent<Animator>().SetTrigger("Annoyed");
        ListOfActors[6].GetComponent<Animator>().SetTrigger("Emote");
        ChangeSort(ListOfActors[5].transform, SortingLayer.NameToID("BackGround"));
        ListOfActors[5].GetComponent<Animator>().SetTrigger("WalkPast");

        Invoke("ZoomIn", 3);
    }

    void ZoomIn()
	{
        targetCameraSize = 0.7f;
        targetCameraPosition = ListOfPositions[3];

        Invoke("FinalZoom", 3);
	}

    void FinalZoom()
	{
        targetCameraSize = 0.5f;
        targetCameraPosition = ListOfPositions[4];

        Invoke("Jump", 5);
	}

    void Jump()
	{
        targetCameraPosition = ListOfPositions[1];
        targetCameraSize = 2.9f;
        ListOfActors[7].SetActive(false);
        ListOfActors[1].GetComponent<Animator>().SetTrigger("Jump");
        ListOfActors[6].GetComponent<Animator>().SetTrigger("GetJumped");
        ListOfActors[8].GetComponent<Animator>().SetTrigger("fall");
        global.audmanager.FadeMusic(ListOfAudioClips[1], 0.1f);

        Invoke("End", 12);
    }

    void End()
	{
        Destroy(e.gameObject);

        ListOfActors[0].SetActive(true);
        ListOfActors[0].GetComponent<PlayerController>().ResetControls();
        ListOfActors[0].transform.position = ListOfPositions[5];
        global.SelectWeapon(newGun);
        ListOfActors[0].GetComponent<PlayerController>().weapon.SwitchWeapon(newGun);
        ListOfActors[1].SetActive(false);
        ListOfActors[2].SetActive(true);
        ListOfActors[3].SetActive(false);
        ListOfActors[2].transform.position = ListOfActors[3].transform.position;
        ListOfActors[4].SetActive(true);
        ListOfActors[9].SetActive(true);
        ListOfActors[10].SetActive(true);
    }

    private void ChangeSort(Transform part, int id)
    {
        //Debug.Log(part.gameObject.name);
        foreach (Transform p in part)
        {
            if (p.childCount > 0) ChangeSort(p, id);
            if (p.gameObject.GetComponent<SpriteRenderer>() == null)
            {
                continue;
            }

            p.gameObject.GetComponent<SpriteRenderer>().sortingLayerID = id;
        }
    }
}
