using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Made with help of AI

public class HideAction : UtilityActionAI
{
    [SerializeField] float weight;
    public List<Transform> hidePosList;
    public NavMeshAgent ai;
    public List<Transform> enemyAi;
    [SerializeField] float dangerLevel;
    float maxDangerLevel = 100f;
    Transform hidePos;

    public override float GetUtilityScore()
    {
        if(hidePosList.Count > 0)
        {
            hidePos = GetClosestHide();
        }
        else return float.MinValue;

        float closestDistance = float.MaxValue;
        foreach(Transform enemy in enemyAi)
        {
            float distance = Vector3.Distance(transform.position, enemy.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
            }
        }
        dangerLevel = 1f / (closestDistance + 1f);

        float distanceToCover = Vector3.Distance(transform.position, hidePos.position);
        float desire = 1f - (dangerLevel/maxDangerLevel);
        return (desire * weight) / distanceToCover;
    }

    public override void Execute()
    {
        if(ai.destination != hidePos.position)
        {
            ai.destination = hidePos.position;
        }
        Debug.Log("Hide");
    }

    private Transform GetClosestHide()
    {
        Transform closest = null;
        float minDist = float.MaxValue;
        foreach (Transform t in hidePosList)
        {
            if (t == null) continue;
            float dist = Vector3.Distance(transform.position, t.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = t;
            }
        }
        return closest;
    }
}
