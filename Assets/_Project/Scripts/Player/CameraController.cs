using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineCamera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] float sensitivity = 0.2f;
    [SerializeField] float verticalMin = -20f;
    [SerializeField] float verticalMax = 60f;

    CinemachineOrbitalFollow orbitalFollow;

    void Awake()
    {
        orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
    }

    void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        Vector2 delta = Mouse.current.delta.ReadValue();

        orbitalFollow.HorizontalAxis.Value += delta.x * sensitivity;
        orbitalFollow.VerticalAxis.Value = Mathf.Clamp(
            orbitalFollow.VerticalAxis.Value - delta.y * sensitivity,
            verticalMin, verticalMax
        );
    }
}
