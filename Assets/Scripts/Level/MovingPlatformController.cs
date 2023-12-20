using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [SerializeField] private WaypointPathController path;
    [SerializeField] private float speed;

    private int targetWaypointIndex;
    private Transform previousWaypoint;
    private Transform targetWaypoint;

    private float timeToReachTargetWaypoint;
    private float elapsedTime;

    private void Start()
    {
        this.targetWaypointIndex = 0;
        this.TargetNextWaypoint();
        this.transform.position = this.previousWaypoint.position;
    }

    private void FixedUpdate()
    {
        this.elapsedTime += Time.fixedDeltaTime;
        float elapsedPercentage = this.elapsedTime / this.timeToReachTargetWaypoint;
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);

        this.transform.position = Vector3.Lerp(this.previousWaypoint.position, this.targetWaypoint.position, elapsedPercentage);
        this.transform.rotation = Quaternion.Lerp(this.previousWaypoint.rotation, this.targetWaypoint.rotation, elapsedPercentage);
        if (elapsedPercentage >= 1)
            this.TargetNextWaypoint();
    }

    private void TargetNextWaypoint()
    {
        this.previousWaypoint = path.GetWaypoint(this.targetWaypointIndex);
        this.targetWaypointIndex = path.GetNextWaypointIndex(this.targetWaypointIndex);
        this.targetWaypoint = path.GetWaypoint(this.targetWaypointIndex);

        this.elapsedTime = 0;
        float distance = Vector3.Distance(this.previousWaypoint.position, this.targetWaypoint.position);
        this.timeToReachTargetWaypoint = distance / this.speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }

}
