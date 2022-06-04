using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacterRight : ScriptableObject, IKnightCommand
{
    public void Execute(GameObject gameObject, float inputX, float speed)
    {
        var m_body2d = gameObject.GetComponent<Rigidbody2D>();
        
        if (m_body2d != null)
        {
            m_body2d.velocity = new Vector2(speed, m_body2d.velocity.y);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}