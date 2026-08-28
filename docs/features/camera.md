# Camera

## CameraController.cs
**Path**: `Scripts/Player/CameraController.cs`
**Attach to**: The CM Camera GameObject (same object as `CinemachineCamera`)

| Field | Type | Default | Notes |
|---|---|---|---|
| `sensitivity` | float | 0.2 | Mouse delta multiplier |
| `verticalMin` | float | -20 | Minimum tilt angle |
| `verticalMax` | float | 60 | Maximum tilt angle |

**Behaviour**:
- Reads `Mouse.current.delta` every frame
- Drives `CinemachineOrbitalFollow.HorizontalAxis.Value` (pan) and `VerticalAxis.Value` (tilt)
- Only active while cursor is locked

---

## Scene Wiring — Hub_Zone01
```
Main Camera
├── Tag: MainCamera
└── CinemachineBrain             (default settings)

CM Camera
├── CinemachineCamera
│   └── Tracking Target → Capsule
├── CinemachineOrbitalFollow
│   ├── Orbit Style: Sphere
│   └── Radius: 5
├── CinemachineRotationComposer  (default — auto-aims at tracking target)
└── CameraController
    ├── Sensitivity: 0.2
    ├── Vertical Min: -20
    └── Vertical Max: 60
```

---

## Design Decisions
- **Cinemachine OrbitalFollow + RotationComposer**: Decouples camera orbit from character rotation; `ThirdPersonFollow` caused jitter and moved the camera on WASD because it is relative to the character's local space
- **Manual camera input (`CameraController.cs`) over `CinemachineInputAxisController`**: The auto-binding picked up keyboard input instead of mouse
