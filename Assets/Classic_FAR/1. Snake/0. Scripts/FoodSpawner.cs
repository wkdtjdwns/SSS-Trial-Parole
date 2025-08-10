using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public static FoodSpawner Instance; // 싱글톤 인스턴스

    public GameObject foodPrefab;   // 소환할 먹이 프리팹
    public BoxCollider spawnArea;   // 스폰 범위

    private void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // 시작 시 첫 먹이 소환
        Spawn();
    }

    // 먹이를 스폰 영역 내에 랜덤 생성
    public void Spawn()
    {
        if (spawnArea == null || foodPrefab == null) return;

        Bounds bounds = spawnArea.bounds;
        float margin = 1f; // 벽과의 최소 거리

        float x = Random.Range(bounds.min.x + margin, bounds.max.x - margin);
        float z = Random.Range(bounds.min.z + margin, bounds.max.z - margin);
        Vector3 spawnPos = new Vector3(x, 0.5f, z);

        Instantiate(foodPrefab, spawnPos, Quaternion.identity);
    }
}
