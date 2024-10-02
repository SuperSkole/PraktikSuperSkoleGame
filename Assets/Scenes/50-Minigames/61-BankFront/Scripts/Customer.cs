using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// Class to handle movement and handling of customers in bank front minigame
/// </summary>
public class Customer : MonoBehaviour
{
    public List<CustomerNavPoint>navPoints;
    public int currentNavPoint;
    public BankManager bankManager;
    public CustomerPool customerPool;
    private bool givenMoney = false;
    private bool leaving = false;
    private float speed = 1.25f;
    [SerializeField] private List<GameObject> customerSkins;
    private bool skinActivated = false;

    [SerializeField] private Animator animator;


    /// <summary>
    /// Handles movement and telling the bankmanager to start a game
    /// </summary>
    void Update()
    {
        if(!skinActivated)
        {
            GameObject currentCustomer = customerSkins[Random.Range(0, customerSkins.Count)];
            currentCustomer.SetActive(true);
            skinActivated = true;
        }
        //handles when the customor has reached a navpoint
        if(navPoints[currentNavPoint].transform.position == transform.position && navPoints.Count - 1> currentNavPoint)
        {
            if(animator.GetFloat("Speed") != 0)
            {
                animator.SetFloat("Speed", 0);
            }
            
            // removes the customer from the game when it reaches the spawnpoint
            if(leaving)
            {
                leaving = false;
                customerPool.returnToPool(this);
            }
            //Tells the customer to begin moving towards the next point if it is empty
            else if(navPoints[currentNavPoint + 1].customer == null)
            {
                transform.LookAt(navPoints[currentNavPoint + 1].transform);
                navPoints[currentNavPoint].customer = null;
                currentNavPoint++;
                animator.SetFloat("Speed", speed);
                navPoints[currentNavPoint].customer = gameObject;
            }       
        }
        //Moves the customer towards the next point if it has not been reached
        if(navPoints[currentNavPoint].transform.position != transform.position)
        {   
            transform.position = Vector3.MoveTowards(transform.position, navPoints[currentNavPoint].transform.position, speed * Time.deltaTime);
        }
        //Starts a game if the last point has been reached
        else if(navPoints[navPoints.Count - 1].transform.position == transform.position && !givenMoney)
        {
            if(animator.GetFloat("Speed") != 0)
            {
                animator.SetFloat("Speed", 0);
            }
            bankManager.HandOverMoney(this);
            givenMoney = true;
        }
    }
    /// <summary>
    /// prepares to move towards the spawnpoint and resets the givenMoney variable
    /// </summary>
    public void PrepareToLeaveBank()
    {
        transform.LookAt(navPoints[0].transform);
        animator.SetFloat("Speed", speed);
        leaving = true;
        givenMoney = false;
        navPoints[currentNavPoint].customer = null;
        currentNavPoint = 0;
    }
}