using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAnimatorController : KinematicObject
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    // For desired direction of travel.
    public Vector2 move;

    protected override void ComputeVelocity()
    {
        targetVelocity = move * maxSpeed;
    }
}