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
    public float detectionZone;
    public Bounds Bounds => _collider.bounds;
    private bool dontKeepJumpFlag = false;
    public float setTimeBetweenJump;
    public float jumpTriggerRange;
    public float followRange = 0.0f;
    public float minJumpInterval;
    public float maxJumpInterval;
    private float knightToSlimeDist;
    private float knightToSlimeInitDist;
    private Vector3 knightPostOutrunPosition = new Vector3(0, 0, 0);
    private int damage = 20;
    private Vector3 initialSlimePosition;
    private bool gotChasedAtLeastOnce;
    private float timeBeforeJump = 0.0f;
    private float jumpInterval = 0.0f;
    private Vector3 prevPos; 
    private Vector3 newPos; 
    private Vector3 objVelocity;
    private float timePassedSinceStuck;
    
    // For pausing when attacked.
    public float speed;
    private float dazedTime;
    public float startDazeTime;
    public float maxHealth = 60.0f;
    private float currentHealth;
    private float timeAfterJump = 0.0f;
    public LayerMask groundLayer;
    public LayerMask slimeBlueLayer;
    public LayerMask slimeGreenLayer;
    public LayerMask slimeRedLayer;
    private float originalSpeed;
    private float originalJumpSpeed;
    private Vector2 spawnPosition;

    void Start()
    {
        this.initialSlimePosition = this.gameObject.transform.position;
        this.jumpInterval = Random.Range(minJumpInterval, maxJumpInterval);
        this.prevPos = this.transform.position;
        this.newPos = this.transform.position;
        this.originalSpeed = this.control.maxSpeed;
        this.originalJumpSpeed = this.control.getJumpTakeOffSpeed();
        this.timePassedSinceStuck = 0;
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
            player.DoDamage(damage);
        }
    }

    void Update()
    {
        // This is for pausing mechanism when the enemy got attacked.
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

            if (knightToSlimeDist < followRange && knightToSlimeInitDist < followRange + detectionZone)
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

            if(timeBeforeJump > jumpInterval && knightToSlimeDist < jumpTriggerRange)
            {
                timeBeforeJump = 0;
                jumpInterval = Random.Range(minJumpInterval, maxJumpInterval);
                control.jump = true;
            }
            control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
        }

        float distance = 1.0f;
        float shortWallDistanceModifier = 50.0f;
        float tempTakeOffToUnstuck = 30.0f;
        float secondTempTakeOffToUnstuck = 50.0f;
        Vector2 leftPos = transform.position;
        Vector2 rightPos = transform.position;

        leftPos.x -= (0.73f/2);
        leftPos.y += 0.2f;

        rightPos.x += (0.73f/2);
        rightPos.y += 0.2f;

        Debug.DrawRay(leftPos, new Vector2(-1, 0), Color.green);
        // Debug.DrawRay(leftPos, new Vector2(-1, 4), Color.green);

        Debug.DrawRay(rightPos, new Vector2(1, 0), Color.green);
        // Debug.DrawRay(rightPos, new Vector2(1, 4), Color.green);

		RaycastHit2D hitLGround = Physics2D.Raycast(leftPos, Vector2.down, distance, groundLayer);
        RaycastHit2D hitRGround = Physics2D.Raycast(rightPos, Vector2.down, distance, groundLayer);
        
        // Jump over walls and obstacles.
        RaycastHit2D hitLWallShort = Physics2D.Raycast(leftPos, new Vector2(-1, 0), distance, groundLayer);
        RaycastHit2D hitRWallShort = Physics2D.Raycast(rightPos, new Vector2(1, 0), distance, groundLayer);
        
        RaycastHit2D hitLBlueSlimeShort = Physics2D.Raycast(leftPos, new Vector2(-1, 0), distance, slimeBlueLayer);
        RaycastHit2D hitRBlueSlimeShort = Physics2D.Raycast(rightPos, new Vector2(1, 0), distance, slimeBlueLayer);

        RaycastHit2D hitLGreenSlimeShort = Physics2D.Raycast(leftPos, new Vector2(-1, 0), distance, slimeGreenLayer);
        RaycastHit2D hitRGreenSlimeShort = Physics2D.Raycast(rightPos, new Vector2(1, 0), distance, slimeGreenLayer);
        
        RaycastHit2D hitLRedSlimeShort = Physics2D.Raycast(leftPos, new Vector2(-1, 0), distance, slimeRedLayer);
        RaycastHit2D hitRRedSlimeShort = Physics2D.Raycast(rightPos, new Vector2(1, 0), distance, slimeRedLayer);

        RaycastHit2D hitLWallSuperShort = Physics2D.Raycast(leftPos, new Vector2(-1, 0), distance/shortWallDistanceModifier, groundLayer);
        RaycastHit2D hitRWallSuperShort = Physics2D.Raycast(rightPos, new Vector2(1, 0), distance/shortWallDistanceModifier, groundLayer);

        newPos = transform.position;
        objVelocity = (newPos - prevPos) / Time.fixedDeltaTime;
        prevPos = newPos;
        timeAfterJump += Time.deltaTime;

        float originalTempTakeOffToUnstuck = tempTakeOffToUnstuck;

        if(hitLWallSuperShort || hitRWallSuperShort) 
        {
            timePassedSinceStuck += Time.deltaTime;

            if(objVelocity.x == 0 && timePassedSinceStuck > 0.20)
            {
                tempTakeOffToUnstuck = secondTempTakeOffToUnstuck;
                control.setJumpTakeOffSpeed(tempTakeOffToUnstuck);
                control.jump = true;
                timePassedSinceStuck = 0;
            }
            else if (objVelocity.x == 0 && timePassedSinceStuck > 0.1 && timePassedSinceStuck < 0.15)
            {
                tempTakeOffToUnstuck = originalTempTakeOffToUnstuck;
                control.setJumpTakeOffSpeed(tempTakeOffToUnstuck);
                control.jump = true;
            }
        }
        else if (hitLBlueSlimeShort || hitRBlueSlimeShort || hitLGreenSlimeShort || hitRGreenSlimeShort || hitLRedSlimeShort || hitRRedSlimeShort)
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

        if (objVelocity.y > 0.0f) // If jumped then don't keep jumping.
        {
            dontKeepJumpFlag = true;
        }

        if(objVelocity.x == 0.0f && (hitLWallShort || hitRWallShort) && !dontKeepJumpFlag)
        {
            control.jump = true;
        }

        if(timeAfterJump > setTimeBetweenJump) // If jump is finished, reset flag so it can jump again if needed.
        {
            timeAfterJump = 0;
            dontKeepJumpFlag = false;
        }

        if(objVelocity.y < 0.0f && !dontKeepJumpFlag && (hitLGround.collider != null || hitRGround.collider != null))
        {
            control.jump = true;
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
        dazedTime = startDazeTime;
        this.currentHealth -= damage;

        if(this.currentHealth <= 0)
        {
            SlimeDeath();
        }

        // need animator here. (Its animators job).
        Debug.Log("damage Taken!");
    }
}
