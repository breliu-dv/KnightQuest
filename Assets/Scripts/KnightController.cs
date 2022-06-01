using UnityEngine;
using System.Collections;

public class KnightController : MonoBehaviour {

    private IKnightCommand right;
    private IKnightCommand left;
    private IKnightCommand roll;
    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] bool       m_noBlood = false;
    [SerializeField] GameObject m_slideDust;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    // private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    // private bool                m_grounded = false;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;
    private float maxHealth = 100f;
    private float currentHealth = 0.0f;
    private float timeAfterDamage = 0.0f;

    public HealthBar healthBar;


    // variables to attack enemies.
    public float attackRange;
    public Transform attackPos;
    public LayerMask blueEnemy;
    public LayerMask greenEnemy;
    public LayerMask redEnemy;

    public float damage;
    public LayerMask groundLayer;
    private Collision2D knightCollideObject;



    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        // m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        this.right = ScriptableObject.CreateInstance<MoveCharacterRight>();
        this.left = ScriptableObject.CreateInstance<MoveCharacterLeft>();
        this.roll = ScriptableObject.CreateInstance<CharacterRoll>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update ()
    {
        timeAfterDamage += Time.deltaTime;
        // print(IsGrounded());
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if(m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        // //Check if character just landed on the ground
        // if (!m_grounded && m_groundSensor.State())
        // {
        //     m_grounded = true;
        //     m_animator.SetBool("Grounded", m_grounded);
        // }

        // //Check if character just started falling
        // if (m_grounded && !m_groundSensor.State())
        // {
        //     m_grounded = false;
        //     m_animator.SetBool("Grounded", m_grounded);
        // }

        m_animator.SetBool("Grounded", IsGrounded());

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            this.right.Execute(this.gameObject, inputX, m_speed);
            m_facingDirection = 1;
        }
            
        else if (inputX < 0)
        {
            this.left.Execute(this.gameObject, inputX, m_speed);
            m_facingDirection = -1;
        }

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        //Death
        if (Input.GetKeyDown("e") && !m_rolling)
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }
            
        //Hurt
        else if (Input.GetKeyDown("q") && !m_rolling)
            m_animator.SetTrigger("Hurt");

        //Attack
        else if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
        {
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // maybe add attack pts here?
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position,attackRange,blueEnemy);

            for(int i = 0; i< enemiesToDamage.Length;i++)
            {
                enemiesToDamage[i].GetComponent<BlueSlimeController>().TakeDamage(damage);
            }

            // for green enemies attack
            Collider2D[] enemiesToAttack = Physics2D.OverlapCircleAll(attackPos.position,attackRange,greenEnemy);

            for(int i = 0; i< enemiesToAttack.Length;i++)
            {
                enemiesToAttack[i].GetComponent<GreenSlimeController>().TakeDamage(damage);
            }

            Collider2D[] redEnemiesToAttack = Physics2D.OverlapCircleAll(attackPos.position,attackRange,redEnemy);

            for(int i = 0; i< redEnemiesToAttack.Length;i++)
            {
                redEnemiesToAttack[i].GetComponent<RedSlimeController>().TakeDamage(damage);
            }

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }

        // Block
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool("IdleBlock", false);

        // Roll
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            this.roll.Execute(this.gameObject, this.m_facingDirection, this.m_rollForce);
        }
            

        //Jump
        else if (Input.GetKeyDown("space") && IsGrounded() && !m_rolling)
        {
            m_animator.SetTrigger("Jump");
            // m_grounded = false;
            m_animator.SetBool("Grounded", false);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            //m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }

        //transform.Translate(Vector2.left*speed*Time.deltaTime);
    }

    void onDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position,attackRange);
    }

    public void DoDamage(int damage) 
    {
        this.currentHealth = Mathf.Max(0, currentHealth - damage);
        m_animator.SetTrigger("Hurt");

        if (this.currentHealth <= 0f) 
        {
            this.PlayerDeath();
            this.maxHealth = 100f;
        }
        healthBar.SetHealth(currentHealth);
    }

    public void PlayerDeath()
    {
        m_animator.SetBool("noBlood", m_noBlood);
        m_animator.SetTrigger("Death");
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        knightCollideObject = collision;
    }

    bool IsGrounded() 
    {
        //Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.0f;
        Vector2 leftPos = transform.position;
        Vector2 rightPos = transform.position;
        leftPos.x -= (0.73f/2);
        leftPos.y += 0.9f;

        rightPos.x += (0.73f/2);    //0.73f is from 2D Box Collider x size
        rightPos.y += 0.9f;

        
        Debug.DrawRay(leftPos, direction, Color.green);
        Debug.DrawRay(rightPos, direction, Color.green);

		RaycastHit2D hitLGround = Physics2D.Raycast(leftPos, direction, distance, groundLayer);
        RaycastHit2D hitRGround = Physics2D.Raycast(rightPos, direction, distance, groundLayer);


        //if left end or right end of collision box as touching ground, is grounded.
        if (hitLGround.collider != null || hitRGround.collider != null) 
        {
            return true;
        }
        return false;
    }

    //  void OnCollisionEnter2D(Collision2D  collision) 
    //  {
    //      Collider2D collider = collision.collider;
  
    //      if(collider.name == "Terrain")
    //      { 
    //          Vector3 contactPoint = collision.contacts[0].point;
    //          Vector3 center = collider.bounds.center;
 
    //          bool right = contactPoint.x > center.x;
    //          bool top = contactPoint.y > center.y;

    //          Debug.Log(right + " right, top " + top);
    //      }
    //  }
}
