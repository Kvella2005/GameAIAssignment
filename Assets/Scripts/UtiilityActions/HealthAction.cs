using System.Collections.Generic;
using UnityEngine;

//Made with help of AI

public class HealAction: UtilityActionAI
{
    [SerializeField] float weight;
    public List<Transform> healthPosList;
    public Transform ai;
    [SerializeField] float healthLevel;
    float maxHealth = 100f;
    Transform healthPos;

    public override float GetUtilityScore()
    {
        float desire = 1f - (healthLevel/maxHealth);
        if(healthPosList.Count > 0)
        {
            healthPos = healthPosList[Random.Range(0, healthPosList.Count)];
        }
        else return float.MinValue;
        if(healthPos == null || healthPos.position == Vector3.zero) return float.MinValue;
        float distanceToHealth = Vector3.Distance(ai.position, healthPos.position);
        return (desire * weight) / distanceToHealth;
    }

    public override void Execute()
    {
        transform.position = Vector3.MoveTowards(transform.position, healthPos.position, 5f * Time.deltaTime);
        Debug.Log("Get health");
    }

    public void AddHealth(float newValue)
    {
        healthLevel += newValue;
        healthPosList.Remove(healthPos);
    }

    public float getHealth()
    {
        return healthLevel;
    }
}
