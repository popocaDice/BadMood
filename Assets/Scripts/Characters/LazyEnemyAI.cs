using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LazyEnemyAI : MonoBehaviour
{
    StateStructure[] states;
    private StateStructure currentState;
    public EnemyAimScript weapon;
    private Rigidbody2D rb2d;
    private PhysicsInterface enemy;
    public float detectionRange;
    public float attackRange;

    private Transform player;
    private SaveManager global;

    private float x = 0, y = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Setting basic states
        StateStructure idle = new StateStructure
        {
            StateValue = 0,
            StateName = "Idle",
            NextState = 1,
            PreviousState = null
        };
        StateStructure chase = new StateStructure
        {
            StateValue = 1,
            StateName = "Chasing",
            NextState = 2,
            PreviousState = 0
        };
        StateStructure attack = new StateStructure
        {
            StateValue = 2,
            StateName = "Attacking",
            NextState = null,
            PreviousState = 1
        };
        states = new StateStructure[]{
            idle,
            chase,
            attack
        };
        currentState = states.FirstOrDefault();

        // Getting other game components
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
        rb2d.velocity = enemy.Move(x, y);
        if (enemy.dead) GameObject.Destroy(gameObject);
        ExecuteState();
    }

    void ExecuteState()
    {
        // Executing state
        if (currentState.StateName == "Idle") IdleState();
        else if (currentState.StateName == "Chasing") ChaseState();
        else if (currentState.StateName == "Attacking") AttackState();
    }

    void IdleState()
    {
        if (Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            SetNextState();
            return;
        }
        x = 0;
        weapon.Unsee();
    }

    void ChaseState()
    {
        if (Vector2.Distance(transform.position, player.position) > detectionRange)
        {
            SetPreviousState();
            return;
        }
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            SetNextState();
            return;
        }
        weapon.See(player);
        if (player.position.x < transform.position.x) x = -1;
        else x = 1;
    }

    void AttackState()
    {
        if (Vector2.Distance(transform.position, player.position) > attackRange)
        {
            SetPreviousState();
            return;
        }
        weapon.See(player);
        x = 0;
        if(weapon.CanShoot()) weapon.Shoot();
    }

    void SetNextState()
    {
        if (GetState() == null) return;
        currentState = states[(int)currentState.NextState];
    }

    void SetPreviousState()
    {
        if (GetState() == null) return;
        currentState = states[(int)currentState.PreviousState];
    }

    public string GetState()
    {
        return currentState.StateName;
    }
}