using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//source: https://simon-truong.medium.com/understanding-unity-fsm-e55979e79180
public class FSMAgentController : MonoBehaviour
{
    enum State
    {
        Patrol,
        Chase,
        Search
    }

    State currentState = State.Patrol;

    //Animator fsmAnimator;

    //ai navigation
    [SerializeField] NavMeshAgent agent;

    //guards destination
    [SerializeField] private List<Vector3> goals;
    
    //guards movement
    private Vector3 pos;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] Transform target;
    Vector3 lastPos;

    List<Transform> targets;
    [SerializeField] int currentIndex;
    int previousIndex;

    LayerMask aiLayerMask;

    void Awake()
    {
        //get components before scene starts
        //fsmAnimator = GetComponent<Animator>();
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
        switch(currentState)
        {
            case State.Patrol:
                agent.speed = speed;
                Patrol();
                Debug.Log("Patrol");
                break;
            case State.Search:
                Debug.Log("Search");
                break;
            case State.Chase:
                agent.speed = chaseSpeed;
                Chase();
                Debug.Log("Chase");
                break;
        }
    }

    void Patrol()
    {
        if(agent.remainingDistance <= .2f)
        {
            SetNextGoal();
        }

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, aiLayerMask))
        {
            currentState = State.Chase;
            target = hit.transform;
        }
    }

    void Chase()
    {
        if(Vector3.Distance(transform.position, target.transform.position) > 5f)
        {
            lastPos = target.transform.position;
            currentState = State.Search;
        }
    }

    void SetNextGoal()
    {
        do
        {
            currentIndex = UnityEngine.Random.Range(0, goals.Count);
        } while (currentIndex == previousIndex);

        agent.destination = goals[currentIndex];
        previousIndex = currentIndex;
    }
}
