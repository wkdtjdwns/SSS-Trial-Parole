using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class Player_Classic : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] private float walkSpeed = 5f;   // 걷기 속도
    [SerializeField] private float runSpeed = 10f;   // 달리기 속도
    [SerializeField] private float turnSpeed = 65f;  // 회전 속도 (현재 미사용, 향후용)

    [SerializeField] private float jumpForce = 5f;   // 점프 힘
    [SerializeField] private LayerMask groundLayer;  // 지면 레이어

    [Header("플레이어 목숨")]
    [SerializeField] private int health = 3;         // 남은 목숨

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera thirdPersonCameraVCam; // 3인칭 카메라
    [SerializeField] private GameObject firstPersonCamera;                   // 1인칭 카메라

    [Header("Respawn")]
    [SerializeField] private Transform spawnPoint;   // 리스폰 위치

    private float curMoveSpeed;          // 현재 이동 속도
    private bool isFirstPerson = false;  // 1인칭 여부
    [SerializeField] private bool isGrounded; // 접지 여부

    private Rigidbody m_Rigidbody;       // 물리 바디

    private void Start()
    {
        // 필수 컴포넌트 캐싱
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.freezeRotation = true;

        // 기본 이동 속도 설정
        curMoveSpeed = walkSpeed;

        // 카메라 초기 상태
        if (thirdPersonCameraVCam != null) thirdPersonCameraVCam.gameObject.SetActive(true);
        if (firstPersonCamera != null) firstPersonCamera.SetActive(false);
    }

    private void Update()
    {
        Move();          // 이동 입력 처리
        Jump();          // 점프 입력 처리
        Interaction();   // 상호작용 입력 처리
        ToggleCamera();  // 카메라 전환

        // 항상 NavMesh 위에 스냅
        StickToNavMesh();
    }

    private void FixedUpdate()
    {
        // 물리 프레임에서 접지 판정
        GroundCheck();
    }

    // 이동 처리 (WASD/화살표, Ctrl로 달리기)
    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Ctrl 키로 달리기 전환
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            curMoveSpeed = runSpeed;
        else
            curMoveSpeed = walkSpeed;

        // 앞뒤/좌우 이동 (스트레이프)
        Vector3 moveDirection = (transform.forward * v) + (transform.right * h);
        transform.position += moveDirection.normalized * curMoveSpeed * Time.deltaTime;
    }

    // 점프 처리 (스페이스, 접지 상태에서만)
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            m_Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    // 접지 판정 (CapsuleCollider가 있으면 스피어캐스트, 없으면 레이캐스트)
    private void GroundCheck()
    {
        CapsuleCollider capCollider = GetComponent<CapsuleCollider>();
        if (capCollider == null)
        {
            // 단순 레이 방식
            float rayLength = 0.2f;
            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
            isGrounded = Physics.Raycast(rayOrigin, Vector3.down, rayLength, groundLayer);

            Debug.DrawRay(rayOrigin, Vector3.down * rayLength, isGrounded ? Color.green : Color.red);
            return;
        }

        // CapsuleCollider 기반 스피어캐스트
        float sphereRadius = capCollider.radius * 0.95f;
        Vector3 sphereOrigin = transform.position + capCollider.center;
        sphereOrigin.y -= (capCollider.height / 2f - capCollider.radius);

        float groundCheckDistance = 0.1f;
        isGrounded = Physics.SphereCast(
            sphereOrigin,
            sphereRadius,
            Vector3.down,
            out RaycastHit hit,
            groundCheckDistance,
            groundLayer
        );

        Debug.DrawRay(sphereOrigin, Vector3.down * (groundCheckDistance + sphereRadius), isGrounded ? Color.green : Color.red);
    }

    // 상호작용 키 (F) — 현재는 디버그 출력만
    private void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("상호작용!");
        }
    }

    // 카메라 전환 (V)
    private void ToggleCamera()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;

            if (thirdPersonCameraVCam != null)
                thirdPersonCameraVCam.gameObject.SetActive(!isFirstPerson);

            if (firstPersonCamera != null)
                firstPersonCamera.SetActive(isFirstPerson);
        }
    }

    // NavMesh에 스냅 (지형 경계 등에서 매 프레임 보정)
    private void StickToNavMesh()
    {
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            transform.position = hit.position;
    }

    // 플레이어를 스폰 위치로 이동 + 목숨 관리
    public void SpawnPoint()
    {
        if (spawnPoint != null)
        {
            // 위치/속도 초기화
            transform.position = spawnPoint.position;
            m_Rigidbody.velocity = Vector3.zero;

            // 목숨 감소
            health--;
            Debug.Log($"플레이어 리스폰. 남은 목숨: {health}");

            // 목숨이 0 이하 → 게임 종료
            if (health <= 0)
            {
                Debug.Log("목숨 소진 → 게임 종료");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
            else
            {
                Debug.Log($"남은 목숨: {health}");
            }
        }
        else
        {
            Debug.LogWarning("spawnPoint가 설정되어 있지 않습니다.");
        }
    }
}
