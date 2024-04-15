using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiCarController : MonoBehaviour
{
    public GameObject target;
    public NavMeshAgent agent;
    private float raycastDistance = 30f;
    private bool isHittingRedLight = false;
    private float resumeDelay = 2f;

    public void Update()
    {
        agent.SetDestination(target.transform.position);
        CheckForObstacles();
    }

    void CheckForObstacles()
    {
        Vector3 raycastOrigin = new Vector3(transform.position.x, 1f, transform.position.z);
        Vector3 direction = agent.desiredVelocity.normalized;
        Ray ray = new Ray(raycastOrigin, direction);
        RaycastHit hit;

        // Debug line to visualize the raycast
        Debug.DrawRay(raycastOrigin, direction * raycastDistance, Color.green);

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("redlight"))
            {
                Debug.Log("Red Light Detected");
                if (!isHittingRedLight)
                {
                    isHittingRedLight = true;
                    agent.isStopped = true;
                }
            }
        }
        else
        {
            if (isHittingRedLight)
            {
                isHittingRedLight = false;
                StartCoroutine(WaitToResume());
            }
        }
    }

    IEnumerator WaitToResume()
    {
        yield return new WaitForSeconds(resumeDelay);
        ResumeMovement();
    }

    void ResumeMovement()
    {
        agent.isStopped = false;
    }
}
