using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace vasik
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager Instance;

        public CameraBehavior cameraBehavior;
        public UIManager m_UIManager;
        public float cubeSpace = 0.1f;

        public Transform levelPoint;
        public GameObject cubePrefab;
        public Material[] materials;
        public Color[] colors;
        public Transform lastCube;
        public Transform parentCube;

        [Space]
        public AudioClip[] placeSounds;

        private MovingCube currentCube;
        private int spawnIndex = 0;
        private int colorIndex = -1;
        private bool isInitialized = false;
        private bool isGameOver = false;
        private string moveDirection = "z";
        private int score = 0;

        private AudioSource audioSource;

        void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            lastCube = parentCube;
            audioSource = GetComponent<AudioSource>();
        }

        public void OnTouch()
        {
            if (isGameOver == true)
                return;

            if (isInitialized == false)
            {
                CreateCube();
                isInitialized = true;
                score = -1;
                UpdateProgress();
            }
            else
            {
                if (IsInside() == true)
                {
                    StopMovingCube();
                    SplitCube();
                    StepUp();
                    CreateCube();
                    UpdateProgress();
                }
                else
                {
                    GameOver();
                }
            }
        }

        private void StepUp()
        {
            Vector3 pos = levelPoint.transform.position;
            pos.y += cubeSpace;
            levelPoint.transform.position = pos;
        }

        private void CreateCube()
        {
            if (spawnIndex == 0)
            {
                currentCube = Instantiate(cubePrefab).GetComponent<MovingCube>();
                currentCube.transform.position = new Vector3(lastCube.position.x, lastCube.position.y + cubeSpace
                    , lastCube.position.z + 1.5f);
                currentCube.transform.localScale = new Vector3(lastCube.transform.localScale.x, currentCube.transform.localScale.y
                    , lastCube.transform.localScale.z);
                currentCube.Launch(new Vector3(0f, 0f, -2.5f), gameObject);
                spawnIndex = 1;
                moveDirection = "z";
            }
            else
            if (spawnIndex == 1)
            {
                currentCube = Instantiate(cubePrefab).GetComponent<MovingCube>();
                currentCube.transform.position = new Vector3(lastCube.position.x + 1.5f, lastCube.position.y + cubeSpace
                    , lastCube.position.z);
                currentCube.transform.localScale = new Vector3(lastCube.transform.localScale.x, currentCube.transform.localScale.y
                    , lastCube.transform.localScale.z);
                currentCube.Launch(new Vector3(-2.5f, 0f, 0f), gameObject);
                spawnIndex = 0;
                moveDirection = "x";
            }

            if (colorIndex < colors.Length - 1)
                colorIndex++;
            else
                colorIndex = 0;

            currentCube.SetColor(materials[colorIndex]);
        }

        private bool IsInside()
        {
            switch (moveDirection)
            {
                case "z":
                    if (Mathf.Abs(lastCube.transform.position.z - currentCube.transform.position.z) >= lastCube.localScale.z)
                    {
                        return false;
                    }
                    break;
                case "x":
                    if (Mathf.Abs(lastCube.transform.position.x - currentCube.transform.position.x) >= lastCube.localScale.x)
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }

        private void StopMovingCube()
        {
            currentCube.Stop();
        }

        private void SplitCube()
        {
            currentCube.Split(lastCube, moveDirection);
            SetLastCube();
        }

        private void SetLastCube()
        {
            lastCube = currentCube.transform;
            cameraBehavior.SetTarget(lastCube);
        }

        private string GetDirection()
        {
            return moveDirection;
        }

        private void UpdateProgress()
        {
            score++;
            m_UIManager.UpdateScore(score);
            cameraBehavior.transform.DOShakePosition(0.15f, 0.05f, 1, 5);
        }

        private void ZoomOutCamera()
        {
            cameraBehavior.ZoomOut(parentCube.position, lastCube.position);
        }

        public void OnNotAccuratePlacement()
        {
            audioSource.PlayOneShot(placeSounds[0], 1.0f);
            audioSource.PlayOneShot(placeSounds[1], 1.0f);
        }

        public void OnAccuratePlacement()
        {
            m_UIManager.UpdateRemarks();
            audioSource.PlayOneShot(placeSounds[0], 1.0f);
        }

        public void Restart()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        private void GameOver()
        {
            currentCube.gameObject.AddComponent<Rigidbody>().useGravity = true;
            currentCube.enabled = false;
            ZoomOutCamera();
            isGameOver = true;
            m_UIManager.NewScore = score;
            m_UIManager.Invoke("OnGameOver", 3f);
        }
    }

}