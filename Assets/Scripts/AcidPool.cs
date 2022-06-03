using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPool : MonoBehaviour
{
    private int damage = 10;
    private float damageInterval = 2.0f;
    private float nextDamage = 0.0f;
    private bool enteredAcid = false;
    [SerializeField]
    private GameObject player;
    private BoxCollider2D boxCollider;

    void Awake() 
    {
        this.boxCollider = this.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (this.boxCollider.IsTouching(player.GetComponent<CapsuleCollider2D>()))
        {
            Action<int> DamageKnight = player.GetComponent<KnightController>().DoDamage;
            
            // If first time entering acid.
            if (!this.enteredAcid)
            {
                this.enteredAcid = true;
                DamageKnight(damage);
                this.nextDamage = Time.time + this.damageInterval;
            }
            // If was already in acid and is time to do next damage.
            else if (this.nextDamage <= Time.time)
            {
                DamageKnight(damage);
                this.nextDamage = Time.time + this.damageInterval;
            }
        }
        // Not touching the player.
        else
        {
            this.enteredAcid = false;
        }
    }
}
