using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerModel
{
    // A global jump modifier applied to all initial jump velocities.
    public float jumpModifier = 1.5f;

    // A global jump modifier to slow down active jump when release jump input.
    public float jumpDeceleration = 0.5f;
}
