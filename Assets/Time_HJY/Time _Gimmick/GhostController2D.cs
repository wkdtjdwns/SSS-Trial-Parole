using UnityEngine;

public class GhostController2D : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        if (!GhostManager.Instance.IsGhostActive) return;

        float h = Input.GetAxis("Horizontal"); // A/D
        float v = Input.GetAxis("Vertical");   // W/S (or 위/아래 방향키)

        // Z축 이동 ➜ A/D = 좌우로 이동, W/S = 위/아래 이동
        Vector3 moveDir = new Vector3(0f, v, h);
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        GhostManager.Instance.RecordPosition(transform.position);
    }
}
