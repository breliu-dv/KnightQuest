using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationController), typeof(Collider2D))]
public class GreenSlimeController : MonoBehaviour
{
    public PatrolPath path;
    public AudioClip ouch;

    internal PatrolPath.Mover mover;
    internal AnimationController control;
    internal Collider2D _collider;
    internal AudioSource _audio;
    SpriteRenderer spriteRenderer;

    public Bounds Bounds => _collider.bounds;

    void Awake()
    {
        control = GetComponent<AnimationController>();
        _collider = GetComponent<Collider2D>();
        _audio = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit enemy");
    }

    void Update()
    {
        if (path != null)
        {
            if (mover == null) mover = path.CreateMover(control.maxSpeed * 0.5f);
            control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
        }
    }
}
