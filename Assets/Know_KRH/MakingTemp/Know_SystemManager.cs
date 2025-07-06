using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class Know_SystemManager : MonoBehaviour
{
    [System.Serializable]
    public class Quests //문제를 만들기 쉽게 들어갈 텍스트 및 상태 정리
    {
        //public int questNum; //문제 번호
        public string questText; //문제 설몀, 예)1+1=?
        public List<string> chooseText; //선택지의 설명
        public int rightAnswerIndex; //맞는 정답의 버튼넘버
    }

    [Header("컴포넌트")]
    [SerializeField] Know_Butts butt1; //버튼1의 상호작용용
    [SerializeField] GameObject obj1; //버튼 색 관리용 오브젝트
    [SerializeField] Text desc1;
    [SerializeField] Know_Butts butt2;
    [SerializeField] Text desc2; 
    [SerializeField] Know_Butts butt3;
    [SerializeField] Text desc3;
    [SerializeField] Text questText; // 문제가 들어갈 공간 받기
    [SerializeField] HpSystem playerHp; //플레이어 체력 받기

    [Header("상태 및 값")]
    [SerializeField] Quests nowQuest; //현재 문제
    [SerializeField] bool isEnd = false; //문제 푸는 게 다 끝났나


    [Header("문제들")]
    [SerializeField] List<Quests> questList = new List<Quests>(); //리스트를 통해 문제 정리
    [SerializeField] int nowQuestNum; //현재의 문제번호

    [Header("이벤트")]
    [SerializeField] UnityEvent onEndEvent;



    public void Start() //시작 시 문제번호 0(준비되었나)만 넣기
    {
        nowQuestNum = 0;
        nowQuest = questList[0];
        questText.text= nowQuest.questText;
        desc1.text = nowQuest.chooseText[0];
        desc2.text = nowQuest.chooseText[1];
        desc3.text = nowQuest.chooseText[2];
    }
    public void OnButtClick(int buttNum) //버튼을 클릭했을 때
    {
        if (buttNum == nowQuest.rightAnswerIndex)
        {
            Debug.Log("정답");
            nowQuest = questList[nowQuestNum];
            StartCoroutine(ToNextQuest(true));
        }
        else
        {
            Debug.Log("오답");

            StartCoroutine(ToNextQuest(false));
        }
    }

    
    public void Update()
    {
    }

    IEnumerator ToNextQuest(bool IsRight)
    {

        nowQuestNum++;

        if (IsRight)
        {
            questText.text = "정답입니다!";
            desc1.text = "";
            desc2.text = "";
            desc3.text = "";
            
        }
        else
        {
            questText.text = "오답입니다!";
            playerHp.TakeDamage(20);
            desc1.text = "";
            desc2.text = "";
            desc3.text = "";
        }
        if (nowQuestNum >= questList.Count) //만약 문제 전체보다 현재 문제의 번호가 더 크다면
        {
            Debug.LogWarning("끝냈음");
            onEndEvent.Invoke();
            yield break;
        }
        yield return new WaitForSeconds(2.5f);

        nowQuest = questList[nowQuestNum];
        questText.text = nowQuest.questText;
        desc1.text = nowQuest.chooseText[0];
        desc2.text = nowQuest.chooseText[1];
        desc3.text = nowQuest.chooseText[2];
    }
}
