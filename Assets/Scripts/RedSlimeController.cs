using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationController), typeof(Collider2D))]
public class RedSlimeController : MonoBehaviour
{
    public PatrolPath path;
    public AudioClip ouch;
    internal PatrolPath.Mover mover;
    internal AnimationController control;
    internal Collider2D _collider;
    internal AudioSource _audio;
    SpriteRenderer spriteRenderer;
    public GameObject knight;
    public float detectionZone;
    public Bounds Bounds => _collider.bounds;
    public float followRange = 0.0f;
    public float minJumpInterval;
    public float maxJumpInterval;
    public GameObject _psystem;
    private float knightToSlimeDist;
    private float knightToSlimeInitDist;
    private Vector3 knightPostOutrunPosition = new Vector3(0, 0, 0);
    private int damage = 25;
    private Vector3 initialSlimePosition;
    private bool gotChasedAtLeastOnce;
    private float timeBeforeJump = 0.0f;
    private float jumpInterval = 0.0f;

    // For pausing when attacked.
    private float originalSpeed;
    public float speed;
    private float dazedTime;
    public float startDazeTime;
    public float maxHealth = 80.0f;
    private float currentHealth;
    private Vector2 spawnPosition;
    void Start()
    {
        this.initialSlimePosition = gameObject.transform.position;
        this.jumpInterval = Random.Range(minJumpInterval, maxJumpInterval);
        this.originalSpeed = this.control.maxSpeed;
        this.spawnPosition = this.transform.position;
        this.currentHealth = this.maxHealth;
    }

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
            _psystem.transform.position = this.gameObject.transform.position;
            player.DoDamage(damage);
           _psystem.GetComponent<ParticleSystem>().Play();
            Debug.Log(_psystem.GetComponent<ParticleSystem>());
        }
    }

    void Update()
    {
        // Pausing mechanism when the enemy got attacked.
        if(dazedTime <= 0)
        {
            control.maxSpeed = originalSpeed;
        }
        else
        {
            control.maxSpeed = 0;
            dazedTime -= Time.deltaTime;
        }

        timeBeforeJump += Time.deltaTime;
        
        if (path != null)
        {
            if (mover == null)
            {
                mover = path.CreateMover(control.maxSpeed * 0.5f);
            }

            knightToSlimeDist = Vector3.Distance(knight.transform.position, gameObject.transform.position);
            knightToSlimeInitDist = Vector3.Distance(knight.transform.position, initialSlimePosition);

            if (knightToSlimeDist < followRange && knightToSlimeInitDist < detectionZone)
            {
                path.transform.position = knight.transform.position;
                gotChasedAtLeastOnce = true;
            }
            else if (knightToSlimeDist > followRange && knightToSlimeInitDist > detectionZone && gotChasedAtLeastOnce)
            {
                knightPostOutrunPosition = knight.transform.position;
            }

            if (knightPostOutrunPosition.magnitude > 0.0f && gotChasedAtLeastOnce)
            {
                gotChasedAtLeastOnce = false;
                path.transform.position = initialSlimePosition;
                knightPostOutrunPosition = new Vector3(0,0,0);
            }

            if(timeBeforeJump > jumpInterval)
            {
                timeBeforeJump = 0;
                jumpInterval = Random.Range(minJumpInterval, maxJumpInterval);
                control.jump = true;
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
            // Destroy(gameObject);
            SlimeDeath();
        }
        // need animator here. (Its animators job).
        Debug.Log("damage Taken!");
    }
}
