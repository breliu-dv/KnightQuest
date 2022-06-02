using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicPlatforms : MonoBehaviour
{
    public Vector2 velocity;
    protected Vector2 targetVelocity;
    protected Rigidbody2D body;
    protected ContactFilter2D contactFilter;
    private float initialYStart;
    private float initialXStart;

    public void Teleport(Vector3 position)
    {
        body.position = position;
        velocity *= 0;
        body.velocity *= 0;
    }

    protected virtual void OnEnable()
    {
        body = GetComponent<Rigidbody2D>();
        body.isKinematic = true;
    }

    protected virtual void OnDisable()
    {
        body.isKinematic = false;
    }

    protected virtual void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
        initialXStart = gameObject.transform.position.x;
        initialYStart = gameObject.transform.position.y;
    }

    protected virtual void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    protected virtual void FixedUpdate()
    {
        velocity.x = targetVelocity.x;
        
        var deltaPosition = velocity * Time.deltaTime;
        var moveAlong = new Vector2(initialXStart, initialYStart);

        PerformMovement(moveAlong * deltaPosition.x);
        PerformMovement(Vector2.up * deltaPosition.y);
    }

    void PerformMovement(Vector2 move)
    {
        body.position = body.position + move.normalized * move.magnitude;
    }
}