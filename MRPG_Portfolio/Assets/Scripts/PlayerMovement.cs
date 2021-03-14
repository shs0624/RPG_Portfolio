using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject MainCam;

    private float currentVelocityY;
    private Animator _animator;
    private CharacterController _characterController;
    private float padDistance;

    public float moveSpeed;
    [HideInInspector]
    public Vector2 moveInput { get; set; }
    [HideInInspector]
    public float currentSpeed =>
        new Vector2(_characterController.velocity.x, _characterController.velocity.z).magnitude;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveUpdate();
        if (currentSpeed >= 0.1f) Look();

        _animator.SetFloat("Speed", currentSpeed);
    }

    private void MoveUpdate()
    {
        currentVelocityY += Time.deltaTime * Physics.gravity.y;
        if (_characterController.isGrounded) currentVelocityY = 0f;

        float targetSpeed = moveSpeed * padDistance * moveInput.magnitude;
        Vector3 moveDirection =
            Vector3.Normalize(MainCam.transform.forward * moveInput.y + MainCam.transform.right * moveInput.x);
        moveDirection.y = 0f;//y좌표 고정 - 필요시 나중에 개선

        var velocity = moveDirection * targetSpeed + Vector3.up * currentVelocityY;
        // x,z평면 속도 계산 + y평면 속도 계산

        _characterController.Move(velocity * Time.deltaTime);
    }

    private void Look()
    {
        Vector3 moveDirection =
            Vector3.Normalize(MainCam.transform.forward * moveInput.y + MainCam.transform.right * moveInput.x);

        moveDirection.y = 0f;

        transform.LookAt(transform.position + moveDirection);
    }

    public void SetValue(Vector2 move, float distance)
    {
        moveInput = move;
        padDistance = distance;
    }
}
