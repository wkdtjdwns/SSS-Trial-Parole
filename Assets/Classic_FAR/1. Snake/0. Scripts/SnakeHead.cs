using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    [Header("지렁이 설정")]
    public float moveSpeed = 5f;
    public float turnSpeed = 100f;
    public GameObject segmentPrefab;

    private List<Transform> segments = new List<Transform>();
    private List<Vector3> pathPositions = new List<Vector3>();

    void Start()
    {
        segments.Add(transform); // 머리 포함
        pathPositions.Add(transform.position);
    }

    void Update()
    {
        MoveForward();
        HandleTurn();
        UpdateSegmentPositions();
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, pathPositions[pathPositions.Count - 1]) > 0.5f)
        {
            pathPositions.Add(transform.position);
        }
    }

    void HandleTurn()
    {
        float input = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * input * turnSpeed * Time.deltaTime);
    }

    void UpdateSegmentPositions()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            int targetIndex = Mathf.Clamp(pathPositions.Count - 1 - i * 3, 0, pathPositions.Count - 1);
            Vector3 targetPos = pathPositions[targetIndex];
            segments[i].position = Vector3.Lerp(segments[i].position, targetPos, 0.5f);
        }
    }

    public void AddSegment()
    {
        GameObject newSeg = Instantiate(segmentPrefab);
        newSeg.transform.position = segments[segments.Count - 1].position;
        segments.Add(newSeg.transform);
    }

    // 먹이 감지
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            AddSegment();
            Destroy(other.gameObject);
            FoodSpawner.Instance.Spawn();
        }
    }

    // 벽, 꼬리 충돌 감지
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Segment"))
        {
            // 여기만 수정!
            if (GameManager_Classic.Instance != null)
                GameManager_Classic.Instance.GameOver();
            else
                Debug.LogWarning("GameManager_Classic 인스턴스가 없습니다.");
        }
    }

}
