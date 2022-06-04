using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformAnimatorController), typeof(Collider2D))]
public class MovingPlatforms : MonoBehaviour
{
    public PatrolPath path;
    internal PatrolPath.Mover mover;
    internal PlatformAnimatorController control;
    public bool disableYMovement;
    public GameObject knight;
 
    void Awake()
    {
        control = GetComponent<PlatformAnimatorController>();
    }

    void OnCollisionEnter2D(Collision2D collides)
    {
        Debug.Log(collides.gameObject.transform);
        collides.gameObject.transform.SetParent(gameObject.transform, true);
    }

    void OnCollisionExit2D(Collision2D collides)
    {
        Debug.Log("exit WITH MOVING PLAYFORM");
        collides.gameObject.transform.parent = null;
    }

    void Update()
    {
        if (path != null)
        {
            if (mover == null) 
            {
                mover = path.CreateMover(control.maxSpeed * 0.5f);
            }
            
            control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);

            if(!disableYMovement)
            {
                control.move.y = Mathf.Clamp(mover.Position.y - transform.position.y, -1, 1);
            }
        }
    }
}
