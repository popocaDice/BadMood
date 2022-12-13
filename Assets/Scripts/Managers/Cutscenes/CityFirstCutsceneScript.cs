using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityFirstCutsceneScript : MonoBehaviour
{
    private EventLoader e;

    private Vector3 targetCameraPosition;
    private float targetCameraSize;
    private bool cameraShake;

    public GameObject[] ListOfActors;
    public Vector3[] ListOfPositions;
    // Start is called before the first frame update
    void Start()
    {
        e = GetComponent<EventLoader>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (e.trigger) Play();

        if (cameraShake)
        {
            ListOfActors[4].transform.position = Vector3.Lerp(ListOfActors[4].transform.position, 
                targetCameraPosition + new Vector3 (Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), 0.1f);
            ListOfActors[4].GetComponent<Camera>().orthographicSize = Mathf.Lerp(ListOfActors[4].GetComponent<Camera>().orthographicSize, 
                targetCameraSize + Random.Range(-0.05f, 0.05f), 0.2f);

		}
		else
        {
            ListOfActors[4].transform.position = Vector3.Lerp(ListOfActors[4].transform.position, targetCameraPosition, 0.1f);
            ListOfActors[4].GetComponent<Camera>().orthographicSize = Mathf.Lerp(ListOfActors[4].GetComponent<Camera>().orthographicSize,
                targetCameraSize, 0.2f);
        }
    }

    void Play()
	{
        e.trigger = false;
        ListOfActors[0].SetActive(false);
        ListOfActors[1].SetActive(true);
        ListOfActors[1].transform.position = ListOfActors[2].transform.position;
        ListOfActors[2].GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        ListOfActors[2].GetComponent<SpriteRenderer>().sortingOrder = 50;
        ListOfActors[3].SetActive(false);
        ListOfActors[4].SetActive(true);
        targetCameraSize = 2;
        targetCameraPosition = ListOfPositions[0];
        ListOfActors[5].SetActive(false);

        Invoke("Call", 5);
    }

    void Call()
	{
        ListOfActors[1].GetComponent<Animator>().SetTrigger("MessageRecieved");
        ListOfActors[6].SetActive(true);
        targetCameraPosition = ListOfPositions[1];
        targetCameraSize = 4;

        Invoke("BossComeIn", 4);
    }

    void BossComeIn()
	{
        ListOfActors[6].GetComponent<Animator>().SetTrigger("BossComesIn");
        ListOfActors[1].GetComponent<Animator>().SetTrigger("BossComesIn");
        cameraShake = true;

        Invoke("BossLeaves", 4);
	}

    void BossLeaves()
	{
        cameraShake = false;
        ListOfActors[6].GetComponent<Animator>().SetTrigger("BossLeaves");
        ListOfActors[1].GetComponent<Animator>().SetTrigger("BossLeaves");

        Invoke("GetUp", 4);
    }

    void GetUp()
	{
        targetCameraPosition = ListOfPositions[2];
        targetCameraSize = 6;
        ListOfActors[1].GetComponent<Animator>().SetTrigger("GetUp");
        ListOfActors[6].SetActive(false);

        Invoke("End", 3);
    }

    void End()
    {
        Destroy(e.gameObject);
        ListOfActors[0].SetActive(true);
        ListOfActors[1].SetActive(false);
        ListOfActors[0].transform.position = ListOfActors[1].transform.position;
        ListOfActors[2].GetComponent<SpriteRenderer>().sortingLayerName = "Background";
        ListOfActors[2].GetComponent<SpriteRenderer>().sortingOrder = 10;
        ListOfActors[3].SetActive(true);
        ListOfActors[3].transform.position = ListOfActors[4].transform.position;
        ListOfActors[4].SetActive(false);
        ListOfActors[5].SetActive(true);
    }
}
