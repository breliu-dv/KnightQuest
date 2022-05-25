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

    private int damage = 20;

    public Bounds Bounds => _collider.bounds;

    public float health = 20f;


    // for pausing when attacked
    public float speed;
    private float dazedTime;
    public float startDazeTime;


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
            player.DoDamage(damage);
        }
    }

    void Update()
    {


        if(dazedTime <= 0)
        {
            control.maxSpeed = 2; // ig this is what makes it faster try to set it to normal speed
        }
        else
        {
            control.maxSpeed = 0;
            dazedTime -= Time.deltaTime;
        }

        if (path != null)
        {
            if (mover == null) 
            {
                mover = path.CreateMover(control.maxSpeed * 0.5f);
            }
            control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
        }

         Debug.Log("Health is "+ health);
        if(health<= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        dazedTime = startDazeTime;
        health -= damage;
        // need animator here. (Its animators job).
        Debug.Log("damage Taken!");

    }
}
