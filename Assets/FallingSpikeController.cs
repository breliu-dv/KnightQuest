using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikeController : MonoBehaviour
{
    private int damage = 100;

    private void OnParticleCollision(GameObject collision)
    {
        var player = collision.gameObject.GetComponent<KnightController>();
        if (player != null)
        {
            player.DoDamage(damage);
        }
    }
}
