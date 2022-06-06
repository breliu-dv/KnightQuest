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
    public LayerMask slimeBlueLayer;
    public LayerMask slimeGreenLayer;
    public LayerMask slimeRedLayer;
    private float timePassedSinceStuck;
    private Vector3 objVelocity;
    private float tempTakeOffToUnstuck = 30.0f;
    private float secondTempTakeOffToUnstuck = 50.0f;
    private Vector3 prevPos; 
    private Vector3 newPos; 
    private float timeAfterJump = 0.0f;
    private float originalJumpSpeed;

    // For pausing when attacked.
    private float originalSpeed;
    public float speed;
    private float dazedTime;
    public float startDazeTime;
    public float maxHealth = 60.0f;
    private float currentHealth;
    private Vector2 spawnPosition;
    private SoundManager soundManager;
    void Start()
    {
        this.initialSlimePosition = gameObject.transform.position;
        this.jumpInterval = Random.Range(minJumpInterval, maxJumpInterval);
        this.originalSpeed = this.control.maxSpeed;
        this.spawnPosition = this.transform.position;
        this.currentHealth = this.maxHealth;

        this.prevPos = this.transform.position;
        this.newPos = this.transform.position;
        this.originalJumpSpeed = this.control.getJumpTakeOffSpeed();
        this.timePassedSinceStuck = 0;
    }

    void Awake()
    {
        control = GetComponent<AnimationController>();
        _collider = GetComponent<Collider2D>();
        _audio = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
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
            soundManager.NotifyExplosion();
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
            
            Vector2 leftPos = transform.position;
            Vector2 rightPos = transform.position;

            leftPos.x -= (0.73f/2);
            leftPos.y += 0.2f;

            rightPos.x += (0.73f/2);
            rightPos.y += 0.2f;

            float distance = 1.0f;
            RaycastHit2D hitLBlueSlimeShort = Physics2D.Raycast(leftPos, new Vector2(-1, 0), distance, slimeBlueLayer);
            RaycastHit2D hitRBlueSlimeShort = Physics2D.Raycast(rightPos, new Vector2(1, 0), distance, slimeBlueLayer);

            RaycastHit2D hitLGreenSlimeShort = Physics2D.Raycast(leftPos, new Vector2(-1, 0), distance, slimeGreenLayer);
            RaycastHit2D hitRGreenSlimeShort = Physics2D.Raycast(rightPos, new Vector2(1, 0), distance, slimeGreenLayer);

            RaycastHit2D hitLRedSlimeShort = Physics2D.Raycast(leftPos, new Vector2(-1, 0), distance, slimeRedLayer);
            RaycastHit2D hitRRedSlimeShort = Physics2D.Raycast(rightPos, new Vector2(1, 0), distance, slimeRedLayer);
            
            newPos = transform.position;
            objVelocity = (newPos - prevPos) / Time.fixedDeltaTime;
            prevPos = newPos;
            timeAfterJump += Time.deltaTime;

            float originalTempTakeOffToUnstuck = tempTakeOffToUnstuck;

            if (hitLBlueSlimeShort || hitRBlueSlimeShort || hitLGreenSlimeShort || hitRGreenSlimeShort || hitLRedSlimeShort || hitRRedSlimeShort)
            {
                float takeOffFactor = 4;

                timePassedSinceStuck += Time.deltaTime;

                if(objVelocity.x == 0 && timePassedSinceStuck > 0.20)
                {
                    tempTakeOffToUnstuck = secondTempTakeOffToUnstuck;
                    control.setJumpTakeOffSpeed(tempTakeOffToUnstuck / takeOffFactor);
                    control.jump = true;
                    timePassedSinceStuck = 0;
                }
                else if (objVelocity.x == 0 && timePassedSinceStuck > 0.1 && timePassedSinceStuck < 0.15)
                {
                    tempTakeOffToUnstuck = originalTempTakeOffToUnstuck;
                    control.setJumpTakeOffSpeed(tempTakeOffToUnstuck / takeOffFactor);
                    control.jump = true;
                }
            }
            else
            {
                control.setJumpTakeOffSpeed(originalJumpSpeed);
            }
        }
    }

    void SlimeDeath()
    {
        this.spriteRenderer.enabled = false;
        this._collider.enabled = false;
        this.GetComponent<Rigidbody2D>().simulated = false;
        GameObject.Find("EnemyManager").GetComponent<PublisherManager>().SubscribeToGroup(1, Respawn);
    }

    void Respawn() 
    {
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
            SlimeDeath();
        }

        // need animator here. (Its animators job).
        Debug.Log("damage Taken!");
    }
}
