using System;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleStateManager : MonoBehaviour
{
    public static PuzzleStateManager Instance { get; private set; }

    private Dictionary<string, bool> flagRegistry = new Dictionary<string, bool>();

    // Event yang akan di-subscribe oleh CausalityReactor punya Dawd
    public static event Action<string, bool> OnFlagChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetFlag(string flagId, bool value)
    {
        if (!flagRegistry.ContainsKey(flagId) || flagRegistry[flagId] != value)
        {
            flagRegistry[flagId] = value;
            OnFlagChanged?.Invoke(flagId, value);
        }
    }

    public bool GetFlag(string flagId)
    {
        return flagRegistry.ContainsKey(flagId) && flagRegistry[flagId];
    }

    public void ResetAll()
    {
        flagRegistry.Clear();
    }
}