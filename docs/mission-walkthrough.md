# Mission Walkthrough

Full step-by-step guide to the first mission, start to finish — **spoilers intentional**. This is the practical companion to `mission-design.md`: that doc has the story and the build order, this one turns it into a concrete sequence of player actions, so we always know exactly what's left to wire up and can playtest against a real checklist instead of just prose.

Each step is marked:
- ✅ **Built** — playable today
- 🔜 **Planned** — designed here, not implemented yet
- 🧩 **Needs** — which mission build-order item (from `mission-design.md`) has to land first

---

## Currently Playable (as of this writing)
The real Act 1 loop is playable start to finish. At the start, only the **Client** is visible (position (0,1,-11)) — everything else is hidden until the case is accepted.

Talk to the **Client**: he explains his fiancée is suspected of infidelity and asks for proof. Choosing "Accept the case" sets `act1_case_accepted` and immediately activates the rest of Act 1's content — no scene reload needed.

Once accepted: **`NPC_Informant`** ("Renata," at (-8,1,-10)) gives real leads pointing toward the fiancée and her lover (grants `lead_informant_tip`, optional flavor). **`Watcher_Fiancee`** (8,1,-10) and **`Watcher_Lover`** (16,1,-10) each have a facing vision cone and an Unaware/Suspicious/Alert state machine; standing unseen in their respective `SneakObservationPoint` (crouch with Left Ctrl to slow detection) grants `evidence_fiancee_seen` / `evidence_lover_seen`. **`CombatEncounterTrigger_Thief`** (-8,1,-15) starts a turn-based fight against a placeholder "Thief" in a separate `Battle` scene — incidental danger, grants no evidence.

Once **both** evidence clues are gathered, returning to the Client offers "Report your findings" — selecting it grants `act1_complete` and the Client's dialogue changes to a closing line on any future visit.

---

## Act 1 — The Case

1. ✅ **Receive the case.** `NPC_Client` in the plaza (0,1,-11) — the only thing active at game start. Talking to him explains the job; accepting sets `act1_case_accepted` and live-activates the rest of Act 1's content, no reload.

2. ✅ **Gather leads.** `NPC_Informant` ("Renata") gives real leads once `act1_case_accepted` is set (a brush-off before), granting the optional flavor clue `lead_informant_tip`.

3. ✅ **Sneak for proof.** `Watcher_Fiancee`/`SneakObservationPoint_Fiancee` and `Watcher_Lover`/`SneakObservationPoint_Lover` — both gated behind `act1_case_accepted`, each granting its own clue (`evidence_fiancee_seen`/`evidence_lover_seen`).

4. ✅ **Survive the streets.** `CombatEncounterTrigger_Thief`, also gated behind `act1_case_accepted`. Grants no evidence — incidental danger, parallel to the sneak clues, not chained to them.

5. ✅ **Report back.** Once both `evidence_fiancee_seen` and `evidence_lover_seen` are present, the Client's "Report your findings" option appears; selecting it grants `act1_complete`, closing Act 1.

---

## Act 2 — The Reveal

6. 🔜 **Receive the invitation.** After Act 1 closes, the Client (or an in-world notice/NPC) invites the detective to the couple's engagement ceremony.
   🧩 Needs: item 5 (event sequencer), gated on Act 1's completion flag.

7. 🔜 **Attend the ceremony.** A new space hosts this — minimal blockout is fine (structural, not art, per `mission-design.md`'s scope boundaries). The player arrives and waits.
   🧩 Needs: item 5, plus a small placeholder scene/area to stage it in.

8. 🔜 **The reveal.** Neither the fiancée nor the lover appears. After a scripted wait, a banquet dish is brought out; revealing its contents shows the two lovers' decapitated heads. Player control is likely taken away for this beat, then returned — the shock beat that pivots into Act 3.
   🧩 Needs: item 5 (the event sequencer's actual reason for existing).

---

## Act 3 — The Hunt

9. 🔜 **New suspicion.** Post-reveal dialogue (with returning NPCs, or new leads) starts pointing at the old Client as the real suspect.
   🧩 Needs: item 6 (Act 3 wiring), reusing the dialogue/clue systems again — no new mechanics expected here beyond content.

10. 🔜 **Track him down.** The player follows the trail to the Client's final location, discovering along the way that he's a serial murderer.
    🧩 Needs: item 6, content + scene wiring (may reuse existing `Hub_Zone01` spaces or need a small new one — TBD when this is built).

11. ✅ **The final confrontation (mechanic done, content pending).** The boss fight reuses the same combat system as the minor encounters (item 3, done) — a boss is just a new `EnemyDefinition` asset with higher stats, no new code.
    🧩 Needs: item 6, to author the Client's boss stats and wire the encounter trigger at the confrontation.

12. 🔜 **Resolution.** Winning the fight ends the mission. No epilogue/ending screen designed yet — worth a small pass once the fight itself works (main menu/UI work is separate, see `roadmap.md`).
    🧩 Needs: item 6.

---

## Cross-Reference: Story Beats → Build Order
| Mission build-order item (`mission-design.md`) | Unlocks story steps |
|---|---|
| 1. Dialogue + clue tracking foundation — **done** | Steps 1, 2, 5, 6, 9, 10 (all dialogue-driven) |
| 2. Sneak/observation mechanic — **done** | Step 3 |
| 3. Combat system core — **done** | Steps 4, 11 |
| 4. Act 1 wiring — **done** | Steps 1–5, assembled into one playable act — see `features/mission-state.md` |
| 5. Event sequencer + Act 2 reveal | Steps 6–8 |
| 6. Act 3 — the hunt and final battle | Steps 9–12 |

Update this doc's ✅/🔜 markers as each build-order item lands, so it stays an accurate playthrough guide rather than a stale plan.
