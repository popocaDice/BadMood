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
    public float flinchTimer;

    private Transform player;
    private SaveManager global;

    private float x = 0, y = 0;
    private bool flinched = false;

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
            NextState = 3,
            PreviousState = 1
        };
        StateStructure flinch = new StateStructure
        {
            StateValue = 3,
            StateName = "Flinching",
            NextState = null,
            PreviousState = 2
        };
        states = new StateStructure[]{
            idle,
            chase,
            attack,
            flinch
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
        if (enemy.Dead) EnemyDeath();
        rb2d.velocity = enemy.Move(x, y);
        ExecuteState();
    }

    void ExecuteState()
    {
        // Executing state
        if (currentState.StateName == "Idle") IdleState();
        else if (currentState.StateName == "Chasing") ChaseState();
        else if (currentState.StateName == "Attacking") AttackState();
        else if (currentState.StateName == "Flinching") FlinchState();
    }

    void IdleState()
    {
        if (Vector2.Distance(transform.position, player.position) <= detectionRange || (enemy.GetHealth() <= 0.4 * enemy.GetMaxHealth() && flinched == false))
        {
            SetNextState();
            return;
        }
        Stop();
        weapon.Unsee();
    }

    void ChaseState()
    {
        if (Vector2.Distance(transform.position, player.position) <= attackRange || (enemy.GetHealth() <= 0.4 * enemy.GetMaxHealth() && flinched == false))
        {
            SetNextState();
            return;
        }
        if (Vector2.Distance(transform.position, player.position) > detectionRange)
        {
            SetPreviousState();
            return;
        }
        weapon.See(player);
        WalkToPlayer();
    }

    void AttackState()
    {
        if(enemy.GetHealth() <= 0.4 * enemy.GetMaxHealth() && flinched == false)
        {
            SetNextState();
            return;
        }
        if (Vector2.Distance(transform.position, player.position) > attackRange)
        {
            SetPreviousState();
            return;
        }
        weapon.See(player);
        if (weapon.CanShoot()) weapon.Shoot();
        WalkToPlayer();
    }

    void FlinchState()
    {
        if (!flinched) flinched = true;
        if(flinchTimer == 0)
        {
            SetPreviousState();
            return;
        }
        flinchTimer -= 1;
        Stop();
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

    private void EnemyDeath()
	{
        BodyPart(enemy.Body());
        Destroy(gameObject);
	}

    private void BodyPart(Transform part)
	{
        //Debug.Log(part.gameObject.name);
        foreach (Transform p in part)
        {
            if (p.childCount > 0)
            {
                BodyPart(p);
            }
            if (p.gameObject.GetComponent<SpriteRenderer>() == null)
            {
                //Destroy(part.gameObject);
                continue;
            }

            //Debug.Log("fuck");
            p.SetParent(null);
            p.gameObject.AddComponent<PolygonCollider2D>();
            p.gameObject.AddComponent<Rigidbody2D>().angularVelocity = Random.Range(-50, 50);
            p.tag = "Untagged";
            p.gameObject.layer = 19;
        }
	}
}