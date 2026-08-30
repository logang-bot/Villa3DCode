using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float gravity = -20f;

    CharacterController controller;
    Transform cam;
    Vector3 velocity;
    InputAction moveAction;
    PlayerConcealment concealment;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        concealment = GetComponent<PlayerConcealment>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        moveAction = new InputAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("Up",    "<Keyboard>/w")
            .With("Down",  "<Keyboard>/s")
            .With("Left",  "<Keyboard>/a")
            .With("Right", "<Keyboard>/d")
            .With("Up",    "<Keyboard>/upArrow")
            .With("Down",  "<Keyboard>/downArrow")
            .With("Left",  "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");
        moveAction.AddBinding("<Gamepad>/leftStick");
        moveAction.Enable();
    }

    void OnDestroy()
    {
        moveAction.Disable();
        moveAction.Dispose();
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        ApplyGravity();
        Move();
    }

    float SpeedMultiplier => concealment != null ? concealment.SpeedMultiplier : 1f;

    void Move()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        if (input.sqrMagnitude < 0.01f) return;

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 direction = (camForward * input.y + camRight * input.x).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        controller.Move(direction * moveSpeed * SpeedMultiplier * Time.deltaTime);
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
