using UnityEngine;


public interface IKnightCommand
{
    void Execute(GameObject gameObject, float inputX, float m_speed);
}
