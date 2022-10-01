using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    private PhysicsInterface parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponentInParent<PhysicsInterface>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.gameObject.tag == "Projectile") return;
        if (gameObject.name == "CeilingCheck" && collision.gameObject.layer == 15) return;
		if (gameObject.name == "GroundCheck")
        {
            if (collision.gameObject.tag != "Floor") return;
            if (collision.gameObject.layer == 15 && parent.GetVertSpeed() > 0) return;
        }
        parent.Invoke(gameObject.name + "In");
    }

    private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Projectile") return;
        if (gameObject.name == "CeilingCheck" && collision.gameObject.layer == 15) return;
        if (gameObject.name == "GroundCheck" && collision.gameObject.tag != "Floor") return;
		parent.Invoke(gameObject.name + "Out");
    }
}
