using UnityEngine;
using System.Collections.Generic;

public class PedestrianSpawner : MonoBehaviour
{
    public GameObject[] pedestrianPrefab;
    public float spawnRadius;
    public float destroyDistance;
    public int maxPedestrians;
    public Transform playerTransform;
    public Transform nodesParent;

    private List<GameObject> pedestrians = new List<GameObject>();
    private List<Transform> spawnNodes = new List<Transform>();

    private void Start()
    {
        // Cache the spawn nodes
        foreach (Transform child in nodesParent)
            spawnNodes.Add(child);

        // Spawn initial pedestrians
        SpawnPedestrians(maxPedestrians);
    }

    private void Update()
    {
        // Destroy pedestrians that are too far away
        for (int i = pedestrians.Count - 1; i >= 0; i--)
        {
            if (pedestrians[i] == null || Vector3.Distance(pedestrians[i].transform.position, playerTransform.position) > destroyDistance)
            {
                Destroy(pedestrians[i]);
                pedestrians.RemoveAt(i);
            }
        }

        // Spawn new pedestrians if needed
        if (pedestrians.Count < maxPedestrians)
            SpawnPedestrians(maxPedestrians - pedestrians.Count);
    }

    private void SpawnPedestrians(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Transform spawnNode = GetNearbySpawnNode();
            if (spawnNode != null)
            {
                GameObject newPedestrian = Instantiate(pedestrianPrefab[Random.Range(0,pedestrianPrefab.Length)], spawnNode.position, Quaternion.identity);
                pedestrians.Add(newPedestrian);
            }
        }
    }

    private Transform GetNearbySpawnNode()
    {
        List<Transform> nearbyNodes = new List<Transform>();
        foreach (Transform node in spawnNodes)
        {
            if (Vector3.Distance(node.position, playerTransform.position) <= spawnRadius)
                nearbyNodes.Add(node);
        }

        return nearbyNodes.Count > 0 ? nearbyNodes[Random.Range(0, nearbyNodes.Count)] : null;
    }
}