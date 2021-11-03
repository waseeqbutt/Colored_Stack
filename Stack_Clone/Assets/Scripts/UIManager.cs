using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace vasik
{
    public class UIManager : MonoBehaviour
    {
        public Text scoreText;
        public Text remarkText;
        public Text newScoreText;
        public Text topScoreText;
        public GameObject gameOverPanel;

        private Color color;
        public int NewScore { get; set; }
        private string[] remarks = { "Spot On", "Nice", "Accurate", "Ethay Rakh", "Awesome" };

        public void UpdateScore(int score)
        {
            scoreText.text = score.ToString();
        }

        public void UpdateRemarks()
        {
            remarkText.text = remarks[Random.Range(0, remarks.Length)];
            SetTextColorValue(remarkText, 1f);
            remarkText.DOKill();
            remarkText.DOFade(0f, 2f);
        }

        public void OnGameOver()
        {
            gameOverPanel.SetActive(true);

            if (NewScore > PlayerPrefs.GetInt("HighScore"))
                PlayerPrefs.SetInt("HighScore", NewScore);

            scoreText.enabled = false;
            newScoreText.text = NewScore.ToString();
            topScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
        }

        private void SetTextColorValue(Text text, float alpha)
        {
            color = text.color;
            color.a = alpha;
            text.color = color;
        }
    }

}