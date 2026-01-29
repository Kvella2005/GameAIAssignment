using System.Collections.Generic;
using UnityEngine;

//Made with help of AI

public class HideAction : UtilityActionAI
{
    [SerializeField] float weight;
    public List<Transform> hidePosList;
    public Transform ai;
    public Transform enemyAi;
    [SerializeField] float dangerLevel;
    float maxDangerLevel = 100f;
    Transform hidePos;

    public override float GetUtilityScore()
    {
        float desire = 1f - (dangerLevel/maxDangerLevel);

        if(hidePosList.Count > 0)
        {
            hidePos = hidePosList[Random.Range(0, hidePosList.Count)];
        }
        else return float.MinValue;

        if(hidePos == null || hidePos.position == Vector3.zero) return float.MinValue;
        float distanceToCover = Vector3.Distance(ai.position, hidePos.position);
        return (desire * weight) / distanceToCover;
    }

    public override void Execute()
    {
        transform.position = Vector3.MoveTowards(transform.position, hidePos.position, 5f * Time.deltaTime);
        Debug.Log("Hide");
    }
}
