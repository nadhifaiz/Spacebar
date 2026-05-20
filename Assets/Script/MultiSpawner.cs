using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MultiSpawner - Spawn semua prefab sekali saat game mulai.
/// Tiap prefab mendapat spawn point acak yang unik (tidak dobel).
/// </summary>
public class MultiSpawner : MonoBehaviour
{
    [Header("Prefab yang akan di-spawn")]
    public List<GameObject> prefabs = new List<GameObject>();

    [Header("Titik-titik spawn yang tersedia")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Parent untuk objek hasil spawn (opsional)")]
    public Transform spawnParent;

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        if (prefabs.Count == 0)
        {
            Debug.LogWarning("[MultiSpawner] Prefab list kosong!", this);
            return;
        }

        if (spawnPoints.Count < prefabs.Count)
        {
            Debug.LogWarning($"[MultiSpawner] Spawn point ({spawnPoints.Count}) kurang dari jumlah prefab ({prefabs.Count})!", this);
            return;
        }

        // Acak urutan spawn points
        List<Transform> shuffled = new List<Transform>(spawnPoints);
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            Transform temp = shuffled[i];
            shuffled[i] = shuffled[rand];
            shuffled[rand] = temp;
        }

        // Spawn tiap prefab di spawn point unik
        for (int i = 0; i < prefabs.Count; i++)
        {
            if (prefabs[i] == null) continue;

            Instantiate(prefabs[i], shuffled[i].position, shuffled[i].rotation, spawnParent);
            Debug.Log($"[MultiSpawner] '{prefabs[i].name}' spawned di '{shuffled[i].name}'");
        }
    }

    private void OnDrawGizmos()
    {
        if (spawnPoints == null) return;

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (spawnPoints[i] == null) continue;

            Gizmos.color = Color.HSVToRGB((float)i / Mathf.Max(1, spawnPoints.Count), 0.85f, 1f);

            Vector3 pos = spawnPoints[i].position;
            Gizmos.DrawWireSphere(pos, 0.35f);
            Gizmos.DrawLine(pos, pos + spawnPoints[i].forward * 0.7f);

#if UNITY_EDITOR
            UnityEditor.Handles.Label(pos + Vector3.up * 0.5f, $"Spawn {i + 1}");
#endif
        }
    }
}