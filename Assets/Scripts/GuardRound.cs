using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class GuardRound : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float stopDistance = 0.5f;
    [SerializeField] private float waitTime = 3.0f;
    private NavMeshAgent agent;
    private int nextWaypointIndex = 0;
    private bool isWaiting = false;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Assert.IsNotNull(agent);
        Assert.IsTrue(waypoints.Count > 0);
        agent.autoBraking = true;
        agent.destination = waypoints[nextWaypointIndex].position;
        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }
    }

    private void Update()
    {
        if (!agent.pathPending && !isWaiting && agent.remainingDistance <= stopDistance)
        {
            StartCoroutine(WaitForNewDestination(waitTime));
        }
    }

    private IEnumerator WaitForNewDestination(float seconds)
    {
        isWaiting = true;
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
        }
        yield return new WaitForSeconds(seconds);
        nextWaypointIndex = (nextWaypointIndex + 1) % waypoints.Count;
        agent.SetDestination(waypoints[nextWaypointIndex].position);
        isWaiting = false;
        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }
        yield return null;
    }
}
