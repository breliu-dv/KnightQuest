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
    private int damage = 25;
    private Vector3 initialSlimePosition;
    private bool gotChasedAtLeastOnce;
    private float timeBeforeJump = 0.0f;
    private float jumpInterval = 0.0f;
    private Vector3 PrevPos; 
    private Vector3 NewPos; 
    private Vector3 ObjVelocity;

    // for pausing when attacked
    public float speed;
    private float dazedTime;
    public float startDazeTime;
    public float health = 60.0f;
    private float timeAfterJump = 0.0f;
    public LayerMask groundLayer;
    private float originalSpeed;
    private float originalJumpSpeed;
    void Start()
    {
        initialSlimePosition = gameObject.transform.position;
        jumpInterval = Random.Range(minJumpInterval, maxJumpInterval);
        PrevPos = transform.position;
        NewPos = transform.position;
        originalSpeed = control.maxSpeed;
        originalJumpSpeed = control.getJumpTakeOffSpeed();
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
        // this is for pausing mechanism when the enemy got attacked 
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

            // Debug.Log(knightToSlimeDist);
            // Debug.Log(knightPostOutrunPosition.magnitude);

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

        //Debug.Log("Health is "+ health);
        if(health <= 0)
        {
            Destroy(gameObject);
        }

        float distance = 1.0f;
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

        RaycastHit2D hitLWallSuperShort = Physics2D.Raycast(leftPos, new Vector2(-1, 0), distance/20, groundLayer);
        RaycastHit2D hitRWallSuperShort = Physics2D.Raycast(rightPos, new Vector2(1, 0), distance/20, groundLayer);

        NewPos = transform.position;
        ObjVelocity = (NewPos - PrevPos) / Time.fixedDeltaTime;
        PrevPos = NewPos;
        timeAfterJump += Time.deltaTime;

        float tempTakeOffToUnstuck = 50.0f;
        float originalTempTakeOffToUnstuck = tempTakeOffToUnstuck;
        if(hitLWallSuperShort || hitRWallSuperShort) 
        {
            timeAfterJump+=2;
            if(ObjVelocity.x == 0)
            {
                control.setJumpTakeOffSpeed(tempTakeOffToUnstuck+=50);
            }
            else
            {
                tempTakeOffToUnstuck = originalTempTakeOffToUnstuck;
                control.setJumpTakeOffSpeed(tempTakeOffToUnstuck);
            }
            Debug.Log(tempTakeOffToUnstuck);
        }
        else
        {
            control.setJumpTakeOffSpeed(originalJumpSpeed);
        }

        if (ObjVelocity.y > 0.0f) // if jumped then don't keep jumping
        {
            dontKeepJumpFlag = true;
        }

        if(ObjVelocity.x == 0.0f && (hitLWallShort || hitRWallShort) && !dontKeepJumpFlag)
        {
            control.jump = true;
        }

        if(timeAfterJump > setTimeBetweenJump) // if jump is finished, reset flag so it can jump again if needed
        {
            timeAfterJump = 0;
            dontKeepJumpFlag = false;
        }

        if(ObjVelocity.y < 0.0f && !dontKeepJumpFlag && (hitLGround.collider != null || hitRGround.collider != null))
        {
            control.jump = true;
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
