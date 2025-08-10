using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SmallDot : MonoBehaviour
{
    private void Start()
    {
        // Door에 등록
        Door.Instance.RegisterDot(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 닿았을 때만 처리
        if (!other.CompareTag("Player")) return;

        Debug.Log("작은 점 수집");

        // Door에 자기 자신 제거 요청
        Door.Instance.UnregisterDot(this);

        // 오브젝트 제거
        Destroy(gameObject);
    }
}