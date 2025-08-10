using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum GhostState
{
    MoveToStart,   // 시작 지점으로 이동
    Chase,         // 플레이어 추적
    Frightened     // 플레이어로부터 도망
}

public class GhostController : MonoBehaviour
{
    [Header("현재 유령 상태")]
    public GhostState CurrentState; // 현재 유령 상태

    [Header("플레이어 참조")]
    private Player_Classic player; // 플레이어 스크립트

    [Header("스폰 위치")]
    public Transform spawnPoint; // 유령이 부활할 위치

    private Coroutine frightenedCoroutine; // 공포 상태 지속 시간 코루틴

    private void Start()
    {
        // 플레이어 찾기
        player = FindObjectOfType<Player_Classic>();

        // 시작 시 MoveToStart 상태로 설정
        SetState(GhostState.MoveToStart);
    }

    public void SetState(GhostState newState)
    {
        CurrentState = newState;
        Debug.Log($"{gameObject.name} 상태 전환: {newState}");

        // 공포 상태 진입 시: 지금 가던 경로 즉시 끊기
        if (newState == GhostState.Frightened)
        {
            var agent = GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.isStopped = false;
                agent.ResetPath();
            }

            if (frightenedCoroutine != null)
                StopCoroutine(frightenedCoroutine);
            frightenedCoroutine = StartCoroutine(FrightenedDuration());
        }
        else
        {
            // 공포 상태 해제 시 코루틴 정리
            if (frightenedCoroutine != null)
            {
                StopCoroutine(frightenedCoroutine);
                frightenedCoroutine = null;
            }
        }
    }

    private IEnumerator FrightenedDuration()
    {
        // 10초간 공포 유지 후 추적으로 복귀
        yield return new WaitForSeconds(10f);
        SetState(GhostState.Chase);
        frightenedCoroutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Chase 상태면 플레이어 리스폰 처리
        if (CurrentState == GhostState.Chase)
        {
            if (player != null)
                player.SpawnPoint();
        }
        // Frightened 상태면 자신을 스폰 위치로 이동
        else if (CurrentState == GhostState.Frightened)
        {
            SpawnPoint();
        }
    }

    public void SpawnPoint()
    {
        // 스폰 위치로 순간 이동
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            Debug.Log($"{gameObject.name}: 스폰 위치로 순간 이동");
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: spawnPoint가 설정되어 있지 않습니다.");
        }
    }
}
