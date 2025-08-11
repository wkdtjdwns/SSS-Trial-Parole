using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Models;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private DefaultInput defaultInput;

    public Vector2 inputMovement;
    public Vector2 inputView;

    private Vector3 newCameraRotation;
    private Vector3 newCharacterRotation;

    [Header("References")]
    public Transform cameraHolder;

    [Header("Settings")]
    public PlayerSettingsModel playerSettings;
    public float viewClampYMin = -70;
    public float viewClampYMax = 80;

    [Header("Gravity")]
    public float gravityAmount;
    public float gracityMin;
    private float playerGravity;

    [Header("Jumping")]
    public Vector3 jumpingForce;
    private Vector3 jumpingForceVelocity;

    private void Awake()
    {
        defaultInput = new DefaultInput();

        defaultInput.Character.Movement.performed += e => inputMovement = e.ReadValue<Vector2>();
        defaultInput.Character.View.performed += e => inputView = e.ReadValue<Vector2>();
        defaultInput.Character.Jump.performed += e => Jump();

        defaultInput.Enable();

        newCameraRotation = cameraHolder.localRotation.eulerAngles;
        newCharacterRotation = transform.localRotation.eulerAngles;

        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CalculateView();
        CalculateMovement();
        CalculateJump();
    }

    /// <summary>
    /// 카메라 움직임 계산
    /// </summary>
    private void CalculateView()
    {
        newCharacterRotation.y += playerSettings.ViewXSensitivity * (playerSettings.ViewXinverted ? -inputView.x : inputView.x) * Time.deltaTime;
        transform.rotation = Quaternion.Euler(newCharacterRotation);

        newCameraRotation.x += playerSettings.ViewYSensitivity * (playerSettings.ViewYinverted ? inputView.y : -inputView.y) * Time.deltaTime;
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClampYMin, viewClampYMax);

        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation);
    }

    /// <summary>
    /// 캐릭터 움직임 계산
    /// </summary>
    private void CalculateMovement()
    {
        var verticalSpeed = playerSettings.walkingForwardSpeed * inputMovement.y * Time.deltaTime;
        var horizontalSpeed = playerSettings.WalkingStrafeSpeed * inputMovement.x * Time.deltaTime;

        var newMovementSpeed = new Vector3(horizontalSpeed, 0, verticalSpeed);
        newMovementSpeed = transform.TransformDirection(newMovementSpeed);

        if(playerGravity > gracityMin && jumpingForce.y < 0.1f)
        {
            playerGravity -= gravityAmount * Time.deltaTime;
        }

        //땅인지 판단
        if(playerGravity < -1 && characterController.isGrounded)
        {
            playerGravity = -1;
        }

        if(jumpingForce.y > 0.1f)
        {
            playerGravity = 0;
        }

        newMovementSpeed.y += playerGravity;
        newMovementSpeed += jumpingForce * Time.deltaTime;

        characterController.Move(newMovementSpeed);
    }

    private void CalculateJump()
    {
        jumpingForce = Vector3.SmoothDamp(jumpingForce, Vector3.zero, ref jumpingForceVelocity, playerSettings.jumpingFalloff); //점프 힘을 부드럽게 감소
    }

    private void Jump()
    {
        if(!characterController.isGrounded)
             return;

        jumpingForce = Vector3.up * playerSettings.jumpngHeight;
    }
}