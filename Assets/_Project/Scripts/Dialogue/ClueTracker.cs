using System;
using System.Collections.Generic;
using UnityEngine;

public class ClueTracker : MonoBehaviour
{
    public static ClueTracker Instance { get; private set; }
    public static event Action<string> ClueAdded;

    readonly HashSet<string> clueIds = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddClue(string id)
    {
        if (!clueIds.Add(id)) return;
        ClueAdded?.Invoke(id);
    }

    public bool HasClue(string id) => clueIds.Contains(id);
}
