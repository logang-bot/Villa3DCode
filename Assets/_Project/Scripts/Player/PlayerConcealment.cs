using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConcealment : MonoBehaviour, IConcealmentProvider
{
    [SerializeField] float crouchSpeedMultiplier = 0.5f;
    [SerializeField] float crouchRiseRateMultiplier = 0.4f;

    InputAction crouchAction;

    public bool IsConcealed { get; private set; }
    public float SpeedMultiplier => IsConcealed ? crouchSpeedMultiplier : 1f;
    public float RiseRateMultiplier => IsConcealed ? crouchRiseRateMultiplier : 1f;

    void Awake()
    {
        crouchAction = new InputAction("Crouch", InputActionType.Button);
        crouchAction.AddBinding("<Keyboard>/leftCtrl");
        crouchAction.AddBinding("<Gamepad>/buttonEast");
        crouchAction.performed += _ => IsConcealed = !IsConcealed;
        crouchAction.Enable();
    }

    void OnDestroy()
    {
        crouchAction.Disable();
        crouchAction.Dispose();
    }
}
