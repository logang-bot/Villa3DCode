# Player Movement

## PlayerMovement.cs
**Path**: `Scripts/Player/PlayerMovement.cs`
**Attach to**: The player GameObject (also needs a `CharacterController` component)

| Field | Type | Default | Notes |
|---|---|---|---|
| `moveSpeed` | float | 4 | Units per second |
| `rotationSpeed` | float | 10 | Slerp speed toward movement direction |
| `gravity` | float | -20 | Applied manually each frame |

**Behaviour**:
- WASD / arrow keys / gamepad left stick for movement
- Movement is camera-relative (reads `Camera.main.transform`)
- Character rotates to face movement direction via `Quaternion.Slerp`
- Cursor locked on Awake; **Escape** unlocks it
- Input bindings defined in code — no InputActionAsset needed

---

## Scene Wiring — Hub_Zone01
```
Capsule                          ← placeholder player
├── Transform: position (0, 1, -14)   ← south edge of the plaza, near Street_South
├── Tag: Player
├── CharacterController          (default settings)
└── PlayerMovement
    ├── Move Speed: 4
    ├── Rotation Speed: 10
    └── Gravity: -20
```

---

## Design Decisions
- **CharacterController over Rigidbody**: Direct control without physics interference; standard for this genre
