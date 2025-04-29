using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Transform playerCarTransform;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    [SerializeField] private float pointsPerMeter = 1f;

    private float startZ;
    private float score;
    private bool isCounting = true;
    private int bestScore = 0;

    private void Start()
    {
        if (playerCarTransform == null)
            playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;

        startZ = playerCarTransform.position.z;

        // Load best score
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        UpdateBestScoreText();
    }

    private void Update()
    {
        if (!isCounting)
            return;

        float distanceTravelled = playerCarTransform.position.z - startZ;
        if (distanceTravelled > 0)
        {
            score = distanceTravelled * pointsPerMeter;
            scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
        }
    }

    public void StopCounting()
    {
        isCounting = false;

        int finalScore = Mathf.FloorToInt(score);

        // Check if we have a new best score
        if (finalScore > bestScore)
        {
            bestScore = finalScore;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
        }
    }

    private void UpdateBestScoreText()
    {
        bestScoreText.text = "Best: " + bestScore.ToString();
    }
}
