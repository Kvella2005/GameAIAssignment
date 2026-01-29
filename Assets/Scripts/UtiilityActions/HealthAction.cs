using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Made with help of AI

public class HealAction: UtilityActionAI
{
    [SerializeField] float weight;
    public List<Transform> healthPosList;
    public NavMeshAgent ai;
    [SerializeField] float healthLevel;
    float maxHealth = 100f;
    Transform healthPos;

    public override float GetUtilityScore()
    {
        float desire = 1f - (healthLevel/maxHealth);
        if(healthPosList.Count > 0)
        {
            healthPos = GetClosestHealthPack();
        }
        else return float.MinValue;
        if(healthPos == null || healthPos.position == Vector3.zero) return float.MinValue;
        float distanceToHealth = Vector3.Distance(transform.position, healthPos.position);
        return (desire * weight) / distanceToHealth;
    }

    public override void Execute()
    {
        if(ai.remainingDistance <= ai.stoppingDistance)
        {
            ai.destination = GetClosestHealthPack().position;
        }
        Debug.Log("Get health");
    }

    public void AddHealth(float newValue)
    {
        healthLevel += newValue;
        healthPosList.Remove(healthPos);
    }

    public void TakeDamage(float damageValue)
    {
        healthLevel -= damageValue;
        if(healthLevel <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("Strategist eliminated");
        gameObject.SetActive(false);
    }

    public float getHealth()
    {
        return healthLevel;
    }

    private Transform GetClosestHealthPack()
    {
        Transform closest = null;
        float minDist = float.MaxValue;
        foreach (Transform t in healthPosList)
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
