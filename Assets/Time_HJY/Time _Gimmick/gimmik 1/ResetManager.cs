using UnityEngine;
using System.Collections.Generic;

public class ResetManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetAll();
        }
    }

    void ResetAll()
    {
        if (GhostManager.Instance != null) 
        {
            GhostManager.Instance.ResetAll(); // 고스트 제거
        }

        GhostReplayer2D[] replayers = FindObjectsOfType<GhostReplayer2D>();
        foreach (var r in replayers)
        {
            Destroy(r.gameObject); // 리플레이 고스트 제거
        }

        ButtonInteraction2D[] buttons = FindObjectsOfType<ButtonInteraction2D>();
        foreach (var btn in buttons)
        {
            btn.ResetButton(); // 버튼 상태 초기화
        }

        ResettableObject[] resettableObjects = FindObjectsOfType<ResettableObject>();
        foreach (var obj in resettableObjects)
        {
            obj.ResetTransform(); // 위치 리셋
        }

        Debug.Log("전체 리셋");
    }
}