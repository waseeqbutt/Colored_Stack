using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vasik
{
    public class InputManager : MonoBehaviour
    {
        private GameplayManager gameplayManager;

        private float next = 0.0f;

        // Start is called before the first frame update
        void Start()
        {
            gameplayManager = GameplayManager.Instance;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    if (Time.time > next)
                    {
                        gameplayManager.OnTouch();

                        next = Time.time + 0.5f;
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (Time.time > next)
                {
                    gameplayManager.OnTouch();

                    next = Time.time + 0.5f;
                }
            }
        }
    }

}