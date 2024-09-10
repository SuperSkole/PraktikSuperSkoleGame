using System.Collections.Generic;
using UnityEngine;

public class CarSaveTPPoint : MonoBehaviour
{

    public Dictionary<int, RayPoint> RayPointDic = new();
    [SerializeField] private List<GameObject> rays = new();
    public LayerMask terrainLayer;
    float timer;

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
        timer += Time.deltaTime;
        for (int i = 0; i < RayPointDic.Count; i++)
        {
            IsWheelTouchingGround(i);
        }
        if (timer > 0.1f)
        {
            timer = 0;
        }
    }


    private void IsWheelTouchingGround(int i)
    {
        RayPointDic[i].rayPos = rays[i].transform;
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
        try
        {
            Vector3 sum = Vector3.zero;
            foreach (var item in RayPointDic)
            {
                sum += item.Value.savedPos;
            }
            Vector3 tmp = sum / RayPointDic.Count;
            tmp.y += 1;
            return tmp;
        }
        catch
        {
            return Vector3.zero;
        }
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