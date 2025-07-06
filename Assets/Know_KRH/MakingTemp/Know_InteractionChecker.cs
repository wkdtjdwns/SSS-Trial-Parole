using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Know_InteractionChecker : MonoBehaviour
{

    [Header("기초 준비")]
    [SerializeField] GameObject playerBody; //플레이어 카메라
    [SerializeField] float checkDistance = 3f;
    
    private Know_Butts currentButton;

    void Update()
    {
        Debug.DrawRay(playerBody.transform.position, playerBody.transform.forward * checkDistance, Color.red); //디버깅용

        Ray ray = new Ray(playerBody.transform.position, playerBody.transform.forward); //레이 카메라 기준으로 앞으로 쏘고
        RaycastHit hit; //레이케스트 맞은 거

        if (Physics.Raycast(ray, out hit, checkDistance)) //맞았으며
        {
            if (hit.collider.CompareTag("Button")) //맞은 애의 태그가 button 이라면
            {
                Know_Butts detected = hit.collider.GetComponent<Know_Butts>(); //감지된 애의 Know_Butts를 detexted에 초기화

                if (detected != null) //감지되었다면
                {
                    if (currentButton != detected) //만약 현재 보고있는 버튼이 이미 감지된 애가 아니라면
                    {
                        ClearHighlight();//현재 버튼을 무시하고 하이라이트 풀기
                        currentButton = detected; //현재 버튼을 감지된 애로 초기화
                        currentButton.Highlight(); //현재 버튼의 하이라이트(색 바꾸기) 코드 실행
                    }

                    if (Input.GetKeyDown(KeyCode.F)) //F 키 눌렀을 때 상호작용하기
                    {
                        currentButton.ButtonClick(); //버튼 누르는 이벤트 실행
                    }

                    return; //했으면 나오기
                }
            }
        }

        ClearHighlight(); //f 안눌렀고 아무 것도 없으면 색 초기화
    }


    void ClearHighlight() //색 초기화
    {
        if (currentButton != null) //최근 버튼이 존재한다면
        {
            currentButton.Unhighlight(); //그 애 하이라이트 없애고
            currentButton = null; //최근 버튼 없애기
        }
    }


}
