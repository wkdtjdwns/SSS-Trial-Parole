using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OrangeGhost : MonoBehaviour
{
    [Header("컴포넌트")]
    private NavMeshAgent agent;
    private GhostController controller;

    [Header("플레이어")]
    private Transform playerTransform;

    [Header("시작 지점")]
    [SerializeField]
    private Transform startPoint;

    [Header("플레이어 예측 거리")]
    public float predictionRadius = 10f;

    [Header("플레이어와 추적 거리")]
    public float switchDistance = 15f;

    [Header("플레이어와 도망 전환 거리")]
    public float retreatDistance = 5f;

    [Header("Frightened 모드 설정")]
    [Tooltip("도망가는 거리")]
    [SerializeField]
    private float retreatOffset = 10f;

    [Tooltip("도망 방향 랜덤 오프셋 각도")]
    [SerializeField]
    private float randomAngleRange = 60f;

    [Tooltip("새 목표 갱신 주기 (초)")]
    [SerializeField]
    private float frightenedUpdateInterval = 5f;

    [Tooltip("플레이어와의 최소 거리")]
    [SerializeField]
    private float minDistanceFromPlayer = 10f;

    [Tooltip("랜덤 이동 반경")]
    [SerializeField]
    private float wanderRadius = 10f;

    // [ 내부 상수 ]
    private const float NavMeshSampleRange = 10f;
    private const float ArrivalThreshold = 0.1f;

    // [ 상태 관리 ]
    private float frightenedTimer = 0f;
    private Vector3 currentFrightenedTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<GhostController>();

        agent.autoBraking = false;
        agent.stoppingDistance = 0f;

        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player 오브젝트를 찾을 수 없음");
        }

        if (controller != null && controller.CurrentState == GhostState.MoveToStart && startPoint != null)
        {
            MoveToPosition(startPoint.position);
        }
    }

    void Update()
    {
        if (controller == null || playerTransform == null)
            return;

        switch (controller.CurrentState)
        {
            case GhostState.MoveToStart:
                HandleMoveToStart();
                break;

            case GhostState.Chase:
                HandleChase();
                break;

            case GhostState.Frightened:
                HandleFrightened();
                break;

            default:
                agent.isStopped = true;
                break;
        }
    }

    private void HandleMoveToStart()
    {
        if (!agent.pathPending && agent.remainingDistance <= ArrivalThreshold)
        {
            controller.SetState(GhostState.Chase);
        }
    }

    private void HandleChase()
    {
        agent.isStopped = false;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        Vector3 targetPosition;

        if (distanceToPlayer <= retreatDistance)
        {
            targetPosition = GetRetreatPosition();
        }
        else if (distanceToPlayer >= switchDistance)
        {
            targetPosition = playerTransform.position;
        }
        else
        {
            targetPosition = GetPredictedPosition();
        }

        TrySetDestination(targetPosition);
    }

    private void HandleFrightened()
    {
        agent.isStopped = false;

        frightenedTimer -= Time.deltaTime;

        if (frightenedTimer <= 0f || agent.remainingDistance <= ArrivalThreshold)
        {
            currentFrightenedTarget = GetSmartRetreatPosition();
            frightenedTimer = frightenedUpdateInterval;
            TrySetDestination(currentFrightenedTarget);
        }
    }

    private Vector3 GetRetreatPosition()
    {
        Vector3 dirAway = (transform.position - playerTransform.position).normalized;
        Vector3 rawTarget = transform.position + dirAway * retreatOffset;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(rawTarget, out hit, NavMeshSampleRange, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return transform.position;
        }
    }

    private Vector3 GetPredictedPosition()
    {
        Vector3 toPlayer = (playerTransform.position - transform.position).normalized;
        Vector3 rawTarget = transform.position + toPlayer * predictionRadius;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(rawTarget, out hit, NavMeshSampleRange, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return playerTransform.position;
        }
    }

    private Vector3 GetSmartRetreatPosition()
    {
        Vector3 dirAway = (transform.position - playerTransform.position).normalized;

        float randomAngle = Random.Range(-randomAngleRange / 2f, randomAngleRange / 2f);
        Quaternion rotation = Quaternion.Euler(0f, randomAngle, 0f);
        Vector3 rotatedDir = rotation * dirAway;

        Vector3 rawTarget = transform.position + rotatedDir * retreatOffset;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(rawTarget, out hit, NavMeshSampleRange, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return GetRandomNearbyPosition();
        }
    }

    private Vector3 GetRandomNearbyPosition()
    {
        const int maxAttempts = 10;
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
            Vector3 candidate = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, NavMeshSampleRange, NavMesh.AllAreas))
            {
                float distToPlayer = Vector3.Distance(hit.position, playerTransform.position);
                if (distToPlayer >= minDistanceFromPlayer)
                {
                    return hit.position;
                }
            }
        }

        Debug.LogWarning("OrangeGhost: 랜덤 위치 탐색 실패");
        return transform.position;
    }

    private void TrySetDestination(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();

        if (agent.CalculatePath(targetPosition, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(targetPosition);
        }
        else
        {
            Debug.LogWarning("OrangeGhost: 경로 실패, 플레이어 추적");
            agent.SetDestination(playerTransform.position);
        }
    }



    private void MoveToPosition(Vector3 position)
    {
        agent.SetDestination(position);
        agent.isStopped = false;
    }
}
