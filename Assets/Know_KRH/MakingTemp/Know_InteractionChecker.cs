using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Know_InteractionChecker : MonoBehaviour
{

    [Header("���� �غ�")]
    [SerializeField] GameObject playerBody; //�÷��̾� ī�޶�
    [SerializeField] float checkDistance = 3f;
    
    private Know_Butts currentButton;

    void Update()
    {
        Debug.DrawRay(playerBody.transform.position, playerBody.transform.forward * checkDistance, Color.red); //������

        Ray ray = new Ray(playerBody.transform.position, playerBody.transform.forward); //���� ī�޶� �������� ������ ���
        RaycastHit hit; //�����ɽ�Ʈ ���� ��

        if (Physics.Raycast(ray, out hit, checkDistance)) //�¾�����
        {
            if (hit.collider.CompareTag("Button")) //���� ���� �±װ� button �̶��
            {
                Know_Butts detected = hit.collider.GetComponent<Know_Butts>(); //������ ���� Know_Butts�� detexted�� �ʱ�ȭ

                if (detected != null) //�����Ǿ��ٸ�
                {
                    if (currentButton != detected) //���� ���� �����ִ� ��ư�� �̹� ������ �ְ� �ƴ϶��
                    {
                        ClearHighlight();//���� ��ư�� �����ϰ� ���̶���Ʈ Ǯ��
                        currentButton = detected; //���� ��ư�� ������ �ַ� �ʱ�ȭ
                        currentButton.Highlight(); //���� ��ư�� ���̶���Ʈ(�� �ٲٱ�) �ڵ� ����
                    }

                    if (Input.GetKeyDown(KeyCode.F)) //F Ű ������ �� ��ȣ�ۿ��ϱ�
                    {
                        currentButton.ButtonClick(); //��ư ������ �̺�Ʈ ����
                    }

                    return; //������ ������
                }
            }
        }

        ClearHighlight(); //f �ȴ����� �ƹ� �͵� ������ �� �ʱ�ȭ
    }


    void ClearHighlight() //�� �ʱ�ȭ
    {
        if (currentButton != null) //�ֱ� ��ư�� �����Ѵٸ�
        {
            currentButton.Unhighlight(); //�� �� ���̶���Ʈ ���ְ�
            currentButton = null; //�ֱ� ��ư ���ֱ�
        }
    }


}
