using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to handle the customer pool in the bank front game
/// </summary>
public class CustomerPool : MonoBehaviour
{
    [SerializeField] private List<CustomerNavPoint> customerNavPoints;
    private List<GameObject> customers;
    [SerializeField]private Queue<Customer> customersToSpawn;
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private BankManager bankManager;
    /// <summary>
    /// Creates the pool of customers and sets up their various variables
    /// </summary>
    void Start()
    {
        customers = new List<GameObject>();
        customersToSpawn = new Queue<Customer>();
        for(int i = 0; i < 10; i++)
        {
            customers.Add(Instantiate(customerPrefab));
            customers[i].SetActive(false);
            Customer customer = customers[i].GetComponent<Customer>();
            customer.bankManager = bankManager;
            customer.customerPool = this;
            customer.navPoints = customerNavPoints;
            customersToSpawn.Enqueue(customer);
        }
    }

    /// <summary>
    /// Spawns a customer if the spawnpoint is empty
    /// </summary>
    void Update()
    {
        //spawns a customer from the pool
        if(customerNavPoints[0].customer == null && customersToSpawn.Count > 0)
        {
            Customer customer = customersToSpawn.Dequeue();
            SpawnCustomer(customer);
            
        }
        //Creates a new customer if the queue is empty
        else if(customerNavPoints[0].customer == null)
        {
            Customer customer = Instantiate(customerPrefab).GetComponent<Customer>();
            customers.Add(customer.gameObject);
            customer.bankManager = bankManager;
            customer.customerPool = this;
            customer.navPoints = customerNavPoints;
            SpawnCustomer(customer);

        }
    }

    /// <summary>
    /// Spawns a customer
    /// </summary>
    /// <param name="customer">the customer to be spawned</param>
    private void SpawnCustomer(Customer customer)
    {
        customer.gameObject.transform.position = customerNavPoints[0].transform.position;
        customerNavPoints[0].customer = customer.gameObject;
        customer.gameObject.SetActive(true);
        customer.currentNavPoint = 0;
    }

    /// <summary>
    /// Returns a customer to the pool
    /// </summary>
    /// <param name="customer">The customer to be readded to the pool</param>
    public void returnToPool(Customer customer)
    {
        customer.gameObject.SetActive(false);
        customersToSpawn.Enqueue(customer);
    }
}
