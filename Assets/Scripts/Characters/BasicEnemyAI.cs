using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : MonoBehaviour
{
    public EnemyAimScript weapon;
    private Rigidbody2D rb2d;
    private PhysicsInterface enemy;
    public float detectionRange;
    public float attackRange;

    private Transform player;
    private SaveManager global;

    private float x = 0, y = 0;

    // Start is called before the first frame update
    void Awake()
    {
        global = GameObject.FindGameObjectWithTag("Save").GetComponent<SaveManager>();
        rb2d = GetComponent<Rigidbody2D>();
        enemy = GetComponent<PhysicsInterface>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (global.pause)
        {
            rb2d.velocity = Vector2.zero;
            return;
        }
        //check if player is in detection range
        if (Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
			weapon.See(player);
            //check if player is out of attack range
            if (Vector2.Distance(transform.position, player.position) > attackRange)
            {
                if (player.position.x < transform.position.x) x = -1;
                else x = 1;
            }
            //if player is inside of attack range
            else
            {
                x = 0;
				weapon.Shoot();
            }

        }
        //player outside of detection // patroling
        else
        {
            x = 0;
			weapon.Unsee();
		}

        rb2d.velocity = enemy.Move(x, y);

        if (enemy.dead) GameObject.Destroy(gameObject);

    }

    void OnFire()
    {
        weapon.Shoot();
    }
}
