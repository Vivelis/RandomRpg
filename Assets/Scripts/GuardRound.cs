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

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        Assert.IsNotNull(agent);
        Assert.IsTrue(waypoints.Count > 0);
        agent.autoBraking = true;
        agent.destination = waypoints[nextWaypointIndex].position;
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
        yield return new WaitForSeconds(seconds);
        nextWaypointIndex = (nextWaypointIndex + 1) % waypoints.Count;
        agent.SetDestination(waypoints[nextWaypointIndex].position);
        isWaiting = false;
        yield return null;
    }
}
