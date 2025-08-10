using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public static Door Instance; // 싱글톤 인스턴스

    private readonly List<SmallDot> smallDots = new List<SmallDot>(); // 현재 남아있는 작은 점 목록

    [Header("모든 점 수집 여부")]
    public bool allDotsCollected = false; // 모든 점을 수집했는지 여부

    private void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 작은 점 등록 (중복 방지)
    public void RegisterDot(SmallDot dot)
    {
        if (dot == null) return;
        if (!smallDots.Contains(dot)) smallDots.Add(dot);
    }

    // 작은 점 제거, 모든 점을 수집하면 상태 갱신
    public void UnregisterDot(SmallDot dot)
    {
        if (dot == null) return;

        if (smallDots.Contains(dot))
            smallDots.Remove(dot);

        if (smallDots.Count == 0)
            AllDotsCollected();
    }

    // 모든 점 수집 완료 처리
    private void AllDotsCollected()
    {
        allDotsCollected = true;
        Debug.Log("모든 작은 점을 수집했습니다.");
    }

    private void OnTriggerEnter(Collider other)
    {
        // 모든 점을 모으지 않았다면 문 작동 X
        if (!allDotsCollected) return;

        // 플레이어가 아니면 무시
        if (!other.CompareTag("Player")) return;

        Debug.Log("플레이어가 문에 도달");

        // GameManager로 클리어 처리 위임
        if (GameManager_Classic.Instance != null)
        {
            GameManager_Classic.Instance.GameClear();
        }
        else
        {
            // GameManager가 없을 경우 대비한 종료 처리
            Debug.LogWarning("GameManager 인스턴스를 찾지 못했습니다, 응급 종료 처리 수행.");
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}