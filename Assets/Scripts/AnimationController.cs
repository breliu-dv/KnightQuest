using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AnimationController integrates physics and animation. 
// It is generally used for simple enemy animation.
[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class AnimationController : KinematicObject
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    // For desired direction of travel.
    public Vector2 move;

    // Set to true to initiate a jump.
    public bool jump;

    // Set to true to set the current jump velocity to zero.
    public bool stopJump;
    SpriteRenderer spriteRenderer;
    PlatformerModel model = Simulation.GetModel<PlatformerModel>();

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void setJumpTakeOffSpeed(float newSpeed)
    {
        this.jumpTakeOffSpeed = newSpeed;
    }

    public float getJumpTakeOffSpeed()
    {
        return this.jumpTakeOffSpeed;
    }

    protected override void ComputeVelocity()
    {
        if (jump && IsGrounded)
        {
            velocity.y = jumpTakeOffSpeed * model.jumpModifier;
            jump = false;
        }
        else if (stopJump)
        {
            stopJump = false;
            
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * model.jumpDeceleration;
            }
        }

        if (move.x > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (move.x < -0.01f)
        {
            spriteRenderer.flipX = true;
        }

        targetVelocity = move * maxSpeed;
    }
}