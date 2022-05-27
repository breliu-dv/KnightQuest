using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

        Tilemap tilemap = GetComponent<Tilemap>();

        var originalPosition = gameObject.transform.position;
        var posInFronOfSlime = new Vector3( originalPosition.x + 10.0f, originalPosition.y, originalPosition.z);
        var tileInFrontOfSlime = getTile(tilemap, posInFronOfSlime);
        Debug.Log(tileInFrontOfSlime);
        
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
    }

    Tile getTile(Tilemap tileMap, Vector3 pos) 
    { 
        Vector3Int tilePos = tileMap.WorldToCell(pos);
        var tile = tileMap.GetTile<Tile>(tilePos);
        return tile;
    }

    public void TakeDamage(float damage)
    {
        dazedTime = startDazeTime;
        health -= damage;
        // need animator here. (Its animators job).
        Debug.Log("damage Taken!");
    }
}
