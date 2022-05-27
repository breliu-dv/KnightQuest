using UnityEngine;

/// <summary>
/// This component is used to create a green slime patrol path, two points which enemies will move between.
/// </summary>
public partial class PatrolPath : MonoBehaviour
{
    /// <summary>
    /// One end of the patrol path.
    /// </summary>
    public Vector2 startPosition, endPosition;
    public GameObject blueSlime;
    public int slimeType = 0;
    /// <summary>
    /// Create a Mover instance which is used to move an entity along the path at a certain speed.
    /// </summary>
    /// <param name="speed"></param>
    /// <returns></returns>
    public Mover CreateMover(float speed = 1) => new Mover(this, speed);

    void Reset()
    {
        startPosition = Vector3.left;
        endPosition = Vector3.right;
    }

    void Update()
    {
        if(blueSlime != null)
        {
            gameObject.transform.position = blueSlime.transform.position;
        }
    }
}