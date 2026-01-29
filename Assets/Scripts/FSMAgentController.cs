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

    [SerializeField] LayerMask aiLayerMask;
    float searchTimer = 0.0f;
    bool attack = true;
    float attackCooldown;

    void Awake()
    {
        //get components before scene starts
        //fsmAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        pos = transform.position;

        // GameObject[] goalGOs = GameObject.FindGameObjectsWithTag("guardGoal");

        // //to find each destination for the guard
        // foreach (var goalGO in goalGOs)
        // {
        //     goals.Add(goalGO.transform.position);
        // }
    }   

    void Start()
    {
        SetNextGoal();
    }

    // Update is called once per frame
    void Update()
    {
        //state machine main functionality
        switch(currentState)
        {
            case State.Patrol: // on patrol go according to the waypoints
                agent.speed = speed;
                Patrol();
                //Debug.Log("Patrol");
                break;
            case State.Search: // stay the enemies last seen position
                //Debug.Log("Search");
                Search();
                break;
            case State.Chase: // chase enemy if raycast caught the enemy
                agent.speed = chaseSpeed;
                Chase();
                //Debug.Log("Chase");
                break;
        }
    }

    private void Search()
    {
        searchTimer+=Time.deltaTime;

        if(searchTimer>=5f) //if the timer is over 5 secs, then switch to patrol
        {
            searchTimer = 0f;
            currentState = State.Patrol;
        }
        else
        {
            // if raycast caught the enemy, then switch to chase and get reference of the enemy
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, aiLayerMask))
            {
                currentState = State.Chase;
                target = hit.transform;
            }
        }
    }

    void Patrol()
    {
        if(agent.remainingDistance <= .2f) //set next goal if the ai is close to the destination
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
        if (target!= null ||target.gameObject.activeInHierarchy)
        {
            float enemyDist = Vector3.Distance(transform.position, target.transform.position);
            if(enemyDist > 5f) // if the enemy is far away, then get the latest postion of the enemy
            {
                lastPos = target.transform.position;
                currentState = State.Search;
            }
            else
            {
                if(enemyDist <= 1.5f) // attack if close enough
                {
                    if(attack)
                    {
                        Attack();   
                    }
                }
                else if(agent.destination != target.transform.position)
                {
                    agent.destination = target.transform.position;
                }
            }

            if(attackCooldown >= 1.5f) //to prevent infinite damage 
            {
                attackCooldown = 0f;
                attack = true;
            }
            else if (!attack) // to start timer
            {
                attackCooldown += Time.deltaTime;
            }    
        }
        else
        {
            agent.ResetPath(); // to clear all path data
            currentState = State.Patrol; // if the enemy is no longer there
            return;
        }
    }

    void SetNextGoal()
    {
        //get the next random position to go to
        do
        {
            currentIndex = UnityEngine.Random.Range(0, goals.Count);
        } while (currentIndex == previousIndex);

        agent.destination = goals[currentIndex];
        previousIndex = currentIndex;
    }

    void Attack()
    {
        //if the ai attacks any enemy, damge it
        string layerName = LayerMask.LayerToName(target.gameObject.layer);
        if(layerName == "AI")
        {
            Debug.Log("Attack");
            target.gameObject.SendMessage("TakeDamage", 15f, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            return;
        }

        attack = false;
    }
}
