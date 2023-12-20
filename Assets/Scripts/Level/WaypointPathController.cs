using UnityEngine;

public class WaypointPathController : MonoBehaviour
{
    public Transform GetWaypoint(int waypointIndex)
    {
        if (waypointIndex < 0 || waypointIndex >= transform.childCount)
            return null;
        return transform.GetChild(waypointIndex);
    }

    public int GetNextWaypointIndex(int currentWaypointIndex)
    {
        if (currentWaypointIndex < 0 || currentWaypointIndex >= transform.childCount)
            return -1;
        return (currentWaypointIndex + 1) % transform.childCount;
    }

}
