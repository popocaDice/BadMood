using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLoader : MonoBehaviour
{
    private SaveManager global;

    public bool cutscene;
    public string sceneToLoad;

    public bool trigger;

    // Start is called before the first frame update
    void Start()
    {
        global = GameObject.FindGameObjectWithTag("Save").GetComponent<SaveManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (!cutscene) global.LoadScene(sceneToLoad);
        else trigger = true;
	}
}
