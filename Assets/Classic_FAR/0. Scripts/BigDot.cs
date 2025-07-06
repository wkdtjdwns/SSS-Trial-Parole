using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDot : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("큰 점 수집, 모든 유령을 공포 상태로 전환.");

            // 모든 GhostController 찾아서 상태 변경
            GhostController[] ghosts = FindObjectsOfType<GhostController>();
            foreach (GhostController ghost in ghosts)
            {
                ghost.SetState(GhostState.Frightened);
            }

            Destroy(gameObject);
        }
    }
}
