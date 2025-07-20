using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePlayer : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] private float speed = 100f;    // 이동 속도
    [SerializeField] private float turnSpeed = 10f; // 회전 속도

    // 현재 이동 중인 방향
    [SerializeField] private Vector3 moveDir = Vector3.zero;

    [Header("Raycast Settings")]
    [SerializeField] private float raycastDistance = 2f;
    [SerializeField] private LayerMask wallLayer;

    [Header("Collision Bools")]
    [SerializeField] private bool isTop;
    [SerializeField] private bool isBottom;
    [SerializeField] private bool isLeft;
    [SerializeField] private bool isRight;

    private GameObject startPos;
    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        if (rigid == null)
        {
            Debug.LogError("PuzzlePlayer Rigidbody is null :(");
        }

        startPos = GameObject.FindGameObjectWithTag("Respawn");
        if (startPos == null)
        {
            Debug.LogError("StartPosition is null :(");
        }

        wallLayer = LayerMask.GetMask("Wall");

        Respawn();
    }

    private void Update()
    {
        HotKey();
        DecideDirection();
    }

    private void FixedUpdate()
    {
        UpdateRaycastDetection();
        Move();
    }

    private void Respawn()
    {
        if (startPos != null)
        {
            this.transform.position = startPos.transform.position;
            rigid.velocity = Vector3.zero;
            moveDir = Vector3.zero;

            isTop = false;
            isBottom = false;
            isLeft = false;
            isRight = false;
        }

        else
        {
            Debug.LogError("StartPosition is null :(");
        }
    }

    private void HotKey()
    {
        if (Input.GetKeyDown(KeyCode.R)) { Respawn(); }
    }

    private void DecideDirection()
    {
        if (moveDir != Vector3.zero) { return; }

        if (Input.GetKeyDown(KeyCode.A) && !isLeft) { moveDir = Vector3.left; }
        else if (Input.GetKeyDown(KeyCode.D) && !isRight) { moveDir = Vector3.right; }
        else if (Input.GetKeyDown(KeyCode.W) && !isTop) { moveDir = Vector3.forward; }
        else if (Input.GetKeyDown(KeyCode.S) && !isBottom) { moveDir = Vector3.back; }
    }

    private void UpdateRaycastDetection()
    {
        isTop = false;
        isBottom = false;
        isLeft = false;
        isRight = false;

        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, raycastDistance, wallLayer)) { isTop = true; }
        if (Physics.Raycast(transform.position, Vector3.back, out hit, raycastDistance, wallLayer)) { isBottom = true; }
        if (Physics.Raycast(transform.position, Vector3.left, out hit, raycastDistance, wallLayer)) { isLeft = true; }
        if (Physics.Raycast(transform.position, Vector3.right, out hit, raycastDistance, wallLayer)) { isRight = true; }
    }

    private void Move()
    {
        if ((moveDir == Vector3.left && isLeft) ||
            (moveDir == Vector3.right && isRight) ||
            (moveDir == Vector3.forward && isTop) ||
            (moveDir == Vector3.back && isBottom))
        {
            moveDir = Vector3.zero;
        }

        rigid.velocity = moveDir * speed;

        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            Debug.Log("게임 클리어!");
        }
    }
}