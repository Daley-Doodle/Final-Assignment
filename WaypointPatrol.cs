using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;

    // normal / burst speed of ghost
    public float normalSpeed = 1.5f;  
    public float burstSpeed = 3.5f;

    // min / max burst duration
    public float burstDurationMin = 1.0f;  
    public float burstDurationMax = 3.0f;

    // min / max time between speed bursts
    public float burstIntervalMin = 5.0f; 
    public float burstIntervalMax = 10.0f; 

    private int m_CurrentWaypointIndex;

    void Start()
    {
        // makes sure ghost starts at normal speed
        navMeshAgent.speed = normalSpeed; 
        navMeshAgent.SetDestination(waypoints[0].position);

        // starts random speed burst coroutine
        StartCoroutine(RandomSpeedBurst());  
    }

    void Update()
    {
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
    }

    // handles random speed bursts
    private IEnumerator RandomSpeedBurst()
    {
        while (true)
        {
            // waits for a random time before triggering next speed burst
            float waitTime = Random.Range(burstIntervalMin, burstIntervalMax);
            yield return new WaitForSeconds(waitTime);

            // triggers speed burst
            yield return StartCoroutine(SpeedBurst());
        }
    }

    // temporarily increases speed of ghost
    private IEnumerator SpeedBurst()
    {
        // increases speed to burst speed
        navMeshAgent.speed = burstSpeed;

        // waits for random burst duration
        float burstDuration = Random.Range(burstDurationMin, burstDurationMax);
        yield return new WaitForSeconds(burstDuration);

        // reverts speed to normal
        navMeshAgent.speed = normalSpeed;
    }
}