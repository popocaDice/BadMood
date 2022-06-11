using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : MonoBehaviour
{
    public Transform body;
    private Rigidbody2D rb2d;
    private PhysicsInterface enemy;
    private bool back;
    public float speed;
    public float range;
    public Transform player;

    private float x = 0, y = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        back = false;
        enemy = GetComponent<PhysicsInterface>();
    }

    // Update is called once per frame
    void Update()
    {
        body.localScale = new Vector3(back ? 1 : -1, 1, 1);
        rb2d.velocity = enemy.Move(x, y);

        if (Vector2.Distance(transform.position, player.position) <= range)
        {
            if (player.position.x < body.position.x) rb2d.velocity = new Vector3(-speed, rb2d.velocity.y, 0);
            else rb2d.velocity = new Vector3(speed, rb2d.velocity.y, 0);

        }
        else
        {
            
        }

        if (rb2d.velocity.x > 0) back = false;
        else if (rb2d.velocity.x < 0) back = true;

    }
}
