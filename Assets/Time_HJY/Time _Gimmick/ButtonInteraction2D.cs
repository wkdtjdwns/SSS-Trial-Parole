using UnityEngine;
using System.Collections.Generic;

public class ButtonInteraction2D : MonoBehaviour
{
    public GameObject door;
    private HashSet<GameObject> objectsOnButton = new HashSet<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (IsValidTrigger(other))
        {
            objectsOnButton.Add(other.gameObject);
            UpdateDoorState();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (objectsOnButton.Contains(other.gameObject))
        {
            objectsOnButton.Remove(other.gameObject);
            UpdateDoorState();
        }
    }

    bool IsValidTrigger(Collider other)
    {
        return other.CompareTag("Player") || other.CompareTag("Ghost");
    }

    void UpdateDoorState()
    {
        if (door != null)
        {
            door.SetActive(objectsOnButton.Count == 0); // 문은 플레이어나 리플레이가 버튼 위에 있으면 열림 (SetActive(false)), 아무도 없으면 닫힘 (SetActive(true))
        }
    }

    public void ResetButton()
    {
        objectsOnButton.Clear();
        UpdateDoorState();
    }

}