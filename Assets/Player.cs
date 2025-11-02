using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerControls controls;
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 moveSpeed;
    private Vector3 movementVector;
    private float gravity = 9.81f;
    private float verticalVelocity = -0.5f;

    void Awake()
    {
        controls = new PlayerControls();

        controls.Character.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Character.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        applyMovement();
    }

    void applyMovement()
    {
        movementVector = new Vector3(moveInput.x, 0, moveInput.y);
        applyGravity();

        if (movementVector.magnitude > 0)
        {
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

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }
}
