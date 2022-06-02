using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    public PatrolPath path;
    internal PatrolPath.Mover mover;
    internal AnimationController control;

    void Awake()
    {
        control = GetComponent<AnimationController>();
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
        }
    }
}
