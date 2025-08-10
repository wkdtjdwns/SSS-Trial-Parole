using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BigDot : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 닿았을 때만 처리
        if (!other.CompareTag("Player")) return;

        Debug.Log("큰 점 수집 → 모든 유령 공포 상태 진입");

        // 씬에 있는 모든 유령을 찾아서 상태를 공포(Frightened)로 변경
        GhostController[] ghosts = FindObjectsOfType<GhostController>();
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].SetState(GhostState.Frightened);
        }

        // 큰 점 제거
        Destroy(gameObject);
    }
}
