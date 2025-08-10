using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("레벨 / 스탯")]
    [Range(1, 3)] public int level = 1;       // 1~3
    public float moveInterval = 0.2f;         // 이동 주기
    public float moveDistance = 0.3f;         // 이동 거리
    private int hp;                           // 체력

    [Header("처치 점수")]
    public int scoreOnDeath = 100;            // 처치 시 점수

    [Header("사격 설정 (레벨 3)")]
    public GameObject enemyBulletPrefab;      // 적 탄환 프리팹
    public Transform shootPoint;              // 발사 위치
    public float shootInterval = 1.2f;        // 사격 주기

    [Header("가속 설정 (레벨 2)")]
    public float fastLevelSpeedMultiplier = 1.8f; // 이동속도 배율

    private Transform player;                 // 플레이어 위치 저장

    void Start()
    {
        // 플레이어 찾기
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // 레벨별 스탯 적용
        if (level == 1)
        {
            hp = 3;
        }
        else if (level == 2)
        {
            hp = 2;
        }
        else if (level == 3)
        {
            hp = 1;
        }

        // 이동 시작
        StartCoroutine(MoveRoutine());

        // 레벨 3이면 사격 시작
        if (level == 3)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    // 이동 패턴
    IEnumerator MoveRoutine()
    {
        float interval = moveInterval;

        // 레벨 2는 속도 배율 적용
        if (level == 2)
        {
            interval = Mathf.Max(0.01f, moveInterval / fastLevelSpeedMultiplier);
        }

        while (true)
        {
            // 한 칸 아래로 이동
            transform.Translate(Vector3.down * moveDistance, Space.World);

            // 플레이어 아래로 내려가면 삭제
            if (player != null && transform.position.y < player.position.y - 1f)
            {
                Destroy(gameObject);
                yield break;
            }

            yield return new WaitForSeconds(interval);
        }
    }

    // 사격 루프 (레벨 3 전용)
    IEnumerator ShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootInterval);
            Shoot();
        }
    }

    // 총알 발사
    void Shoot()
    {
        if (enemyBulletPrefab == null) return;

        Vector3 pos = transform.position;
        if (shootPoint != null)
        {
            pos = shootPoint.position;
        }

        GameObject bullet = Instantiate(enemyBulletPrefab, pos, Quaternion.identity);

        Bullet b = bullet.GetComponent<Bullet>();
        if (b != null)
        {
            b.owner = BulletOwner.Enemy; // 적 탄으로 설정
        }
    }

    // 피격 처리
    public bool TakeDamage(int amount)
    {
        hp -= amount;

        if (hp <= 0)
        {
            ScoreManager.Instance.AddScore(scoreOnDeath);
            Destroy(gameObject);
            return true;
        }

        return false;
    }

    // 플레이어와 충돌 시 게임 오버
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager_Classic.Instance.GameOver();
        }
    }

    // 잘못된 값 방지
    void OnValidate()
    {
        if (level < 1) level = 1;
        if (level > 3) level = 3;

        if (moveInterval < 0.01f) moveInterval = 0.01f;
        if (moveDistance < 0f) moveDistance = 0f;
        if (shootInterval < 0.1f) shootInterval = 0.1f;
        if (fastLevelSpeedMultiplier < 1f) fastLevelSpeedMultiplier = 1f;
    }
}
