using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterRoll : ScriptableObject, IKnightCommand
{

    public void Execute(GameObject gameObject, float m_facingDirection, float m_rollForce)
    {
        var m_body2d = gameObject.GetComponent<Rigidbody2D>();
        m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
    }
} 