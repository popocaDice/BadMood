using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    private float born;

    // Start is called before the first frame update
    void Start()
    {
        born = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time - born > 10)
        {
            Destroy(GetComponentInChildren<SpriteRenderer>());
            Destroy(GetComponent<Rigidbody2D>());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(GetComponentInChildren<SpriteRenderer>());
        Destroy(GetComponent<Rigidbody2D>());
    }
}
