using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.gameObject.GetComponent<KnightController>();

        Debug.Log("Collided with checkpoint");

        if (player != null)
        {
            var newSpawn = this.transform.position;
            newSpawn.y -= this.GetComponent<BoxCollider2D>().size.y/2;
            Debug.Log("updated spawn");
            player.SetSpawn(newSpawn);
        }
    }
}
