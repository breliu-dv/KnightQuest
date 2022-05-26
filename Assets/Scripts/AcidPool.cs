using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPool : MonoBehaviour
{
    private int damage = 10;
    private bool enteredAcid = false;
    private KnightController player;
    void OnCollisionEnter2D(Collision2D collision)
    {
        player = collision.gameObject.GetComponent<KnightController>();
        Debug.Log(player);
        if (player != null)
        {
            enteredAcid = true;
            player.DoDamage(damage);
        }
    }

    public bool GetAcidStatus()
    {
        return enteredAcid;
    }

    void Update()
    {
        if (enteredAcid)
        {
            player.GetComponent<Animator>().SetBool("Grounded", true);
        }
    }
}
