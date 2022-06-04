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

    void OnCollisionEnter2D(Collision2D otherCollide)
    {
        if(otherCollide.gameObject == knight)
        {
            Debug.Log(transform.position);
            Debug.Log(knight.transform.position);
            knight.transform.parent = transform;
        }
    }

    void OnCollisionExit2D(Collision2D otherCollide)
    {
        if(otherCollide.gameObject == knight)
        {
            Debug.Log("exit WITH MOVING PLAYFORM");
            knight.transform.parent = null;
        }
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
