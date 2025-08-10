using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    public float interval = 3f;      // 벽을 내리는 시간 간격(초)
    public float dropDistance = 1f;  // 한 번 내릴 때 이동 거리
    private float timer;             // 시간 측정용 변수

    void Update()
    {
        // 매 프레임마다 경과 시간 더하기
        timer += Time.deltaTime;

        // 설정한 시간 간격(interval) 이상이 지나면
        if (timer >= interval)
        {
            DropWalls(); // 벽을 한 칸 내림
            timer = 0f;  // 타이머 초기화
        }

        // 남은 벽이 있는지 확인
        CheckWallsCleared();
    }

    // 모든 벽을 dropDistance만큼 아래로 내리는 함수
    void DropWalls()
    {
        // 이 스크립트가 붙은 오브젝트의 자식들을 하나씩 확인
        foreach (Transform wall in transform)
        {
            // 각 자식(벽)의 위치를 아래로 이동
            wall.position += Vector3.down * dropDistance;
        }
    }

    // 벽이 다 사라졌는지 확인하는 함수
    void CheckWallsCleared()
    {
        // 씬 안에서 Wall 스크립트가 붙은 오브젝트 개수를 찾음
        if (FindObjectsOfType<Wall>().Length == 0)
        {
            // 벽이 하나도 없으면 GameManager에 클리어
            GameManager_Classic.Instance.GameClear();
        }
    }
}
