using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("적 프리팹 목록 (앞쪽일수록 자주 등장)")]
    public GameObject[] enemyPrefabs;

    [Header("스폰 설정")]
    public float spawnInterval = 1f;   // 생성 간격(초)
    public float spawnHeight = 10f;    // 생성 높이(Y)
    public float spawnRangeX = 10f;    // X축 범위
    public float spawnRangeZ = 5f;     // Z축 범위

    [Header("희귀도 조절 (0~1, 낮을수록 뒤쪽이 잘 안 나옴)")]
    [Range(0f, 1f)] public float rarityFalloff = 0.5f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0) return;

        // 생성 위치 랜덤
        Vector3 pos = new Vector3(
            Random.Range(-spawnRangeX, spawnRangeX),
            spawnHeight,
            Random.Range(-spawnRangeZ, spawnRangeZ)
        );

        // 프리팹 선택
        int index = PickIndex();
        if (index >= 0 && enemyPrefabs[index] != null)
        {
            Instantiate(enemyPrefabs[index], pos, Quaternion.identity);
        }
    }

    int PickIndex()
    {
        float[] weights = new float[enemyPrefabs.Length];
        float total = 0f;

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (enemyPrefabs[i] == null) continue;

            // 희귀도 배율 적용 (앞쪽은 그대로, 뒤로 갈수록 줄어듦)
            float weight = 1f;
            for (int j = 0; j < i; j++)
            {
                weight *= rarityFalloff; // 한 칸 뒤로 갈 때마다 곱하기
            }

            weights[i] = weight;
            total += weight;
        }

        if (total <= 0f) return 0;

        // 랜덤 뽑기
        float rand = Random.value * total;
        float sum = 0f;

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            sum += weights[i];
            if (rand <= sum)
            {
                return i;
            }
        }

        return enemyPrefabs.Length - 1;
    }

    void OnValidate()
    {
        if (spawnInterval < 0.01f) spawnInterval = 0.01f;
        if (spawnRangeX < 0f) spawnRangeX = 0f;
        if (spawnRangeZ < 0f) spawnRangeZ = 0f;
        if (rarityFalloff < 0f) rarityFalloff = 0f;
        if (rarityFalloff > 1f) rarityFalloff = 1f;
    }
}
