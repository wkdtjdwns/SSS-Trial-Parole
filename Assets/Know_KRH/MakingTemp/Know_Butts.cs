using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class Know_Butts : MonoBehaviour
{
    [Header("�̺�Ʈ")]
    [SerializeField] UnityEvent onButtonClick;
    public void ButtonClick()
    {
        onButtonClick.Invoke();
        
    }


    [Header("��ư �� ���� ����")]
    [SerializeField] Renderer rend;
    [SerializeField] Color originalColor;
    [SerializeField] Color highlightColor;
    [SerializeField] Color rightButtColor;
    [SerializeField] Color wrongButtColor;


   
    public void Highlight()
    {
        {
            rend.material.color = highlightColor;
        }
    }
    public void Unhighlight()
    {
        {
            rend.material.color = originalColor;
        }
    }
}
