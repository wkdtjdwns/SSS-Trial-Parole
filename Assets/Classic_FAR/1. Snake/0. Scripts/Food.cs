using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Food : MonoBehaviour
{
    [Header("회전 설정")]
    [Tooltip("초당 회전 속도")]
    [SerializeField] private float rotateSpeed = 100f;

    private void Update()
    {
        // Y축으로 연속 회전
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }
}
