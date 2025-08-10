using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class ScoreManager : MonoBehaviour
{
    // 싱글톤
    public static ScoreManager Instance;

    [Header("점수")]
    public int currentScore = 0;   // 현재 점수
    public int scoreToClear = 10000; // 클리어 기준 점수

    [Header("UI")]
    public TextMeshProUGUI scoreText; // 점수 텍스트

    void Awake()
    {
        // 싱글톤 설정
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // 시작할 때 점수 표시 초기화
        UpdateScoreUI();
    }

    // 점수 추가
    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();

        // 기준 점수 이상이면 게임 클리어
        if (currentScore >= scoreToClear)
        {
            if (GameManager_Classic.Instance != null)
            {
                GameManager_Classic.Instance.GameClear();
            }
        }
    }

    // 점수 UI 갱신
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }
}