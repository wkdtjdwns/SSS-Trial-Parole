using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float turnSpeed = 65f;

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera thirdPersonCameraVCam;
    [SerializeField] private GameObject firstPersonCamera;

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

        // 3인칭으로 게임 시작
        if (thirdPersonCameraVCam != null) thirdPersonCameraVCam.gameObject.SetActive(true);
        if (firstPersonCamera != null) firstPersonCamera.SetActive(false);
    }

    private void Update()
    {
        Move();
        Jump();

        Interaction();

        ToggleCamera();
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) { curMoveSpeed = runSpeed; }
        else { curMoveSpeed = walkSpeed; }

        // ###########################################################################
        // -- 수정 이동 로직
        // ###########################################################################
        transform.Rotate(0f, h * turnSpeed * Time.deltaTime, 0f);

        Vector3 moveDirection = transform.forward * v;
        transform.position += moveDirection * curMoveSpeed * Time.deltaTime;

        // ###########################################################################
        // -- 원래 이동 로직
        // ###########################################################################
        //m_Movement = new Vector3(h, 0, v).normalized;

        //transform.position += m_Movement * curMoveSpeed * Time.deltaTime;

        //if (m_Movement != Vector3.zero)
        //{
        //    Quaternion toRotation = Quaternion.LookRotation(m_Movement, Vector3.up);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
        //}
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

    // 오버라이딩해서 쓸려했는데 각이 안 나와서 일단 보류
    //public virtual void InteractionDetail()
    //{
    //    print("상호작용!");
    //}

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
}
