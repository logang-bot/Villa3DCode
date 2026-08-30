using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatEncounterTrigger : MonoBehaviour
{
    [SerializeField] List<EnemyDefinition> enemies;
    [SerializeField] string battleSceneName = "Battle";

    public void StartEncounter()
    {
        PendingEncounter.Set(enemies);
        SceneManager.LoadScene(battleSceneName);
    }
}
