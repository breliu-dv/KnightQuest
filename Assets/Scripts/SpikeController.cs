using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<KnightController>();
        if (player != null)
        {
            int damage = player.PlayerHurt(100);
            if(damage > 0)
            {
                damage = damage;
            }
            else
            {
                player.PlayerDeath();
            }
            
        }
    }
}
