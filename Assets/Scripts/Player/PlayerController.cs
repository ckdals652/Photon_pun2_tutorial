using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPun
{
    private Rigidbody playerRigidBody;
    private PlayerAction input;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float verticalSpeed = 5f;
    [SerializeField] private float dashForce = 15f;
    [SerializeField] private float maxSpeed = 10f;

    private bool dashPressed = false;
    private Vector3 lastMoveDirection = Vector3.forward;

    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        input = new PlayerAction();
    }

    private void OnEnable()
    {
        if (photonView.IsMine)
        {
            //이동 활성화
            input.Enable();
        }
    }

    private void OnDisable()
    {
        if (photonView.IsMine)
        {
            //이동 비활성화
            input.Disable();
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (input.PlayerActionMap.Dash.triggered)
        {
            dashPressed = true;
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        Move();

        if (dashPressed)
        {
            Dash();
            dashPressed = false;
        }
    }

    private void Move()
    {
        // 1. 방향 입력 받기 (Vector2)
        Vector2 moveInput = input.PlayerActionMap.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        // 2. 수직 입력 (RiseFall → Space or Ctrl 둘 중 하나라도 누르면)
        float y = 0f;
        if (Keyboard.current.spaceKey.isPressed) y += 1f;
        if (Keyboard.current.leftCtrlKey.isPressed) y -= 1f;

        // 3. 최종 속도 설정
        Vector3 force = (move * moveSpeed) + (Vector3.up * (y * verticalSpeed));
        playerRigidBody.AddForce(force,ForceMode.Force);
        
        //입력한 마지막 방향 정해주기
        if (force != Vector3.zero)
        {
            lastMoveDirection = playerRigidBody.velocity;
        }
        
        // 속도 제한
        if (playerRigidBody.velocity.magnitude > maxSpeed)
        {
            playerRigidBody.velocity = playerRigidBody.velocity.normalized * maxSpeed;
        }
    }

    private void Dash()
    {
        playerRigidBody.AddForce(lastMoveDirection.normalized
                                 * dashForce, ForceMode.Impulse);
    }
}