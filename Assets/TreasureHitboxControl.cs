using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureHitboxControl : MonoBehaviour
{
    private int healthValue = 100;
    private bool alreadyHealthBoosted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<KnightController>();

        if (player != null && !alreadyHealthBoosted && player.getPlayerHealth() != player.getPlayerMaxHealth())
        {
            alreadyHealthBoosted = true;
            player.SetPlayerHealth(healthValue);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
