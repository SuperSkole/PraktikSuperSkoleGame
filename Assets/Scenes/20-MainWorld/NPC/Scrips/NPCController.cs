using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    private Transform[] pointsOfIntrest;
    private Vector3 destination = Vector3.zero;

    /// <summary>
    /// used to setup the NPC.
    /// </summary>
    /// <param name="POI">POI is short for points of intrest. thies are the spawn locations and destioatons of the NPC's</param>
    public void Setup(Transform[] POI)
    {
        if(POI == null) return;
        pointsOfIntrest = POI;
        gameObject.SetActive(true);
        FindNewDest();
    }

    /// <summary>
    /// used to give the NPC a new destination.
    /// </summary>
    private void FindNewDest()
    {
        do
        {
            destination = pointsOfIntrest[Random.Range(0, pointsOfIntrest.Length)].position;
        }
        while (Vector3.Distance(destination, transform.position) <= 1);
        agent.SetDestination(destination);
    }

    private void Update()
    {
        if (destination == Vector3.zero) return;

        if(Vector3.Distance(destination,transform.position) <= 1)
        {
            Reset();
        }
    }

    private void Reset()
    {
        agent.isStopped = true;
        gameObject.SetActive(false);
        destination = Vector3.zero;
    }
}
