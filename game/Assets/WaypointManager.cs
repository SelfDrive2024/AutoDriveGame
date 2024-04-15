using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public Transform player;
    public Transform target;
    public List<Transform> waypoints;
    public List<Transform> pathwaypoints;

    void Start()
    {
        waypoints = new List<Transform>();
        foreach (Transform child in transform)
        {
            waypoints.Add(child);
        }
        pathwaypoints = new List<Transform>();
        GetAllWaypoints();
        CalculatePath();
        Debug.Log("Pathwaypoints count: " + pathwaypoints.Count);
        Debug.Log("Pathwaypoints: " + string.Join(", ", pathwaypoints));
    }

    void GetAllWaypoints()
    {
        waypoints.Clear();
        foreach (Transform child in transform)
        {
            waypoints.Add(child);
        }
    }

    void CalculatePath()
    {
        pathwaypoints.Clear();

        if (waypoints.Count == 0)
        {
            Debug.LogError("No waypoints available.");
            return;
        }

        Vector3 playerPosition = player.position;
        Vector3 targetPosition = target.position;

        Transform currentWaypoint = GetClosestWaypoint(playerPosition);

        while (currentWaypoint != null && currentWaypoint.position != targetPosition)
        {
            pathwaypoints.Add(currentWaypoint);
            waypoints.Remove(currentWaypoint);

            currentWaypoint = GetNextWaypoint(currentWaypoint, targetPosition);
        }

        pathwaypoints.Add(target); // Add the target as the final pathwaypoint

        // Draw debug lines between each waypoint in the pathwaypoints list
        for (int i = 0; i < pathwaypoints.Count - 1; i++)
        {
            Debug.DrawLine(pathwaypoints[i].position, pathwaypoints[i + 1].position, Color.red, 10f);
        }
    }


    Transform GetClosestWaypoint(Vector3 position)
    {
        float minDist = Mathf.Infinity;
        Transform closest = null;

        foreach (Transform waypoint in waypoints)
        {
            float dist = Vector3.Distance(position, waypoint.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = waypoint;
            }
        }

        return closest;
    }

    Transform GetNextWaypoint(Transform currentWaypoint, Vector3 targetPosition)
    {
        Transform nextWaypoint = null;
        float minDist = Mathf.Infinity;

        foreach (Transform waypoint in waypoints)
        {
            if (waypoint == currentWaypoint) continue;

            float dist = Vector3.Distance(currentWaypoint.position, waypoint.position);
            if (dist < minDist)
            {
                Vector3 waypointToTarget = targetPosition - waypoint.position;
                Vector3 currentToWaypoint = waypoint.position - currentWaypoint.position;

                if (Vector3.Dot(waypointToTarget.normalized, currentToWaypoint.normalized) > 0)
                {
                    minDist = dist;
                    nextWaypoint = waypoint;
                }
            }
        }

        return nextWaypoint;
    }
}
