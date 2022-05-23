using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikeController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnParticleCollision(GameObject HurricaneInner)
    {
        Debug.Log("Particle hit!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
