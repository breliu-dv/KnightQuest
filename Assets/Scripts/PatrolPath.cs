using UnityEngine;

// This component is used to create a green slime patrol path, 
// two points which enemies will move between.
public partial class PatrolPath : MonoBehaviour
{
    // One end of the patrol path.
    public Vector2 startPosition, endPosition;
    public GameObject movingObj;
    public int slimeType = 0;

    // Create a Mover instance which is used to move an entity along the path at a certain speed.
    public Mover CreateMover(float speed = 1) => new Mover(this, speed);

    void Reset()
    {
        startPosition = Vector3.left;
        endPosition = Vector3.right;
    }

    void Awake()
    {
        Debug.Log(this.gameObject.transform.position == this.transform.parent.gameObject.transform.position);
    }

    void Update()
    {
        Debug.Log(this.gameObject.transform.position == this.transform.parent.gameObject.transform.position);
        if(movingObj != null)
        {
            gameObject.transform.position = movingObj.transform.position;
        }
    }
}