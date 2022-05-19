using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obscura
{
    public class PushBoxCamera : AbstractCameraController
    {
        [SerializeField] public Vector3 topLeft;
        [SerializeField] public Vector3 bottomRight;
        private Camera managedCamera;
        private LineRenderer cameraLineRenderer;

        private void Awake()
        {
            this.managedCamera = this.gameObject.GetComponent<Camera>();
            this.cameraLineRenderer = this.gameObject.GetComponent<LineRenderer>();
        }

        //Use the LateUpdate message to avoid setting the camera's position before
        //GameObject locations are finalized.
        void LateUpdate()
        {
            var targetPosition = this.target.transform.position;
            var cameraPosition = this.managedCamera.transform.position;

            Debug.Log(cameraPosition);

            if (targetPosition.y >= cameraPosition.y + topLeft.y)
            {
                cameraPosition = new Vector3(cameraPosition.x, targetPosition.y - topLeft.y, cameraPosition.z);
            }

            if (targetPosition.y <= cameraPosition.y + bottomRight.y)
            {
                cameraPosition = new Vector3(cameraPosition.x, targetPosition.y- bottomRight.y, cameraPosition.z);
            }

            if (targetPosition.x >= cameraPosition.x + bottomRight.x)
            {
                cameraPosition = new Vector3(targetPosition.x - bottomRight.x, cameraPosition.y, cameraPosition.z);
            }
            
            if (targetPosition.x <= cameraPosition.x + topLeft.x)
            {
                cameraPosition = new Vector3(targetPosition.x- topLeft.x, cameraPosition.y, cameraPosition.z);
            }

            this.managedCamera.transform.position = cameraPosition;

            if (this.drawLogic)
            {
                this.cameraLineRenderer.enabled = true;
                this.DrawCameraLogic();
            }
            else
            {
                this.cameraLineRenderer.enabled = false;
            }
        }

        public override void DrawCameraLogic()
        {
            this.cameraLineRenderer.positionCount = 5;
            this.cameraLineRenderer.useWorldSpace = false;
            this.cameraLineRenderer.SetPosition(0, topLeft);
            this.cameraLineRenderer.SetPosition(1, new Vector3(bottomRight.x, topLeft.y, topLeft.z));
            this.cameraLineRenderer.SetPosition(2, bottomRight);
            this.cameraLineRenderer.SetPosition(3, new Vector3(topLeft.x, bottomRight.y, bottomRight.z));
            this.cameraLineRenderer.SetPosition(4, topLeft);
        }
    }
}
