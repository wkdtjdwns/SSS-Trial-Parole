using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 총알의 주인 (플레이어 / 적)
public enum BulletOwner { Player, Enemy }

public class Bullet : MonoBehaviour
{
    [Header("기본 설정")]
    public BulletOwner owner = BulletOwner.Player; // 기본: 플레이어 탄
    public float speed = 10f;                      // 이동 속도
    public float lifeTime = 5f;                    // 생존 시간(초)

    void Start()
    {
        // 일정 시간 후 자동 파괴
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 이동 방향 설정
        Vector3 dir = Vector3.zero;
        if (owner == BulletOwner.Player)
        {
            dir = Vector3.up; // 플레이어 탄은 위로
        }
        else if (owner == BulletOwner.Enemy)
        {
            dir = Vector3.down; // 적 탄은 아래로
        }

        // 이동
        transform.Translate(dir * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (owner == BulletOwner.Player)
        {
            // 플레이어 탄 → 적에 맞으면
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
                Destroy(gameObject);
            }
        }
        else if (owner == BulletOwner.Enemy)
        {
            // 적 탄 → 플레이어에 맞으면
            if (other.CompareTag("Player"))
            {
                GameManager_Classic.Instance.GameOver();
                Destroy(gameObject);
            }
        }
    }
}
