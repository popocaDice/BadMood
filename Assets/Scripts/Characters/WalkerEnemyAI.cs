using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WalkerEnemyAI : MonoBehaviour
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
        if (enemy.dead) GameObject.Destroy(gameObject);
        rb2d.velocity = enemy.Move(x, y);
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
        Stop();
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
        WalkToPlayer();
    }

    void AttackState()
    {
        if (Vector2.Distance(transform.position, player.position) > attackRange)
        {
            SetPreviousState();
            return;
        }
        weapon.See(player);
        if (weapon.CanShoot()) weapon.Shoot();
        WalkToPlayer();
    }

    void Stop()
    {
        x = 0;
    }

    void WalkToPlayer()
    {
        if (player.position.x < transform.position.x) x = -1;
        else x = 1;
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