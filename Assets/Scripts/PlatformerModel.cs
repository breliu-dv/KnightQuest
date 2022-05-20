using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerModel : MonoBehaviour
{
    /// <summary>
    /// A global jump modifier applied to all initial jump velocities.
    /// </summary>
    public float jumpModifier = 1.5f;

    /// <summary>
    /// A global jump modifier applied to slow down an active jump when 
    /// the user releases the jump input.
    /// </summary>
    public float jumpDeceleration = 0.5f;
}
