using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<EnemyDefinition> fallbackEncounter;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject placeholderPrefab;

    public List<CombatantState> SpawnEnemies()
    {
        List<EnemyDefinition> defs = PendingEncounter.Enemies ?? fallbackEncounter;
        List<CombatantState> states = new();
        for (int i = 0; i < defs.Count && i < spawnPoints.Length; i++)
            states.Add(SpawnOne(defs[i], spawnPoints[i]));
        PendingEncounter.Clear();
        return states;
    }

    CombatantState SpawnOne(EnemyDefinition def, Transform point)
    {
        GameObject go = Instantiate(placeholderPrefab, point.position, point.rotation);
        go.name = def.DisplayName;
        TextMeshPro label = go.GetComponentInChildren<TextMeshPro>();
        if (label != null) label.text = def.DisplayName;
        return def.CreateState();
    }
}
