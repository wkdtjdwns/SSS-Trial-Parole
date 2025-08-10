using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("이동")]
    public float speed = 8f;                       // 공이 항상 유지할 속도
    public Vector3 initialDirection = new Vector3(1, 0.3f, 1); // 처음 날아가는 방향

    [Header("보정 옵션")]
    [Range(0f, 0.2f)] public float minNormalY = 0f; // 모서리에 걸려서 이상하게 튀는 거 방지
    public float minBounceAngleDeg = 5f;            // 거의 안 튀고 미끄러지는 거 방지
    [Range(0f, 1f)] public float normalBoost = 0.35f; // 튈 때 좀 더 자연스럽게 하기 위해 법선 방향으로 살짝 밀기
    public float separationDistance = 0.03f;        // 충돌한 면에서 살짝 떨어지게
    public float minIncidenceAngleDeg = 12f;        // 너무 얕게 부딪히면 방향 보정

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // 공이 떨어지지 않게 중력 꺼놓기
    }

    void Start()
    {
        // 시작 방향이 있으면 그쪽으로, 없으면 랜덤 방향으로 날리기
        Vector3 dir = (initialDirection.sqrMagnitude > 0.0001f)
            ? initialDirection.normalized
            : Random.onUnitSphere;

        rb.velocity = dir * speed;
    }

    void FixedUpdate()
    {
        // 속도가 줄거나 변해도 항상 같은 속도로 유지하기
        if (rb.velocity.sqrMagnitude > 0.0001f)
            rb.velocity = rb.velocity.normalized * speed;
        else
            rb.velocity += Random.onUnitSphere * 0.01f; // 혹시 멈추면 살짝 밀어서 안 멈추게
    }

    void OnCollisionEnter(Collision col)
    {
        // 바닥에 닿으면 게임오버
        if (col.gameObject.CompareTag("Floor"))
        {
            Destroy(gameObject);
            GameManager_Classic.Instance.GameOver();
            return;
        }

        if (col.contactCount == 0) return;

        // 부딪힌 면의 방향(법선) 가져오기
        Vector3 n = col.contacts[0].normal;

        // 너무 평평하면 살짝 위로 올려서 튀게
        if (minNormalY > 0f && n.y < minNormalY) n.y = minNormalY;
        n.Normalize();

        // 현재 공의 진행 방향
        Vector3 v = rb.velocity;
        if (v.sqrMagnitude < 0.0001f) v = transform.forward * speed;
        Vector3 vDir = v.normalized;

        // 부딪히는 각도 구하기
        float incidence = Vector3.Angle(-vDir, n); 
        if (incidence < minIncidenceAngleDeg)
        {
            // 너무 비스듬하게 닿으면 방향 보정
            Vector3 vN = Vector3.Project(vDir, n);     // 법선 방향 성분
            Vector3 vT = vDir - vN;                    // 옆으로 가는 성분
            vDir = (vT * 0.6f - vN).normalized;        // 튀는 쪽으로 살짝 변경
        }

        Vector3 reflected = Vector3.Reflect(vDir, n);
        reflected = (reflected + n * normalBoost).normalized;

        // 충돌한 면에서 조금 떨어뜨리기 / 끼이거나 멈추는 거 방지
        rb.position += n * separationDistance;

        // 너무 평평하게 튀면 법선 조금 섞어서 더 튀게 만들기
        float bounceAngle = Vector3.Angle(reflected, -n);
        if (bounceAngle < minBounceAngleDeg)
            reflected = (reflected + n * 0.1f).normalized;

        // 튄 뒤에도 속도는 그대로 유지
        rb.velocity = reflected * speed;
    }
}
