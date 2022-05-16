using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obscura
{
    public class PositionLockCameraController : AbstractCameraController
    {
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

            cameraPosition = new Vector3(targetPosition.x, targetPosition.y, cameraPosition.z); //lock at center. 

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
            // Stage 1
            this.cameraLineRenderer.positionCount = 8;
            this.cameraLineRenderer.useWorldSpace = false;
            this.cameraLineRenderer.SetPosition(0, new Vector3(0,0,85));//center
            this.cameraLineRenderer.SetPosition(1, new Vector3(0,10,85)); // go up
            this.cameraLineRenderer.SetPosition(2, new Vector3(0,0,85)); // start at center
            this.cameraLineRenderer.SetPosition(3, new Vector3(10,0,85)); // go right
            this.cameraLineRenderer.SetPosition(4, new Vector3(0,0,85)); // start at center
            this.cameraLineRenderer.SetPosition(5, new Vector3(-10,0,85)); // go left
            this.cameraLineRenderer.SetPosition(6, new Vector3(0,0,85)); // start at center
            this.cameraLineRenderer.SetPosition(7, new Vector3(0,-10,85)); // go down
        }
    }
}
