# Hub Environment

Documents the real-art geometry for hub zones — layout, dimensions, and materials. No scripts are involved; this is pure scene/asset wiring.

## Scene Wiring — Hub_Zone01 (Potosí Plaza)

The plaza is now built from real models imported from the `VillaCity.blend` source (see `[[blender-source-file]]` memory) — all primitive greybox geometry has been retired in favor of real imports. Positions follow a single fixed coordinate mapping from the Blender file's world space — see `[[hub-zone01-coordinate-mapping]]` memory — anchored on the real `Plaza_Pavement` object. That mapping uses a **zero global offset** (Blender `(X, Y)` → Unity `(X, 0, Y)`) **plus a mandatory `(0, 180, 0)` root rotation on every import** — see the orientation gotcha below.

```
Plaza_District                   ← renamed from "Plaza_Greybox"; empty parent, groups all plaza geometry, rotation (0,180,0) on every child
├── Plaza                                              ← real model (pavement, checkered tiles, benches, gardens, paths, tree canopy/trunks, fountain water)
│   ├── Imported from Assets/_Project/Art/Environments/Models/Plaza.fbx
│   ├── Position (0, 0, 0) — the calibration anchor
│   └── 11 mesh sub-objects; ~36.5×36.5 footprint
├── Statue_Central                                     ← real model, central monument — Position (0, 0, 0)
├── CathedralStreet                                    ← real model, connects the plaza to the cathedral — Position (0, 0, -22)
├── Cathedral                                          ← real model, fronts the plaza
│   ├── Imported from Assets/_Project/Art/Environments/Models/Cathedral.fbx
│   ├── Position (0, 0, -43) — see the bounds-vs-transform gotcha below; one of two exports predating the manifest workflow
│   └── 12 mesh sub-objects (Cathedral_Body, Cathedral_Towers, Cathedral_Doors, etc.)
├── GovPalace                                          ← real model ("Moneda" in the source file) — Position (56, 0, -68)
│   └── 12 mesh sub-objects (Moneda_Body, Moneda_Balconies, Moneda_Apron, etc.); now sits on real ground since `Ground.fbx` landed
├── LPlaza                                             ← real model, secondary square west of the main plaza, ~1 unit lower
│   ├── Position (-44.85, -1, 13)
│   └── 11 mesh sub-objects: pavement, an east-facing arcade (colonnade + floor + roof + arch ring), a long L-shaped fountain, benches, chain posts, grass, red floor accent, statue pad
├── Statue_Grand                                       ← real model, centerpiece of LPlaza's arcade area — Position (-36, -0.88, 0)
├── Terrace                                            ← real model, staircase + retaining walls connecting the main plaza down to LPlaza — Position (-24.325, -1, 0)
│   └── 3 mesh sub-objects: stairs, north wall, south wall
├── Ground                                             ← real model, the two site terraces (206×170 m combined), everything else sits on this
│   ├── Imported from Assets/_Project/Art/Environments/Models/Ground.fbx
│   ├── Position (17, -1.01, -25)
│   └── 2 mesh sub-objects: Ground_Lower, Ground_Upper
├── Lamps                                              ← real model, 12 lamp posts, plus 12 child Point Lights (not part of the FBX — recreated as Unity Light components)
│   ├── Imported from Assets/_Project/Art/Environments/Models/Lamps.fbx
│   ├── Position (-12.985, -0.95, -2.235)
│   └── 12 mesh sub-objects (lamp posts) + 12 Light children — color (1, 0.82, 0.5), intensity 3 / range 9 as a first pass (source was 150W Blender wattage — not linearly convertible, may need visual tuning)
├── Building_East                                      ← real model, city block east of the plaza — Position (40, 0, 0)
├── Building_EastStreet                                ← real model, city block further along the east street — Position (52, 0, 28)
├── Building_North                                     ← real model, city block north of the cathedral — Position (0, 0, 40)
├── Building_NorthEast                                 ← real model — Position (26, 0, 40)
├── Building_SouthMid                                  ← real model — Position (26, 0, -40)
├── Building_SouthWest1                                ← real model, sits on the lower terrace — Position (-36, -1, -40)
├── Building_SouthWest2                                ← real model, lower terrace — Position (-62, -1, -40)
├── Building_WestBlock                                  ← real model, lower terrace — Position (-36, -1, 27)
└── Palace_West                                        ← real model, lower terrace, westernmost block — Position (-74, -1, 0)
    (each: body + _Roof + _Apron + Facade_ + Glass_ — 5 mesh sub-objects; concentric-shell facades with no single entrance direction)
```

The full site is now imported: 19 landmark groups (~115 mesh objects, matching 19 FBX files on disk) covering 10 non-block groups (plaza, statue, cathedral, government palace, LPlaza/statue/terrace, ground, lamps) and 9 surrounding city blocks (the eight `Building_*` plus `Palace_West`). Only the 12 lamp lights and the sun were handled separately (see below) since they aren't mesh data.

Every object above has root rotation `(0, 180, 0)` — see the orientation gotcha below for why.

**Materials**: all real models bring their own materials/colors authored in Blender — no placeholder greybox materials remain in the scene (`Mat_Greybox_Building`/`Street`/`Fountain` in `Assets/_Project/Art/Environments/` are unused leftovers from the retired primitive pass).

**Collision**: none of the 19 imported groups (115 mesh sub-objects) had a `Collider` from the FBX import — confirmed via `Physics.Raycast` returning no hit even at the documented player spawn point, meaning the player fell straight through the world once physics had a moment to accelerate gravity. Fixed by adding a non-convex `MeshCollider` (using the same `sharedMesh` as each object's `MeshFilter`) to every one of the 115 sub-objects; re-verified with raycasts at the plaza, LPlaza, in front of the cathedral, and in front of the Moneda — all now hit real geometry.

**Orientation gotcha — every import needs a 180° yaw, not just asymmetric ones.** First diagnosed as a bug specific to `LPlaza`/`Terrace` (a "whole-group mirror"), further investigation (vertex-centroid-vs-bbox-center measurements by the Blender-side session) showed it's universal: the standard Blender→Unity FBX axis conversion, combined with reading `footprint_center_xy` as Unity `(X,Z)` directly, negates both horizontal axes — which is mathematically a 180° yaw. It's invisible on symmetric geometry (`Plaza`, `Statue_Central`) and load-bearing on asymmetric geometry (`Cathedral`'s doors were confirmed facing away from the plaza before the fix — bounds at Z=-54 instead of the correct Z=-32). **Every future import needs `rotation: (0, 180, 0)` on its root, always** — see `[[hub-zone01-coordinate-mapping]]` for the full derivation.

**Bounds-vs-transform gotcha**: `Cathedral.fbx` and `GovPalace.fbx` were exported before the manifest workflow existed and carry a *second*, opposing offset baked into their mesh vertex data — reading `Transform.position` alone gives a confident but wrong answer for these two. Verify via `MeshRenderer.bounds.center` instead. Every later, manifest-driven import didn't have this problem.

**Light-placement gotcha**: a manifest's "position relative to [group] root" field is only valid for an *unrotated* parent — since every group root now carries the mandatory 180° yaw, that field needs the same rotation compensation (negate the two horizontal components) or it places things far from their intended spot. Safer to axis-relabel the manifest's **absolute** world-position field directly. See `[[hub-zone01-coordinate-mapping]]` Gotcha 2 for the full method (this is how the 12 lamp lights were ultimately placed correctly).

**Reference layout**: Cathedral fronts the plaza directly (connected by `CathedralStreet`), matching the real Plaza 10 de Noviembre in Potosí. Government palace ("Moneda") sits ~56 units east / ~68 units south of the plaza — consistent with the real Casa de la Moneda being several blocks away — but is now within `Ground.fbx`'s modeled site, no longer floating outside it. `LPlaza` connects to the main plaza via `Terrace`'s staircase, ~1 unit lower in elevation.

Player spawn (`Capsule`, at `(0,1,-14)`) sits just inside the real plaza's south edge, facing the statue/fountain — unchanged since the blockout era, still reads correctly against the real geometry.

`Zone_Entrance` (see `interaction-system.md`) was left at its original position `(3, 0, 3)` and now sits inside the real plaza, close to the statue/fountain — expected to be relocated when real landmark zones are seeded (roadmap next step).

**Known cleanup item**: the old primitive placeholders (`Cathedral_Nave`, `Cathedral_Tower`, `GovPalace` cube) are disabled in the Hierarchy but not deleted — the MCP tooling couldn't resolve them for deletion once inactive. Safe to delete manually in the Editor.

**Known limitations, handed over from the Blender-side session**:
- **No entrance-direction data for generic buildings**: `facing` was only ever meaningful for `Cathedral` and `GovPalace` (both north-facing). Every `Building_*`'s `Facade_` mesh is a concentric shell wrapping the whole body, not a front face, so there's no entrance direction to encode. `LPlaza`'s "east" describes which side the arcade is on, not a facing.
- **The 180° yaw is a workaround, not a fix**: every export still carries the underlying axis mismatch, and every future consumer has to know to compensate for it. A one-time re-export with the yaw baked directly into the FBX data would remove this permanently, at the cost of repositioning everything already placed. Not worth doing now that the whole site is in, but worth keeping in mind if this pipeline gets reused for another zone.

---

## Design Decisions
- **Bounds-based verification over trusting exporter fixes**: a re-export ("v2") that used a genuinely different transform strategy produced byte-identical wrong bounds to the original — which turned out to be expected (both strategies produce the same world geometry by construction), not evidence the bug was elsewhere. Lesson: identical bounds across two export strategies proves the strategies are equivalent, not that either is correct — always re-verify against the manifest's actual target coordinates.
- **Trust but verify a peer session's diagnosis, don't just apply it blindly**: when the Blender-side session reported the 180°-yaw finding applied universally (not just to asymmetric groups), it was checked independently in Unity (cathedral door position) before being applied to 5 already-placed objects — confirmed correct, but worth the extra step given how much rework a wrong diagnosis would have caused a third time.
- **Retired all primitive plaza blockout in favor of real imports**: once `Plaza.fbx` arrived with real pavement/fountain/gardens/benches, the primitive `Ground_Plaza`, fountain, and street stubs were fully redundant and were deleted rather than kept as a fallback.
- **Coordinate mapping anchored on a real reference object, not a guess**: earlier passes anchored on hand-placed greybox positions, both inconsistent with the true Blender layout. The current mapping anchors on `Plaza_Pavement`'s real Blender coordinates via a manifest file the Blender-side export reports — see `[[hub-zone01-coordinate-mapping]]`.
- **Manifest-driven exports going forward**: each Blender export batch includes a `layout_batchN.json` alongside the FBX files, giving each object's real Blender-space footprint center and base directly.
- **Lamp lights recreated as Unity Light components, not imported geometry**: point lights aren't mesh data, so the Blender-side session reported their transforms/color/energy in the manifest instead, and they were built directly as `Light` components parented under `Lamps`.
- **Player spawn kept at (0,1,-14)**: still reads correctly now that it sits just inside the real plaza's south edge, facing the statue — no change needed from the original blockout-era placement.
