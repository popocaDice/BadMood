using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : MonoBehaviour
{
    public Transform enemyTransform;
    private Rigidbody2D rb2d;
    private PhysicsInterface enemy;
    private bool back;
    public float speed;
    public float detectionRange;
    public float attackRange;
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
        enemyTransform.localScale = new Vector3(back ? 1 : -1, 1, 1);
        rb2d.velocity = enemy.Move(x, y);

        //check if player is in detection range
        if (Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            //check if player is out of attack range
            if (Vector2.Distance(transform.position, player.position) > attackRange)
            {
                if (player.position.x < enemyTransform.position.x) rb2d.velocity = new Vector3(-speed, rb2d.velocity.y, 0);
                else rb2d.velocity = new Vector3(speed, rb2d.velocity.y, 0);
            }
            //if player is inside of attack range
            else
            {

            }

        }
        //player outside of detection // patroling
        else
        {
            
        }

        if (rb2d.velocity.x > 0) back = false;
        else if (rb2d.velocity.x < 0) back = true;

    }
}
