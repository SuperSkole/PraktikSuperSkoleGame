using UnityEngine;
using UnityEngine.Events;

public class Interactions : MonoBehaviour
{
    [SerializeField] private GameObject interactionZoneObj;


    //Where should the interaction zone be placed uses DrawGizmo to see where it will be placed
    [Header("Interaction Zone")]
    [SerializeField] private float interactionZoneRadius;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;
    [SerializeField] private float zOffset;

    public UnityEvent action { get; set; }
    public bool inZone { get; set; } = false;
    private void Start()
    {
        interactionZoneObj.transform.position = new Vector3(transform.position.x + xOffset,
            transform.position.y + yOffset,
            transform.position.z + zOffset);
        interactionZoneObj.transform.localScale = new Vector3(interactionZoneRadius * 4,
            interactionZoneRadius * 4,
            interactionZoneRadius * 4);
    }

    private void Update()
    {
        //if (inZone)
        //{
        //    if (Input.GetKeyDown(KeyCode.F)) { action.Invoke(); }
        //}
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z + zOffset), interactionZoneRadius);
    }

}
