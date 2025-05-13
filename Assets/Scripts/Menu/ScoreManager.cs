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
    private bool isCounting = false; // Changed default to false
    private int bestScore = 0;

    private void Start()
    {
        // Load best score only; do not try to find player car yet
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        UpdateBestScoreText();
    }

    private void Update()
    {
        // Don’t run if score tracking is off or player car not set
        if (!isCounting || playerCarTransform == null)
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

        // Save best score if new high score achieved
        if (finalScore > bestScore)
        {
            bestScore = finalScore;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
        }
    }

    public void SetPlayer(Transform newPlayerTransform)
    {
        playerCarTransform = newPlayerTransform;
        startZ = playerCarTransform.position.z;
        isCounting = true;
    }

    private void UpdateBestScoreText()
    {
        bestScoreText.text = "Best: " + bestScore.ToString();
    }
}
