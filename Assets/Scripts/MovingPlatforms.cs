using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformAnimatorController), typeof(Collider2D))]

public class MovingPlatforms : MonoBehaviour
{
    public PatrolPath path;
    internal PatrolPath.Mover mover;
    internal PlatformAnimatorController control;
    public bool disableYMovement;

    void Awake()
    {
        control = GetComponent<PlatformAnimatorController>();
    }

    void Update()
    {
        if (path != null)
        {
            if (mover == null) 
            {
                mover = path.CreateMover(control.maxSpeed * 0.5f);
            }
            
            control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);

            if(!disableYMovement)
            {
                control.move.y = Mathf.Clamp(mover.Position.y - transform.position.y, -1, 1);
            }
        }
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class MovingPlatforms : MonoBehaviour
// {
//     [SerializeField] private AnimationCurve MotionCurve;
//     [SerializeField] private Vector3 Movement = new Vector3(-10f, 0f, 0f);
//     [SerializeField] private float Duration = 5.0f;
//     private Vector3 StartPosition;
//     private Vector3 EndPosition;
//     private float StartTime = 0f;

//     // Start is called before the first frame update
//     void Start()
//     {
//         this.StartPosition = this.gameObject.GetComponent<Transform>().position;
//         this.EndPosition = this.StartPosition + this.Movement;
//         this.StartTime = Time.time;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         //evaluates the y value of the animation curve given the input ranging from 0 to 1.
//         // var fractionOfJourney = this.MotionCurve.Evaluate((Time.time - this.StartTime) / this.Duration);
//         // this.transform.position = Vector3.Lerp(this.StartPosition, this.EndPosition, fractionOfJourney);
//         var distanceCovered = this.Movement * Time.deltaTime / this.Duration;
//         float fractionOfJourney = Vector3.Distance(this.StartPosition, this.transform.position + distanceCovered) / Vector3.Distance(this.StartPosition, this.EndPosition);
//         this.transform.position = Vector3.Lerp(this.StartPosition, this.EndPosition, 2);
//     }
// }
