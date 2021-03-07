using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject MainCam;
    [SerializeField] private float moveSpeed;

    private Animator animator;
    private CharacterController characterController;
    private float currentVelocityY;
    private float padDistance;

    public Vector2 moveInput { get; set; }
    public float currentSpeed =>
        new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        moveInput = Vector2.zero;
    }

    void FixedUpdate()
    {
        Move();
        if (currentSpeed >= 0.1f) Look();

        UpdateAnimator();
    }

    public void Move()
    {
        currentVelocityY += Time.deltaTime * Physics.gravity.y;
        if (characterController.isGrounded) currentVelocityY = 0f;

        float targetSpeed = moveSpeed * padDistance * moveInput.magnitude;
        Vector3 moveDirection =
            Vector3.Normalize(MainCam.transform.forward * moveInput.y + MainCam.transform.right * moveInput.x);
        moveDirection.y = 0f;//y좌표 고정 - 필요시 나중에 개선

        var velocity = moveDirection * targetSpeed + Vector3.up * currentVelocityY;
        // x,z평면 속도 계산 + y평면 속도 계산
        characterController.Move(velocity * Time.deltaTime);
    }

    public void Look()
    {
        Vector3 targetDir = transform.position + new Vector3(moveInput.x, 0f, moveInput.y) * 100;

        Vector3 moveDirection =
            Vector3.Normalize(MainCam.transform.forward * moveInput.y + MainCam.transform.right * moveInput.x);

        moveDirection.y = 0;

        transform.LookAt(transform.position + moveDirection);
    }

    public void SetValue(Vector2 move, float distance)
    {
        moveInput = move;
        padDistance = distance;
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("Speed", currentSpeed);
    }
}
