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
        timeBeforeJump += Time.deltaTime;
        
        if (path != null)
        {
            if (mover == null)
            {
                mover = path.CreateMover(control.maxSpeed * 0.5f);
            }

            knightToSlimeDist = Vector3.Distance(knight.transform.position, gameObject.transform.position);
            knightToSlimeInitDist = Vector3.Distance(knight.transform.position, initialSlimePosition);

            Debug.Log(knightToSlimeDist);
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
    }
}
