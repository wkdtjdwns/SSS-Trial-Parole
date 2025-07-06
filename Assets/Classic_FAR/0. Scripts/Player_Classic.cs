using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class Player_Classic : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float turnSpeed = 65f;

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("플레이어 목숨")]
    [SerializeField] private int health = 3;


    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera thirdPersonCameraVCam;
    [SerializeField] private GameObject firstPersonCamera;

    [Header("Respawn")]
    [SerializeField] private Transform spawnPoint;
    


    private float curMoveSpeed;

    private bool isFirstPerson = false;
    [SerializeField] private bool isGrounded;

    private Rigidbody m_Rigidbody;
    private Vector3 m_Movement;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.freezeRotation = true;

        curMoveSpeed = walkSpeed;

        if (thirdPersonCameraVCam != null) thirdPersonCameraVCam.gameObject.SetActive(true);
        if (firstPersonCamera != null) firstPersonCamera.SetActive(false);
    }

    private void Update()
    {
        Move();
        Jump();

        Interaction();

        ToggleCamera();

        // NavMesh에 항상 스냅
        StickToNavMesh();
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            curMoveSpeed = runSpeed;
        }
        else
        {
            curMoveSpeed = walkSpeed;
        }

        // 앞뒤 + 좌우 이동 (스트레이프)
        Vector3 moveDirection = (transform.forward * v) + (transform.right * h);

        transform.position += moveDirection.normalized * curMoveSpeed * Time.deltaTime;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            m_Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void GroundCheck()
    {
        CapsuleCollider capCollider = GetComponent<CapsuleCollider>();
        if (capCollider == null)
        {
            float rayLength = 0.2f;

            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
            isGrounded = Physics.Raycast(rayOrigin, Vector3.down, rayLength, groundLayer);

            Debug.DrawRay(rayOrigin, Vector3.down * rayLength, isGrounded ? Color.green : Color.red);

            return;
        }

        float sphereRadius = capCollider.radius * 0.95f;

        Vector3 sphereOrigin = transform.position + capCollider.center;
        sphereOrigin.y -= (capCollider.height / 2f - capCollider.radius);

        float groundCheckDistance = 0.1f;
        isGrounded = Physics.SphereCast(sphereOrigin, sphereRadius, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer);

        Debug.DrawRay(sphereOrigin, Vector3.down * (groundCheckDistance + sphereRadius), isGrounded ? Color.green : Color.red);
    }

    private void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            print("상호작용!");
        }
    }

    private void ToggleCamera()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;

            if (thirdPersonCameraVCam != null)
            {
                thirdPersonCameraVCam.gameObject.SetActive(!isFirstPerson);
            }

            if (firstPersonCamera != null)
            {
                firstPersonCamera.SetActive(isFirstPerson);
            }
        }
    }

    /// 플레이어를 항상 NavMesh 위에 스냅
    private void StickToNavMesh()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
    }

    public void SpawnPoint()
{
    if (spawnPoint != null)
    {
        transform.position = spawnPoint.position;
        m_Rigidbody.velocity = Vector3.zero;

        // 목숨 감소
        health--;
        Debug.Log($"플레이어를 스폰 위치로 이동했습니다. 남은 목숨: {health}");

        // 목숨이 0 이하이면 종료
        if (health <= 0)
        {
            Debug.Log("목숨이 모두 소진되었습니다. 게임 종료.");

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
