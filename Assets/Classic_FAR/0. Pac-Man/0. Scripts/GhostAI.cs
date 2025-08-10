using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum GhostType { Red, Pink, Blue, Orange }

[RequireComponent(typeof(NavMeshAgent), typeof(GhostController))]
public class GhostAI : MonoBehaviour
{
    [Header("유령 유형")]
    public GhostType type;

    [Header("참조")]
    public Transform startPoint;        // 시작 시 이동할 위치
    public Transform player;

    [Header("거리/예측 파라미터")]
    public float switchDistance = 15f;  // 가까우면 직추, 멀면 예측
    public float predictionRadius = 10f;
    public float retreatDistance = 5f;  // Orange: 너무 가까우면 도망

    [Header("Frightened(공포) 파라미터")]
    public float retreatOffset = 10f;
    public float randomAngleRange = 60f;
    public float frightenedUpdateInterval = 5f;
    public float minDistanceFromPlayer = 10f;
    public float wanderRadius = 10f;

    // 내부
    NavMeshAgent agent;
    GhostController controller;
    float frightenedTimer;
    const float NavSample = 10f;
    const float Arrived = 0.1f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<GhostController>();
        agent.autoBraking = false;
        agent.stoppingDistance = 0f;

        if (!player)
        {
            var p = GameObject.Find("Player");
            if (p) player = p.transform;
        }
    }

    void Start()
    {
        if (controller.CurrentState == GhostState.MoveToStart && startPoint)
            Go(startPoint.position);
    }

    void Update()
    {
        if (!player) return;

        switch (controller.CurrentState)
        {
            case GhostState.MoveToStart:
                if (!agent.pathPending && agent.remainingDistance <= Arrived)
                    controller.SetState(GhostState.Chase);
                break;

            case GhostState.Chase:
                Chase();
                break;

            case GhostState.Frightened:
                Frightened();
                break;
        }
    }

    // 행동 로직
    void Chase()
    {
        agent.isStopped = false;
        float d = Vector3.Distance(transform.position, player.position);
        Vector3 target = player.position;

        switch (type)
        {
            case GhostType.Red:
                target = player.position;
                break;
            case GhostType.Pink:
                target = (d > switchDistance) ? PredictPink() : player.position;
                break;
            case GhostType.Blue:
                target = (d > switchDistance) ? PredictTowardPlayer() : player.position;
                break;
            case GhostType.Orange:
                if (d <= retreatDistance)
                    target = RetreatStraight();
                else if (d >= switchDistance)
                    target = player.position;
                else
                    target = PredictTowardPlayer();
                break;
        }

        TryGo(target);
    }

    void Frightened()
    {
        agent.isStopped = false;
        frightenedTimer -= Time.deltaTime;

        if (frightenedTimer <= 0f || agent.remainingDistance <= Arrived)
        {
            frightenedTimer = frightenedUpdateInterval;
            TryGo(RetreatSmart());
        }
    }

    // 예측과 도망
    Vector3 PredictTowardPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        return SampleOr(player.position, transform.position + dir * predictionRadius);
    }

    Vector3 PredictPink()
    {
        Vector3 fwd = player.forward.normalized;
        Vector3 ghostFromPlayer = (transform.position - player.position).normalized;
        Vector3 dir = (fwd + ghostFromPlayer).normalized;
        return SampleOr(player.position, player.position + dir * predictionRadius);
    }

    Vector3 RetreatStraight()
    {
        Vector3 away = (transform.position - player.position).normalized;
        return SampleOr(transform.position, transform.position + away * retreatOffset);
    }

    Vector3 RetreatSmart()
    {
        Vector3 away = (transform.position - player.position).normalized;
        float ang = Random.Range(-randomAngleRange * 0.5f, randomAngleRange * 0.5f);
        Vector3 dir = Quaternion.Euler(0f, ang, 0f) * away;
        Vector3 raw = transform.position + dir * retreatOffset;

        if (NavMesh.SamplePosition(raw, out var hit, NavSample, NavMesh.AllAreas))
            return hit.position;

        // 랜덤한 짖ㅁ 탐색
        const int tries = 10;
        for (int i = 0; i < tries; i++)
        {
            Vector2 c = Random.insideUnitCircle * wanderRadius;
            Vector3 cand = transform.position + new Vector3(c.x, 0, c.y);
            if (NavMesh.SamplePosition(cand, out hit, NavSample, NavMesh.AllAreas))
                if (Vector3.Distance(hit.position, player.position) >= minDistanceFromPlayer)
                    return hit.position;
        }
        return transform.position;
    }

    Vector3 SampleOr(Vector3 fallback, Vector3 raw)
    {
        return NavMesh.SamplePosition(raw, out var hit, NavSample, NavMesh.AllAreas) ? hit.position : fallback;
    }

    // 이동
    void TryGo(Vector3 dst)
    {
        var path = new NavMeshPath();
        if (agent.CalculatePath(dst, path) && path.status == NavMeshPathStatus.PathComplete)
            agent.SetDestination(dst);
        else
            agent.SetDestination(player.position); // 실패 시 플레이어 추적
    }

    void Go(Vector3 pos)
    {
        agent.isStopped = false;
        agent.SetDestination(pos);
    }
}
