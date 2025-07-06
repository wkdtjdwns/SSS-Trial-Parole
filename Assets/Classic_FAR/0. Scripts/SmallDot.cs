using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SmallDot : MonoBehaviour
{
    private void Start()
    {
        Door.Instance.RegisterDot(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("작은 점 수집");
            Door.Instance.UnregisterDot(this);
            Destroy(gameObject);
        }
    }
}
