using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public static Door Instance;

    private List<SmallDot> smallDots = new List<SmallDot>();

    [Header("모든 점을 수집했는지 여부")]
    public bool allDotsCollected = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// 작은 점 등록
    public void RegisterDot(SmallDot dot)
    {
        smallDots.Add(dot);
    }

    /// 작은 점 제거
    public void UnregisterDot(SmallDot dot)
    {
        smallDots.Remove(dot);

        if (smallDots.Count == 0)
        {
            AllDotsCollected();
        }
    }

    private void AllDotsCollected()
    {
        allDotsCollected = true;
        Debug.Log("모든 작은 점을 수집했습니다");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!allDotsCollected)
            return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("게임을 종료합니다.");

            #if UNITY_EDITOR
                // 에디터에서 중지
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                // 빌드 실행 파일 종료
                Application.Quit();
            #endif
        }
    }
}
