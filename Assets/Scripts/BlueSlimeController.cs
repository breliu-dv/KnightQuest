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
    public GameObject blueSlime;
    public float detectionZone;
    public Bounds Bounds => _collider.bounds;
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

    // for pausing when attacked
    public float speed;
    private float dazedTime;
    public float startDazeTime;
    public float health = 40.0f;
    public LayerMask groundLayer;

    void Start()
    {
        initialSlimePosition = gameObject.transform.position;
        jumpInterval = Random.Range(minJumpInterval, maxJumpInterval);

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
            control.maxSpeed = 5; // ig this is what makes it faster 
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
        if(health<= 0)
        {
            Destroy(gameObject);
        }

        float distance = 1.5f;
        Vector2 leftPos = transform.position;
        Vector2 rightPos = transform.position;
        leftPos.x -= (0.73f/2);
        leftPos.y += 0.9f;

        rightPos.x += (0.73f/2);
        rightPos.y += 0.9f;

        
        // Debug.DrawRay(leftPos, direction, Color.green);
        // Debug.DrawRay(rightPos, direction, Color.green);

		RaycastHit2D hitLGround = Physics2D.Raycast(leftPos, Vector2.down, distance, groundLayer);
        RaycastHit2D hitRGround = Physics2D.Raycast(rightPos, Vector2.down, distance, groundLayer);
        Debug.Log(hitLGround.collider);
        if((hitLGround.collider == null || hitRGround.collider == null) && timeBeforeJump > 1.3f)
        {
            control.jump = true;
            timeBeforeJump = 0;
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
