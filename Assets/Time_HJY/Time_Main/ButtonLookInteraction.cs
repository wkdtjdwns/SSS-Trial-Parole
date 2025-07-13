using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLookInteraction : MonoBehaviour
{
    public string buttonTag = "Button"; // 버튼 오브젝트에 부여한 태그 이름
    public Color highlightColor = Color.red; // 바라봤을 때 버튼이 바뀔 색상
    public string sceneToLoad = "Time2"; // F 키를 눌렀을 때 이동할 씬 이름

    private Camera playerCamera; // 플레이어 카메라 참조
    private Renderer currentButtonRenderer; // 현재 바라보고 있는 버튼의 렌더러
    private Color originalColor; // 버튼 원래 색상 저장용

    void Start()
    {
        playerCamera = Camera.main; // 플레이어 카메라를 가져옴 (MainCamera 태그 있는거)
    }

    void Update()
    {
        CheckButtonLook(); // 버튼을 바라보고 있는지 체크
        CheckInteraction(); // F 키 입력 감지해서 씬 전환
    }

    /// 플레이어가 버튼을 바라보고 있으면 버튼 색을 변경함
    void CheckButtonLook()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward); // 전방 레이 발사
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f)) // 3미터 내에 닿은 오브젝트가 있다면
        {
            if (hit.collider.CompareTag(buttonTag)) // 태그가 "Button"인 경우
            {
                Renderer newRenderer = hit.collider.GetComponent<Renderer>();

                if (currentButtonRenderer != newRenderer) // 새로운 버튼을 바라봤다면
                {
                    ResetButtonColor(); // 이전 버튼 색 복원

                    currentButtonRenderer = newRenderer;
                    originalColor = currentButtonRenderer.material.color; // 원래 색 저장
                    currentButtonRenderer.material.color = highlightColor; // 강조 색상으로 변경
                }

                return; // 버튼을 바라보는 상태이므로 여기서 함수 종료
            }
        }
        ResetButtonColor(); // 버튼이 아니거나 아무것도 안 보고 있을 경우 초기화
    }

    /// F 키를 누르면 씬 이동
    void CheckInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentButtonRenderer != null)
        {
            Debug.Log("F 키 입력됨 - 씬 이동");
            SceneManager.LoadScene(sceneToLoad); // 지정된 씬으로 이동
        }
    }

    /// 버튼 색을 원래대로 되돌리는 함수
    void ResetButtonColor()
    {
        if (currentButtonRenderer != null)
        {
            currentButtonRenderer.material.color = originalColor;
            currentButtonRenderer = null;
        }
    }
}
