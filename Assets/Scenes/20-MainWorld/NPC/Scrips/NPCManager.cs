using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCManager : MonoBehaviour
{
    [SerializeField] private ObjectPool pool;
    [SerializeField] private Transform[] POI;
    [SerializeField] private float timeBetweenNPCSpawn = 5f;
    

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnAgent), 2, timeBetweenNPCSpawn);
    }

    /// <summary>
    /// is called automaticly. it is used to spawn in a new npc agent.
    /// </summary>
    private void SpawnAgent()
    {
        GameObject agent = pool.GetObject();
        if(!NavMesh.SamplePosition(POI[Random.Range(0, POI.Length)].position, out NavMeshHit hit, 2, NavMesh.AllAreas)) return;
        agent.GetComponent<NavMeshAgent>().Warp(hit.position);
        agent.GetComponent<NavMeshAgent>().enabled = true;
        agent.GetComponent<NPCController>().Setup(POI);
        agent.SetActive(true);
    }
}
