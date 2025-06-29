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

    void CheckButtonLook()
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
                ResetButtonColor();
            }
        }
        else
        {
            ResetButtonColor();
        }
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
