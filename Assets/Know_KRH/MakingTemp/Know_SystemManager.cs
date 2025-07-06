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
    public class Quests //������ ����� ���� �� �ؽ�Ʈ �� ���� ����
    {
        //public int questNum; //���� ��ȣ
        public string questText; //���� ���m, ��)1+1=?
        public List<string> chooseText; //�������� ����
        public int rightAnswerIndex; //�´� ������ ��ư�ѹ�
    }

    [Header("������Ʈ")]
    [SerializeField] Know_Butts butt1; //��ư1�� ��ȣ�ۿ��
    [SerializeField] GameObject obj1; //��ư �� ������ ������Ʈ
    [SerializeField] Text desc1;
    [SerializeField] Know_Butts butt2;
    [SerializeField] Text desc2; 
    [SerializeField] Know_Butts butt3;
    [SerializeField] Text desc3;
    [SerializeField] Text questText; // ������ �� ���� �ޱ�
    [SerializeField] HpSystem playerHp; //�÷��̾� ü�� �ޱ�

    [Header("���� �� ��")]
    [SerializeField] Quests nowQuest; //���� ����
    [SerializeField] bool isEnd = false; //���� Ǫ�� �� �� ������


    [Header("������")]
    [SerializeField] List<Quests> questList = new List<Quests>(); //����Ʈ�� ���� ���� ����
    [SerializeField] int nowQuestNum; //������ ������ȣ

    [Header("�̺�Ʈ")]
    [SerializeField] UnityEvent onEndEvent;



    public void Start() //���� �� ������ȣ 0(�غ�Ǿ���)�� �ֱ�
    {
        nowQuestNum = 0;
        nowQuest = questList[0];
        questText.text= nowQuest.questText;
        desc1.text = nowQuest.chooseText[0];
        desc2.text = nowQuest.chooseText[1];
        desc3.text = nowQuest.chooseText[2];
    }
    public void OnButtClick(int buttNum) //��ư�� Ŭ������ ��
    {
        if (buttNum == nowQuest.rightAnswerIndex)
        {
            Debug.Log("����");
            nowQuest = questList[nowQuestNum];
            StartCoroutine(ToNextQuest(true));
        }
        else
        {
            Debug.Log("����");

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
            questText.text = "�����Դϴ�!";
            desc1.text = "";
            desc2.text = "";
            desc3.text = "";
            
        }
        else
        {
            questText.text = "�����Դϴ�!";
            playerHp.TakeDamage(20);
            desc1.text = "";
            desc2.text = "";
            desc3.text = "";
        }
        if (nowQuestNum >= questList.Count) //���� ���� ��ü���� ���� ������ ��ȣ�� �� ũ�ٸ�
        {
            Debug.LogWarning("������");
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
