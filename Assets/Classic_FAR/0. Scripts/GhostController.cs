using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostState
{
    MoveToStart,   // 시작할 때 지정 위치로 이동
    Chase,         // 플레이어 추적
    Frightened     // 도망(공포 상태)
}

public class GhostController : MonoBehaviour
{
    [Header("유령 상태")]
    public GhostState CurrentState;

    [Header("플레이어")]
    private Player_Classic player;

    [Header("스폰 위치")]
    public Transform spawnPoint;

    private Coroutine frightenedCoroutine;

    void Start()
    {
        player = FindObjectOfType<Player_Classic>();

        SetState(GhostState.MoveToStart);
    }

    void Update()
    {

    }

    public void SetState(GhostState newState)
    {
        CurrentState = newState;

        Debug.Log($"{gameObject.name} 상태 전환: {newState}");

        // 공포 상태에 진입하면 타이머 시작
        if (newState == GhostState.Frightened)
        {
            // 이미 코루틴이 돌고 있으면 중단
            if (frightenedCoroutine != null)
            {
                StopCoroutine(frightenedCoroutine);
            }
            frightenedCoroutine = StartCoroutine(FrightenedDuration());
        }
    }

    IEnumerator FrightenedDuration()
    {
        yield return new WaitForSeconds(10f);

        // 10초 후 상태 복귀
        SetState(GhostState.Chase);

        frightenedCoroutine = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (CurrentState == GhostState.Chase)
            {
                if (player != null)
                    player.SpawnPoint();
            }
            else if (CurrentState == GhostState.Frightened)
            {
                SpawnPoint();
            }
        }
    }

    public void SpawnPoint()
    {
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
