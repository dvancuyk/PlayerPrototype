using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public float patrolTime = 10f;
    public float aggroRange = 10f;
    public Transform[] waypoints;


    private int index;
    private float speed, agentSpeed;
    private Transform player;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
            agentSpeed = agent.speed;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        index = Random.Range(0, waypoints.Length);

        InvokeRepeating("Tick", 0, 0.5f);

        if(waypoints.Length > 0)
        {
            InvokeRepeating("Patrol", 0, patrolTime);
        }
    }

    // We need to set the index of the waypoints that we're currently using.
    void Patrol()
    {
        index = index == waypoints.Length - 1
            ? 0
            : index + 1;

    }

    void Tick()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) < aggroRange)
        {
            Debug.Log("In Aggrorange! Attack!");
            agent.destination = player.position;
            agent.speed = agentSpeed;
        }
        else
        {
            Debug.Log($"Patrolling NPC moving to {waypoints[index].position}");
            agent.destination = waypoints[index].position;
            agent.speed = agentSpeed / 2;
        }
    }

}
