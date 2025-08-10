using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Wall : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Destroy(gameObject); // 공에 맞으면 파괴
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            GameManager_Classic.Instance.GameOver();
        }
    }
}
