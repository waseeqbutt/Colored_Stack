using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vasik
{
    public class MovingCube : MonoBehaviour
    {
        public Vector3 direction;
        public float speed = 2.0f;
        public GameObject highlighter;

        private Color cubeColor;
        private float mid = 0.0f;
        private float splitDirection = 0.0f;
        private float newSize = 0.0f;
        private float newZPosition = 0.0f;
        private float debrisSize = 0.0f;
        private float counter = 0.0f;

        private GameObject debris = null;
        private GameObject gameManagerObj = null;

        public void Launch(Vector3 moveDirection, GameObject gameManagerObj)
        {
            direction = moveDirection;
            this.gameManagerObj = gameManagerObj;
        }

        // Update is called once per frame
        void Update()
        {
            counter += speed;
            transform.Translate((Mathf.Sin(counter)) * direction * Time.deltaTime);
        }

        public void SetColor(Color color)
        {
            cubeColor = color;
            GetComponent<MeshRenderer>().material.color = cubeColor;
        }

        public void SetColor(Material mat)
        {
            cubeColor = mat.color;
            GetComponent<MeshRenderer>().material = mat;
        }

        public void Split(Transform lastCube, string direction)
        {
            if(direction == "x")
            {
                SplitX(lastCube);
            }
            else
            if (direction == "z")
            {
                SplitZ(lastCube);
            }
        }

        private void SplitZ(Transform lastCube)
        {
            mid = transform.position.z - lastCube.position.z;

            if(Mathf.Abs(mid) > 0.05f)
            {
                splitDirection = mid > 0.0f ? 0.5f : -0.5f;
                newSize = lastCube.localScale.z - Mathf.Abs(mid);
                newZPosition = lastCube.position.z + (mid / 2);

                Vector3 scale = transform.localScale;
                scale.z = newSize;
                transform.localScale = scale;

                Vector3 pos = transform.position;
                pos.z = newZPosition;
                transform.position = pos;

                debris = GameObject.CreatePrimitive(PrimitiveType.Cube);
                debrisSize = lastCube.localScale.z - newSize;
                debris.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, debrisSize);
                debris.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + splitDirection);
                debris.GetComponent<MeshRenderer>().material.color = cubeColor;
                debris.AddComponent<Rigidbody>().useGravity = true;
                debris.AddComponent<Debris>();
                gameManagerObj.SendMessage("OnNotAccuratePlacement");
            }
            else
            {
                AutoAdjustZ(lastCube);
                gameManagerObj.SendMessage("OnAccuratePlacement");
            }

            gameManagerObj = null;
        }

        private void SplitX(Transform lastCube)
        {
            mid = transform.position.x - lastCube.position.x;

            if (Mathf.Abs(mid) > 0.05f)
            {
                splitDirection = mid > 0.0f ? 0.5f : -0.5f;
                newSize = lastCube.localScale.x - Mathf.Abs(mid);
                newZPosition = lastCube.position.x + (mid / 2);

                Vector3 scale = transform.localScale;
                scale.x = newSize;
                transform.localScale = scale;

                Vector3 pos = transform.position;
                pos.x = newZPosition;
                transform.position = pos;

                debris = GameObject.CreatePrimitive(PrimitiveType.Cube);
                debrisSize = lastCube.localScale.x - newSize;
                debris.transform.localScale = new Vector3(debrisSize, transform.localScale.y, transform.localScale.z);
                debris.transform.position = new Vector3(transform.position.x + splitDirection, transform.position.y, transform.position.z);
                debris.GetComponent<MeshRenderer>().material.color = cubeColor;
                debris.AddComponent<Rigidbody>().useGravity = true;
                debris.AddComponent<Debris>();
                gameManagerObj.SendMessage("OnNotAccuratePlacement");
            }
            else
            {
                AutoAdjustX(lastCube);
                gameManagerObj.SendMessage("OnAccuratePlacement");
            }

            gameManagerObj = null;
        }

        private void AutoAdjustZ(Transform lastCube)
        {
            Vector3 pos = transform.position;
            pos.z = lastCube.transform.position.z;
            transform.position = pos;
            highlighter.GetComponent<Animator>().enabled = true;
        }

        private void AutoAdjustX(Transform lastCube)
        {
            Vector3 pos = transform.position;
            pos.x = lastCube.transform.position.x;
            transform.position = pos;
            highlighter.GetComponent<Animator>().enabled = true;
        }

        public void Stop()
        {
            speed = 0.0f;
            this.enabled = false;
        }
    }

}