using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public static GhostManager Instance;

    public GameObject ghostPrefab;
    public GameObject player;
    public float ghostTime = 8f;

    private List<Vector3> positions = new List<Vector3>();
    public bool IsGhostActive { get; private set; }
    private GameObject ghost;

    public GameObject ghostReplayPrefab;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !IsGhostActive)
        {
            StartGhost();
        }
    }

    void StartGhost()
    {
        positions.Clear();

        // 플레이어 위치 + 살짝 위 (0.1f)
        Vector3 spawnPos = player.transform.position + Vector3.up * 0.1f;

        ghost = Instantiate(ghostPrefab, spawnPos, Quaternion.identity);
        Debug.Log("Ghost 생성됨 at " + spawnPos);

        // 중력 설정 유지 (갑자기 튀는 거 방지용으로 약간 띄워서 생성함)
        Rigidbody ghostRb = ghost.GetComponent<Rigidbody>();
        if (ghostRb != null)
        {
            ghostRb.isKinematic = false;
            ghostRb.useGravity = true;
        }

        IsGhostActive = true;

        Invoke(nameof(EndGhost), ghostTime);
    }


    void EndGhost()
    {
        IsGhostActive = false;

        if (ghost != null)
        {
            Destroy(ghost);
            ghost = null;
            Debug.Log("조종 고스트 제거");
        }

        if (positions.Count == 0)
        {
            Debug.LogWarning("리플레이 생략");
            return;
        }

        GameObject replayGhost = Instantiate(ghostReplayPrefab);
        Debug.Log("리플레이 고스트 생성");

        // 리플레이용 고스트도 설정 동일하게
        Rigidbody rb = replayGhost.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }

        Collider col = replayGhost.GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = false; // 트리거 안 쓰는 구조라면 false
        }

        // 고스트 리플레이 컴포넌트 추가 및 경로 설정
        replayGhost.AddComponent<GhostReplayer2D>().Setup(positions);

        // 일정 시간 후 자동 파괴 (리플레이 잔상 유지기간)
        Destroy(replayGhost, ghostTime);
    }

    public void RecordPosition(Vector3 pos)
    {
        positions.Add(pos);
    }

    public void ResetAll()
    {
        // EndGhost() 실행 취소
        CancelInvoke(nameof(EndGhost));

        // 고스트 제거
        if (ghost != null)
        {
            Destroy(ghost);
            ghost = null;
        }

        // 행동 기록 초기화
        positions.Clear();

        // 상태 초기화
        IsGhostActive = false;
    }


}
