using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pedestrian : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;
    private List<Transform> pathNodes = new List<Transform>();

    private float raycastDistance=1f;
    private bool isHittingRedLight = false;
    private float resumeDelay = 2f;
    private string tagToFind = "nodes";
    private float maxDistance = 400f;
    private Rigidbody rb;
    private bool force = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Random.Range(1f, 4f);
        agent.acceleration = Random.Range(3f, 10f);
        // Find all path nodes within the specified distance
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag(tagToFind);
        foreach (GameObject go in foundObjects)
        {
            float distance = Vector3.Distance(transform.position, go.transform.position);
            if (distance <= maxDistance)
            {
                pathNodes.Add(go.transform);
            }
        }

        SetRandomDestination();
    }

    void Update()
    {
        if(force==false)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                SetRandomDestination();
            }

            CheckForObstacles();
        }

    }

    void SetRandomDestination()
    {
        if (pathNodes.Count > 0)
        {
            Transform randomNode = pathNodes[Random.Range(0, pathNodes.Count)];
            agent.SetDestination(randomNode.position);
        }
    }

    void CheckForObstacles()
    {
        Vector3 raycastOrigin = new Vector3(transform.position.x, 1f, transform.position.z);
        Vector3 direction = agent.desiredVelocity.normalized;
        Ray ray = new Ray(raycastOrigin, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("redlight"))
            {
                if (!isHittingRedLight)
                {
                    isHittingRedLight = true;
                    agent.isStopped = true;
                    anim.SetBool("ideal", true);
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
    {   if(force==false)
        {
            agent.isStopped = false;
            anim.SetBool("ideal", false);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            Debug.Log("touch");
            agent.enabled = false;
            rb.isKinematic = false;
            Vector3 collisionDirection = other.transform.position - transform.position;
            // Apply impulse force in the direction of the collision direction
            rb.AddForce(-collisionDirection.normalized * 50, ForceMode.Impulse);
        
            force = true;
        }
    }
}