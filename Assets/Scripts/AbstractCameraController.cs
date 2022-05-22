using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obscura
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(LineRenderer))]
    public abstract class AbstractCameraController : MonoBehaviour
    {
        [SerializeField]
        protected bool drawLogic;
        [SerializeField]
        protected GameObject target;

        public abstract void DrawCameraLogic();
    }

}