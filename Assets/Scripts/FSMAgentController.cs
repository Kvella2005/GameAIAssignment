using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMAgentController : MonoBehaviour
{
    enum State
    {
        Patrol,
        Chase,
        Search
    }

    State currentState;

    Animator fsmAnimator;

    //ai navigation
    NavMeshAgent agent;

    //guards destination
    [SerializeField] private List<Vector3> goals;
    
    //guards movement
    private Vector3 pos;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] Transform target;

    List<Transform> targets;
    int currentIndex;
    int previousIndex;

    void Awake()
    {
        //get components before scene starts
        fsmAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        pos = transform.position;

        GameObject[] goalGOs = GameObject.FindGameObjectsWithTag("guardGoal");

        //to find each destination for the guard
        foreach (var goalGO in goalGOs)
        {
            goals.Add(goalGO.transform.position);
        }
    }   

    void Start()
    {
        SetNextGoal();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Patrol()
    {
        if(agent.remainingDistance <= .2f)
        {
            SetNextGoal();
            agent.destination = goals[currentIndex];
        }
    }

    void SetNextGoal()
    {
        do
        {
            currentIndex = UnityEngine.Random.Range(0, goals.Count);
        } while (currentIndex == previousIndex);
    }
}
