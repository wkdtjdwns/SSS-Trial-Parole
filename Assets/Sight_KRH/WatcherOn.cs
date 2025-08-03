using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WatcherOn : MonoBehaviour
{
    [Header("Get Cams")]
    [SerializeField] private Camera enemyCamera;           // 강조용 특수 카메라
    [SerializeField] private Camera mainCamera;            // 메인 시야 카메라

    [Header("Post-Processing")]
    [SerializeField] Material effectMaterial;      // 흑백 필터용 마테리알
    [SerializeField] RenderTexture enemyRT;        // 특수용 렌더텍스처
    [SerializeField] Shader highlightShader;        //특수용 강조 셰이더

    [Header("UI Overlay")]
    [SerializeField] RawImage overlayImage;        // UI에 출력될 생이미지 렌더텍스쳐 표시용


    private bool isShaderActive = false;


    void Start()
    {
        // 카메라 설정: enemyCamera가 위에 그려지도록 depth 설정
        mainCamera.depth = 0;
        enemyCamera.depth = 1;

        // clearFlags 설정
        mainCamera.clearFlags = CameraClearFlags.Skybox;
        enemyCamera.clearFlags = CameraClearFlags.SolidColor;
        enemyCamera.backgroundColor = new Color(0, 0, 0, 0); // 알파 0

        // 강조 카메라는 처음엔 비활성화
        enemyCamera.enabled = false;
        enemyCamera.SetReplacementShader(highlightShader, "RenderType");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isShaderActive = !isShaderActive; //버튼 딸깍으로 바로 전환

            enemyCamera.enabled = isShaderActive; //전환 적용

            if (overlayImage != null)
                overlayImage.enabled = isShaderActive;
        }
        if (!isShaderActive)
        {
            enemyRT.DiscardContents(); // RenderTexture 내용 삭제
        }
    }
    void LateUpdate()
    {
        // 메인 카메라 위치/회전을 강조 카메라에 복사
        if (enemyCamera.enabled)
        {
            enemyCamera.transform.position = mainCamera.transform.position;
            enemyCamera.transform.rotation = mainCamera.transform.rotation;
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest) //후처리용 코드(카메라 있을 때만 적용됨), src=처음 받는 기초, dest=후처리된 최종 화면
    {
        if (effectMaterial != null && isShaderActive)
        {
            Graphics.Blit(src, dest, effectMaterial); // 흑백 셰이더 적용
        }
        else
        {
            Graphics.Blit(src, dest); // 원본 그대로 출력
        }
    }
}
