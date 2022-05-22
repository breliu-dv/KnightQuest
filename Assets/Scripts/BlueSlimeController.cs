using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationController), typeof(Collider2D))]
public class BlueSlimeController : MonoBehaviour
{
    public PatrolPath path;
    public AudioClip ouch;
    internal PatrolPath.Mover mover;
    internal AnimationController control;
    internal Collider2D _collider;
    internal AudioSource _audio;
    SpriteRenderer spriteRenderer;
    public GameObject knight;
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
        var player = collision.gameObject.GetComponent<KnightController>();
        if (player != null)
        {
            // var ev = Schedule<PlayerEnemyCollision>();
            // ev.player = player;
            // ev.enemy = this;
            int damage = player.PlayerHurt(20);
            if(damage > 0)
            {
                damage = damage;
            }
            else
            {
                player.PlayerDeath();
            }
            
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
            path.transform.position = knight.transform.position;
            control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
        }
    }
}
