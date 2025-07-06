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
        CheckButtonLook();
        CheckInteraction();
    }

    void CheckButtonLook() //레이케스트로 버튼 콜라이더를 볼 때 상호작용 할 수 있게 하는 함수
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f))
        {
            // 버튼에 닿았는가?
            if (hit.collider.CompareTag(buttonTag))
            {
                Renderer newRenderer = hit.collider.GetComponent<Renderer>();

                // 버튼이 새로 바뀌었는가?
                if (currentButtonRenderer != newRenderer)
                {
                    ResetButtonColor(); // 이전 버튼 색 되돌림

                    currentButtonRenderer = newRenderer;
                    originalColor = currentButtonRenderer.material.color;
                    currentButtonRenderer.material.color = highlightColor;
                }

                return; // 버튼 맞았고, 처리 완료했으면 끝
            }
        }
        // 여기에 왔다는 건 버튼 안 보고 있음
        ResetButtonColor();
    }

    void CheckInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentButtonRenderer != null)
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
