using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerControls controls;
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 moveSpeed;
    private Vector3 movementVector;

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

        if (movementVector.magnitude > 0)
        {
            controller.Move(movementVector * Time.deltaTime * moveSpeed);
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
