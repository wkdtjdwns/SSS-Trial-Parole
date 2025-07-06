using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLookInteraction : MonoBehaviour
{
    [Header("Button Settings")]
    public string buttonTag = "Button"; // 버튼 오브젝트의 태그
    public Color highlightColor = Color.red; // 바라봤을 때 변경될 색
    public string sceneToLoad = "Time2"; // 변경될 씬 이름

    private Camera playerCamera;
    private Renderer currentButtonRenderer;
    private Color originalColor;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        CheckButtonLook(); //버튼을 지금 보고 있는지 확인하는 함수
        CheckInteraction(); //버튼을 눌렀는지 확인하고 씬 이동하는 함수
    }

    void CheckButtonLook() //레이케스트로 버튼 콜라이더를 볼 때 상호작용 할 수 있게 하는 함수
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f))
        {
            if (hit.collider.CompareTag(buttonTag))
            {
                // 버튼 히트 시
                if (currentButtonRenderer == null)
                {
                    currentButtonRenderer = hit.collider.GetComponent<Renderer>(); 
                    originalColor = currentButtonRenderer.material.color;
                    currentButtonRenderer.material.color = highlightColor;
                }
            }
            else
            {
                ResetButtonColor();// 버튼 색 초기화
            }
        }
        else
        {
            ResetButtonColor(); //버튼 색 초기화
        }
    }

    void CheckInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentButtonRenderer != null) //버튼 렌더러가 있을 때 f키를 누른다면 씬 이동
        {
            Debug.Log("F 키 입력");
            SceneManager.LoadScene(sceneToLoad);
        }

    }

    void ResetButtonColor()
    {
        if (currentButtonRenderer != null)
        {
            currentButtonRenderer.material.color = originalColor;
            currentButtonRenderer = null;
        }
    }
}
