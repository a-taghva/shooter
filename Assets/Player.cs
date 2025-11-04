using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerControls controls;
    private CharacterController controller;
    private Animator animator;
    [SerializeField] private LayerMask aimMask;
    private Vector2 moveInput;
    private float moveSpeed = 5f;
    private Vector3 movementVector;
    private float gravity = 9.81f;
    private float verticalVelocity = -0.5f;
    private Vector3 lookingDirection;

    private Vector2 aimInput;
    private Boolean isRunning;
    void Awake()
    {
        controls = new PlayerControls();

        controls.Character.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Character.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Character.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        controls.Character.Aim.canceled += ctx => aimInput = Vector2.zero;

        controls.Character.Run.performed += ctx => isRunning = true;
        controls.Character.Run.canceled += ctx => isRunning = false;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        applyMovement();
        aimTowardsMouse();
    }

    void applyMovement()
    {
        movementVector = new Vector3(moveInput.x, 0, moveInput.y);
        applyGravity();
        AnimatorController();

        if (movementVector.magnitude > 0)
        {
            if (isRunning)
            {
                moveSpeed = 10f;
            }
            else
            {
                moveSpeed = 5f;
            }
            controller.Move(movementVector * Time.deltaTime * moveSpeed);
        }
    }

    void applyGravity()
    {
        if (!controller.isGrounded)
        {
            // i dont know why * time.deltatime
            verticalVelocity -= gravity * Time.deltaTime;
            movementVector.y = verticalVelocity;
        }
        else
        {
            verticalVelocity = -0.5f;
        }
    }

    void aimTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, aimMask))
        {
            lookingDirection = hit.point - transform.position;
            lookingDirection.y = 0;
            lookingDirection.Normalize();

            transform.forward = lookingDirection;
        }
    }
    
    private void AnimatorController()
    {
        float xVelocity = Vector3.Dot(movementVector.normalized, transform.right);
        float zVelocity = Vector3.Dot(movementVector.normalized, transform.forward);

        animator.SetFloat("xVelocity", xVelocity , 0.1f , Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, 0.1f, Time.deltaTime);

        animator.SetBool("isRunning", isRunning);
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }
}
