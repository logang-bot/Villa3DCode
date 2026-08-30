# Mission Walkthrough

Full step-by-step guide to the first mission, start to finish — **spoilers intentional**. This is the practical companion to `mission-design.md`: that doc has the story and the build order, this one turns it into a concrete sequence of player actions, so we always know exactly what's left to wire up and can playtest against a real checklist instead of just prose.

Each step is marked:
- ✅ **Built** — playable today
- 🔜 **Planned** — designed here, not implemented yet
- 🧩 **Needs** — which mission build-order item (from `mission-design.md`) has to land first

---

## Currently Playable (as of this writing)
Walk up to **`NPC_Witness`** in `Hub_Zone01` (open plaza pavement, southwest of spawn) and press E. They mention seeing a stranger near the cathedral; asking follow-up questions grants the clue `witness_saw_stranger`, visible in the debug list in the top-left corner. Talking to them again shows a different opening line, proving the clue was remembered. Reloading the scene (via the test zone) doesn't lose the clue.

This is a mechanical proof-of-concept, not yet reskinned into the actual mission content below — `NPC_Witness` isn't tied to any of the real characters or story clues yet.

---

## Act 1 — The Case

1. 🔜 **Receive the case.** An NPC ("the Client") is placed in the plaza. Talking to him opens a dialogue explaining the job: he suspects his fiancée of infidelity and wants proof. Accepting starts Act 1.
   🧩 Needs: item 4 (Act 1 wiring) on top of item 1 (dialogue foundation, done).

2. 🔜 **Gather leads.** Talk to NPCs around the plaza (witnesses, staff, passersby) for clues pointing toward the fiancée's and the suspected lover's whereabouts/routine. Reuses the dialogue + clue system exactly as `NPC_Witness` demonstrates today.
   🧩 Needs: item 4, content only (more NPCs authored against the existing system) — no new mechanics.

3. 🔜 **Sneak for proof.** Locate the fiancée and the lover in the world and observe them without being noticed to gather direct evidence — this is the one genuinely new mechanic this act needs.
   🧩 Needs: item 2 (sneak/observation mechanic) — not started.

4. 🔜 **Survive the streets.** While moving through the city between leads, the player is occasionally ambushed by thieves/robbers, triggering a small turn-based fight.
   🧩 Needs: item 3 (combat system core) — not started.

5. 🔜 **Report back.** Once enough evidence is collected (exact threshold TBD when this is built — e.g. 3 pieces), return to the Client and hand it over. This closes Act 1 and should set a flag (e.g. `has_clue("act1_complete")`) gating Act 2.
   🧩 Needs: item 4, using the clue system already in place to gate the hand-off dialogue.

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

11. 🔜 **The final confrontation.** Finding the Client triggers the boss fight — same combat system as the minor encounters (item 3), but with unique stats/stakes befitting a final boss.
    🧩 Needs: item 6 + item 3.

12. 🔜 **Resolution.** Winning the fight ends the mission. No epilogue/ending screen designed yet — worth a small pass once the fight itself works (main menu/UI work is separate, see `roadmap.md`).
    🧩 Needs: item 6.

---

## Cross-Reference: Story Beats → Build Order
| Mission build-order item (`mission-design.md`) | Unlocks story steps |
|---|---|
| 1. Dialogue + clue tracking foundation — **done** | Steps 1, 2, 5, 6, 9, 10 (all dialogue-driven) |
| 2. Sneak/observation mechanic | Step 3 |
| 3. Combat system core | Steps 4, 11 |
| 4. Act 1 wiring | Steps 1–5, assembled into one playable act |
| 5. Event sequencer + Act 2 reveal | Steps 6–8 |
| 6. Act 3 — the hunt and final battle | Steps 9–12 |

Update this doc's ✅/🔜 markers as each build-order item lands, so it stays an accurate playthrough guide rather than a stale plan.
