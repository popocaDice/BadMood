using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemyController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private PhysicsInterface p;
    private SaveManager global;

    public Sprite deadSprite;

    public float moveRange;
    public float positionSensibility;
    public float cycleTime;
    public float cycleChance;
    public float maxMoveInclineRotation;

    private Vector2 direction;
    private Vector2 targetPosition;
    private float cycleTimer = 0;

    // Start is called before the first frame update
    void Awake()
    {
        targetPosition = transform.position;
        global = GameObject.FindGameObjectWithTag("Save").GetComponent<SaveManager>();
        p = GetComponent<PhysicsInterface>();
        rb2d = GetComponent<Rigidbody2D>();
    }

	private void Update()
	{
        transform.localRotation = Quaternion.Euler(Vector3.forward * Mathf.Clamp(-rb2d.velocity.x*2, -maxMoveInclineRotation, maxMoveInclineRotation)); 
	    if (p.Dead)
		{
            p.Body().SetParent(null);
            p.Body().gameObject.AddComponent<CapsuleCollider2D>().size = new Vector2(0.24f, 0.32f);
            p.Body().gameObject.AddComponent<Rigidbody2D>().angularVelocity = Random.Range(-50, 50);
            p.Body().gameObject.GetComponent<SpriteRenderer>().enabled = true;
            p.Body().tag = "Untagged";
            Destroy(gameObject);
		}
    }

	private void FixedUpdate()
    {
        if (global.pause)
        {
            rb2d.velocity = Vector2.zero;
            return;
        }

        if (Vector2.Distance((Vector2)transform.position, targetPosition) < positionSensibility)
        {
            direction = Vector2.zero;

            //Debug.Log("reached at " + Time.time);

            cycleTimer++;

            if (cycleTimer >= cycleTime)
            {
                if (Random.Range(0, 100) <= cycleChance)
                {
                    targetPosition = new Vector3(Random.Range(-moveRange, moveRange), Random.Range(-moveRange, moveRange));
                    RaycastHit2D r = Physics2D.Raycast(transform.position,
                        targetPosition.normalized, targetPosition.magnitude, (1 << 10));
                    if (r.collider != null)
                    {
                        //Debug.Log("ray hit at " + Time.time);
                        targetPosition = Vector2.ClampMagnitude(targetPosition, (targetPosition.magnitude * r.fraction) -1);
                    }
                    direction = targetPosition.normalized;
                    targetPosition = (Vector2)transform.position + targetPosition;
				}
				else
				{
                    direction = Vector2.zero;
				}
                cycleTimer = 0;
            }
        }
		else
        {
            if ((targetPosition - (Vector2)transform.position).normalized != direction.normalized)
            {
                //Debug.Log("overshot at " + Time.time);
                direction = Vector2.ClampMagnitude(targetPosition - (Vector2)transform.position, 0.5f);
            }
        }

        rb2d.velocity = p.Move(direction);
    }

	private void OnDrawGizmos()
	{
        /*Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetPosition, moveRange);
        Gizmos.DrawRay(transform.position, direction);
        */
    }

    private void BodyPart(Transform part)
    {
        //Debug.Log(part.gameObject.name);
        foreach (Transform p in part)
        {
            if (p.childCount > 0) BodyPart(p);
            if (p.gameObject.GetComponent<SpriteRenderer>() == null)
            {
                Destroy(part.gameObject);
                continue;
            }

            //Debug.Log("fuck");
            p.SetParent(null);
            p.gameObject.AddComponent<PolygonCollider2D>();
            p.gameObject.AddComponent<Rigidbody2D>().angularVelocity = Random.Range(-50, 50);
            p.tag = "Untagged";
        }
    }
}
