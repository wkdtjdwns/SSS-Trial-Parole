using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동")]
    public float moveSpeed = 5f;                     // 이동 속도
    public Vector2 moveRange = new Vector2(10f, 5f); // x, z 이동 제한

    [Header("총알 발사")]
    public GameObject bulletPrefab;              // 발사할 프리팹
    public Transform firePoint;                  // 총알 나가는 위치

    void Update()
    {
        Move();
        Shoot();
    }

    // 플레이어 이동 (정규화/고급 연산 없이 단순 계산)
    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");  // -1, 0, 1
        float z = -Input.GetAxisRaw("Vertical");   

        Vector3 pos = transform.position;

        pos.x += x * moveSpeed * Time.deltaTime;
        pos.z += z * moveSpeed * Time.deltaTime;

        // 범위 제한 (좌우/상하)
        if (pos.x < -moveRange.x) pos.x = -moveRange.x;
        if (pos.x >  moveRange.x) pos.x =  moveRange.x;
        if (pos.z < -moveRange.y) pos.z = -moveRange.y;
        if (pos.z >  moveRange.y) pos.z =  moveRange.y;

        transform.position = pos;
    }

    // 총알 발사
    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (bulletPrefab == null) return;
            if (firePoint == null) return;

            Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        }
    }
}