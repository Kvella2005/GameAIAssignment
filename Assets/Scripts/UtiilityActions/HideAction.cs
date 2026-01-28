using UnityEngine;

//Made with help of AI

public class HideAction : UtilityActionAI
{
    [SerializeField] float weight;
    public Transform hidePos;
    public Transform ai;
    public Transform enemyAi;
    [SerializeField] float dangerLevel;
    float maxDangerLevel = 100f;

    public override float GetUtilityScore()
    {
        float desire = 1f - (dangerLevel/maxDangerLevel);
        if(hidePos is null) return float.MinValue;
        float distanceToCover = Vector3.Distance(ai.position, hidePos.position);
        return (desire * weight) / distanceToCover;
    }

    public override void Execute()
    {
        transform.position = Vector3.MoveTowards(transform.position, hidePos.position, 5f * Time.deltaTime);
        Debug.Log("Hide");
    }
}
