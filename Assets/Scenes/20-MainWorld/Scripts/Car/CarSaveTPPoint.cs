using System.Collections.Generic;
using UnityEngine;

public class CarSaveTPPoint : MonoBehaviour
{

    public Dictionary<int, RayPoint> RayPointDic = new();
    [SerializeField] private List<GameObject> rays = new();
    public LayerMask terrainLayer;
    private Vector3 SavedPos = Vector3.zero;
   [SerializeField] PrometeoCarController carController;

    private void Start()
    {
        //FrontWheels
        RayPointDic.Add(0, new RayPoint(rays[0].transform, true, Vector3.zero));
        RayPointDic.Add(1, new RayPoint(rays[1].transform, true, Vector3.zero));
        //BackWheels
        RayPointDic.Add(2, new RayPoint(rays[2].transform, true, Vector3.zero));
        RayPointDic.Add(3, new RayPoint(rays[3].transform, true, Vector3.zero));
    }

    void FixedUpdate()
    {
        bool allWheelsTouchingGround = true;
        //timer += Time.deltaTime;
        for (int i = 0; i < RayPointDic.Count; i++)
        {
            RayPointDic[i].rayPos = rays[i].transform;
            IsWheelTouchingGround(i);

            if (!RayPointDic[i].isWheelTouching)
            {
                allWheelsTouchingGround = false;
            }
        }
        if (allWheelsTouchingGround)
        {
            SaveCurrentSafePosition();
        }
    }
    private void SaveCurrentSafePosition()
    {
        // This method saves the current position of all wheels as the last safe position
        Vector3 sum = Vector3.zero;

        foreach (var item in RayPointDic)
        {
            sum += item.Value.savedPos;
        }

        // Average position of the four wheels, to act as the safe position
        //SavedPos = sum / RayPointDic.Count;
        if (carController.isReversing)
        {
            SavedPos = (RayPointDic[0].savedPos + RayPointDic[1].savedPos) / 2;
        }
        else
        {
            SavedPos = (RayPointDic[2].savedPos + RayPointDic[3].savedPos) / 2;
        }
    }

    private void IsWheelTouchingGround(int i)
    {
        Vector3 fwd = RayPointDic[i].rayPos.TransformDirection(Vector3.forward);

        RaycastHit hit;

        if (Physics.Raycast(RayPointDic[i].rayPos.position, fwd, out hit, 1, terrainLayer))
        {
            RayPointDic[i].savedPos = hit.point;
            RayPointDic[i].isWheelTouching = true;
        }
        else
        {
            RayPointDic[i].isWheelTouching = false;
        }
    }
    public Vector3 ReturnLastSafePos()
    {

        // Return the last known safe position
        return SavedPos + Vector3.up;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Rigidbody carRigidbody = GetComponent<Rigidbody>();
            if (carRigidbody != null)
            {
                carRigidbody.velocity = Vector3.zero;
                carRigidbody.angularVelocity = Vector3.zero;
            }
            transform.position = ReturnLastSafePos();
        }
    }
}

public class RayPoint
{
    public Transform rayPos;
    public bool isWheelTouching;
    public Vector3 savedPos;

    public RayPoint(Transform rayPos, bool isWheelTouching, Vector3 savedPos)
    {
        this.rayPos = rayPos;
        this.isWheelTouching = isWheelTouching;
        this.savedPos = savedPos;
    }
}