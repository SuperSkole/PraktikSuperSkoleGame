using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchingPoint : MonoBehaviour
{
    public RacingGameCore racingGameCore; // Reference to the RacingGameCore script

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ActiveCar"))
        {
            // Notify the RacingGameCore about which branch was triggered
            racingGameCore.BranchTriggered(gameObject.name);
        }
    }
}
