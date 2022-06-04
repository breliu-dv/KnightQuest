using UnityEngine;
using System.Collections;

public class KnightController : MonoBehaviour 
{
    private IKnightCommand right;
    private IKnightCommand left;
    private IKnightCommand roll;
    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] bool       m_noBlood = false;
    [SerializeField] GameObject m_slideDust;
    [SerializeField] float      heavyChargeTime = 0.0f;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;
    private Vector2             spawnPosition;
    [SerializeField] float      respawnTime = 1.5f;
    private float               maxHealth = 100f;
    private float               currentHealth = 0.0f;
    private float               timeAfterDamage = 0.0f;
    private bool                canDoubleJump = true;
    private float               attackTimer = 0.0f;

    public HealthBar healthBar;

    // variables to attack enemies.
    public float attackRange;
    public Transform rightAttackPos;
    public Transform leftAttackPos;
    public LayerMask blueEnemy;
    public LayerMask greenEnemy;
    public LayerMask redEnemy;

    public float damage;
    public LayerMask groundLayer;
    private Collision2D knightCollideObject;


    // Use this for initialization
    void Start ()
    {
        this.m_animator = GetComponent<Animator>();
        this.m_body2d = GetComponent<Rigidbody2D>();
        this.m_wallSensorR1 = this.transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        this.m_wallSensorR2 = this.transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        this.m_wallSensorL1 = this.transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        this.m_wallSensorL2 = this.transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        this.right = ScriptableObject.CreateInstance<MoveCharacterRight>();
        this.left = ScriptableObject.CreateInstance<MoveCharacterLeft>();
        this.roll = ScriptableObject.CreateInstance<CharacterRoll>();
        this.currentHealth = this.maxHealth;
        this.healthBar.SetMaxHealth(maxHealth);
        this.spawnPosition = this.transform.position;
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
        {
            m_rollCurrentTime += Time.deltaTime;
        }

        // Disable rolling if timer extends duration
        if(m_rollCurrentTime > m_rollDuration)
        {
            m_rolling = false;
            m_rollCurrentTime = 0.0f;
        }

        // Re-enable Double Jump when on the ground
        if (IsGrounded())
        {
            canDoubleJump = true;
        }

        m_animator.SetBool("Grounded", IsGrounded());

        // -- Handle input and movement --
        if (this.currentHealth > 0f) 
        {
            float inputX = Input.GetAxis("Horizontal");
            if(inputX > 0 && m_rolling)
            {
                m_facingDirection = 1;
                float speed = (m_facingDirection * m_rollForce) + (inputX * m_speed);
                this.right.Execute(this.gameObject, inputX, speed);
            }
            else if (inputX < 0 && m_rolling)
            {
                m_facingDirection = -1;
                float speed = (m_facingDirection * m_rollForce) + (inputX * m_speed);
                this.left.Execute(this.gameObject, inputX, speed);
            }
            // Swap direction of sprite depending on walk direction
            else if (inputX > 0)
            {
                float speed = inputX * m_speed;
                this.right.Execute(this.gameObject, inputX, speed);
                m_facingDirection = 1;
            }
                
            else if (inputX < 0)
            {
                float speed = inputX * m_speed;
                this.left.Execute(this.gameObject, inputX, speed);
                m_facingDirection = -1;
            }
            

            //Set AirSpeed in animator
            m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

            // -- Handle Animations --
            //Wall Slide
            m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
            m_animator.SetBool("WallSlide", m_isWallSliding);

            // Increment Attack Charge Timer
            if (Input.GetMouseButton(0) && m_timeSinceAttack > 0.25f && !m_rolling)
            {
                attackTimer += Time.deltaTime;
            }

            //Attack
            else if(!Input.GetMouseButton(0) && m_timeSinceAttack > 0.25f && !m_rolling && attackTimer < heavyChargeTime && attackTimer > 0.0f && !m_isWallSliding)
            {
                Attack();                
                
            }

            // Heavy Attack
            else if (!Input.GetMouseButton(0) && m_timeSinceAttack > 0.25f && !m_rolling && attackTimer >= heavyChargeTime && !m_isWallSliding)
            {
                heavyAttack();
            }

            // Spin Attack
            else if (Input.GetKeyDown("e") && !m_rolling && !m_isWallSliding)
            {
                spinAttack();
            }

            // Block
            else if (Input.GetMouseButtonDown(1) && !m_rolling)
            {
                Block();
            }

            else if (Input.GetMouseButtonUp(1))
            {
                m_animator.SetBool("IdleBlock", false);
            }

            // Roll
            else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
            {
                Roll();
            }
                
            //Jump
            else if (Input.GetKeyDown("space") && IsGrounded() && !m_rolling)
            {
                Jump();
            }

            // Double Jump
            else if (Input.GetKeyDown("space") && !IsGrounded() && !m_rolling && canDoubleJump)
            {
                canDoubleJump = false;
                Jump();
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
                if (m_delayToIdle < 0)
                {
                    m_animator.SetInteger("AnimState", 0);
                }
            }
        }
    }

    public void SetPlayerHealth(float newHealthValue)
    {
        currentHealth = newHealthValue;
        healthBar.SetMaxHealth(newHealthValue);
    }

    public float getPlayerHealth()
    {
        return currentHealth;
    }

    public float getPlayerMaxHealth()
    {
        return maxHealth;
    }

    public void onDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rightAttackPos.position,attackRange);
    }

    public void DoDamage(int damage) 
    {
        // only update if still have health to remove
        if (currentHealth > 0) 
        {
            this.currentHealth = Mathf.Max(0, currentHealth - damage);
            this.m_animator.SetTrigger("Hurt");

            if (this.currentHealth <= 0f) 
            {
                this.PlayerDeath();
            }
            
            this.healthBar.SetHealth(currentHealth);
        }
    }

    public void PlayerDeath()
    {
        this.m_animator.SetBool("noBlood", m_noBlood);
        this.m_animator.SetTrigger("Death");
        
        StartCoroutine(DelayedRespawn());
    }

    IEnumerator DelayedRespawn() 
    {
        yield return new WaitForSeconds(respawnTime);
        this.transform.position = this.spawnPosition;
        this.currentHealth = this.maxHealth;
        this.healthBar.SetHealth(currentHealth);
        this.m_animator.SetTrigger("Respawn");
        GameObject.Find("EnemyManager").GetComponent<PublisherManager>().Trigger(1);
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 dustSpawnPosition;

        if (m_facingDirection == 1)
        {
            dustSpawnPosition = m_wallSensorR2.transform.position;
        }
        else
        {
            dustSpawnPosition = m_wallSensorL2.transform.position;
        }

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, dustSpawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        knightCollideObject = collision;
    }

    public void SetSpawn(Vector2 newSpawn)
    {
        this.spawnPosition = newSpawn;
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

    // Functions for certain movement that will be used in the mobile implementation
    void Attack()
    {
        Debug.Log("Normal Attack");
            attackTimer = 0.0f;
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
            {
                m_currentAttack = 1;
            }

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
            {
                m_currentAttack = 1;
            }

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // maybe add attack pts here?
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(rightAttackPos.position,attackRange,blueEnemy);

            for(int i = 0; i< enemiesToDamage.Length;i++)
            {
                enemiesToDamage[i].GetComponent<BlueSlimeController>().TakeDamage(damage);
            }

            // for green enemies attack
            Collider2D[] enemiesToAttack = Physics2D.OverlapCircleAll(rightAttackPos.position,attackRange,greenEnemy);

            for(int i = 0; i< enemiesToAttack.Length;i++)
            {
                enemiesToAttack[i].GetComponent<GreenSlimeController>().TakeDamage(damage);
            }

            Collider2D[] redEnemiesToAttack = Physics2D.OverlapCircleAll(rightAttackPos.position,attackRange,redEnemy);

            for(int i = 0; i< redEnemiesToAttack.Length;i++)
            {
                redEnemiesToAttack[i].GetComponent<RedSlimeController>().TakeDamage(damage);
            }

            // Reset timer
            m_timeSinceAttack = 0.0f;
    }

    void heavyAttack()
    {
        attackTimer = 0.0f;
            Debug.Log("Heavy Attack!");
            // maybe add attack pts here?
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(rightAttackPos.position,attackRange,blueEnemy);

            for(int i = 0; i< enemiesToDamage.Length;i++)
            {
                enemiesToDamage[i].GetComponent<BlueSlimeController>().TakeDamage(damage);
            }

            // for green enemies attack
            Collider2D[] enemiesToAttack = Physics2D.OverlapCircleAll(rightAttackPos.position,attackRange,greenEnemy);

            for(int i = 0; i< enemiesToAttack.Length;i++)
            {
                enemiesToAttack[i].GetComponent<GreenSlimeController>().TakeDamage(damage);
            }

            Collider2D[] redEnemiesToAttack = Physics2D.OverlapCircleAll(rightAttackPos.position,attackRange,redEnemy);

            for(int i = 0; i< redEnemiesToAttack.Length;i++)
            {
                redEnemiesToAttack[i].GetComponent<RedSlimeController>().TakeDamage(damage);
            }
            // Reset timer
            m_timeSinceAttack = 0.0f;
    }

    void spinAttack()
    {
        // Right Hit box
        Collider2D[] rightEnemiesToDamage = Physics2D.OverlapCircleAll(rightAttackPos.position,attackRange,blueEnemy);

        for(int i = 0; i< rightEnemiesToDamage.Length;i++)
        {
            rightEnemiesToDamage[i].GetComponent<BlueSlimeController>().TakeDamage(damage);
        }

        // for green enemies attack
        Collider2D[] rightEnemiesToAttack = Physics2D.OverlapCircleAll(rightAttackPos.position,attackRange,greenEnemy);

        for(int i = 0; i< rightEnemiesToAttack.Length;i++)
        {
            rightEnemiesToAttack[i].GetComponent<GreenSlimeController>().TakeDamage(damage);
        }

        Collider2D[] rightRedEnemiesToAttack = Physics2D.OverlapCircleAll(rightAttackPos.position,attackRange,redEnemy);

        for(int i = 0; i< rightRedEnemiesToAttack.Length;i++)
        {
            rightRedEnemiesToAttack[i].GetComponent<RedSlimeController>().TakeDamage(damage);
        }

        // Left Hit box
        Collider2D[] leftEnemiesToDamage = Physics2D.OverlapCircleAll(leftAttackPos.position,attackRange,blueEnemy);

        for(int i = 0; i< leftEnemiesToDamage.Length;i++)
        {
            leftEnemiesToDamage[i].GetComponent<BlueSlimeController>().TakeDamage(damage);
        }

        // for green enemies attack
        Collider2D[] leftEnemiesToAttack = Physics2D.OverlapCircleAll(leftAttackPos.position,attackRange,greenEnemy);

        for(int i = 0; i< leftEnemiesToAttack.Length;i++)
        {
            leftEnemiesToAttack[i].GetComponent<GreenSlimeController>().TakeDamage(damage);
        }

        Collider2D[] leftRedEnemiesToAttack = Physics2D.OverlapCircleAll(leftAttackPos.position,attackRange,redEnemy);

        for(int i = 0; i< leftRedEnemiesToAttack.Length;i++)
        {
            leftRedEnemiesToAttack[i].GetComponent<RedSlimeController>().TakeDamage(damage);
        }
    }

    void Roll()
    {
        m_rolling = true;
        m_animator.SetTrigger("Roll");
        this.roll.Execute(this.gameObject, this.m_facingDirection, m_rollForce);
    }

    void Jump()
    {
        m_animator.SetTrigger("Jump");
        m_animator.SetBool("Grounded", false);
        m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
    }

    void Block()
    {
        m_animator.SetTrigger("Block");
        m_animator.SetBool("IdleBlock", true);
    }
}
