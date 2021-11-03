using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace vasik
{
    public class CameraBehavior : MonoBehaviour
    {
        public Transform lookTarget;
        public float lookOffset = 6.0f;
        public float speed = 2f;

        private float followOnCount = 0;
        private Camera mainCamera;
        private float startCameraZoom = 0.0f;
        private Vector3 zoomOutPosition;
        private bool isZoomOut = false;
        private float zoomOutAmount = 0.0f;

        private void Start()
        {
            mainCamera = GetComponent<Camera>();
            startCameraZoom = mainCamera.orthographicSize;
        }

        void LateUpdate()
        {
            if(isZoomOut == false)
            {
                if (followOnCount > 3)
                {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, lookTarget.position.y + lookOffset, transform.position.z)
                        , speed * Time.deltaTime);
                }
            }
            else
            {
                transform.LookAt(zoomOutPosition);
            }
        }

        public void SetTarget(Transform target)
        {
            lookTarget = target;
            followOnCount++;
        }

        public void ZoomOut(Vector3 startPos, Vector3 lastPos)
        {
            isZoomOut = true;

            zoomOutPosition = startPos;
            Vector3 pos = zoomOutPosition;
            pos.y = startPos.y + (lastPos.y - startPos.y) / 2;
            zoomOutPosition = pos;

            zoomOutAmount += (lastPos.y - startPos.y) / 3.0f;
            zoomOutAmount += startCameraZoom;
       
            DOTween.To(() => mainCamera.orthographicSize, x => mainCamera.orthographicSize = x, zoomOutAmount, 1f);
        }
    }

}