using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        if (!GhostManager.Instance.IsGhostActive)
        {
            Move();
        }
        else
        {
            // 멈추기
            rb.velocity = Vector3.zero;
        }
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        bool isJumping = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space);

        Vector3 moveDir = Vector3.zero;

        if (Mathf.Abs(h) > 0.1f)
        {
            moveDir.z = h;
        }

        if (isJumping && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, moveDir.z * moveSpeed);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
