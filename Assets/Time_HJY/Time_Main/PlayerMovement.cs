using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal"); 
        float v = Input.GetAxis("Vertical");   

        // 카메라 기준 전후좌우
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * v + right * h;

        if (moveDir.magnitude < 0.1f) return;

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed; // 달리기

        Vector3 velocity = moveDir * speed;

        Vector3 newPosition = rb.position + velocity * Time.fixedDeltaTime; // Rigidbody로 이동함

        rb.MovePosition(newPosition);
    }
}
