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
    public float maxHealth = 20f;
    private float currentHealth;

    // for pausing when attacked
    private float originalSpeed;
    private float dazedTime;
    public float startDazeTime;

    private Vector2 spawnPosition;

    void Start()
    {
        originalSpeed = control.maxSpeed;
    }

    void Awake()
    {
        control = GetComponent<AnimationController>();
        _collider = GetComponent<Collider2D>();
        _audio = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.spawnPosition = this.transform.position;
        this.currentHealth = this.maxHealth;
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
            control.maxSpeed = originalSpeed;
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
    }

    void SlimeDeath()
    {
        this.spriteRenderer.enabled = false;
        this._collider.enabled = false;
        this.GetComponent<Rigidbody2D>().simulated = false;
        GameObject.Find("EnemyManager").GetComponent<PublisherManager>().SubscribeToGroup(1, Respawn);
    }

    void Respawn() {
        this.transform.position = this.spawnPosition;
        this.currentHealth = this.maxHealth;
        this.spriteRenderer.enabled = true;
        this._collider.enabled = true;
        this.GetComponent<Rigidbody2D>().simulated = true;
    }

    public void TakeDamage(float damage)
    {
        this.dazedTime = this.startDazeTime;
        this.currentHealth -= damage;
        if(this.currentHealth <= 0)
        {
            //Destroy(gameObject);
            SlimeDeath();
        }
        // need animator here. (Its animators job).
        Debug.Log("damage Taken!");

    }
}
