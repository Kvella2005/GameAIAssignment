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
    Transform hidePos;
    Transform currentSpot;
    float hideTimer; 
    float maxHideTimer = 3.5f; 
    float cooldownTimer = 0f; 

    public override float GetUtilityScore()
    {
        if(cooldownTimer > 0f)
        {
            cooldownTimer-=Time.deltaTime;
        }

        if(hidePosList.Count == 0) return float.MinValue;

        if(cooldownTimer > 0f)
        {
            return 0;
        }

        float closestDistance = float.MaxValue;
        foreach(Transform enemy in enemyAi)
        {
            float distance = Vector3.Distance(transform.position, enemy.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
            }
        }
        float decayFactor = Mathf.Clamp01(1f - (hideTimer / maxHideTimer));
        float dangerLevel = 1f / (closestDistance + 1f);

        return (dangerLevel * weight) * decayFactor;
    }

    public override void Execute()
    {    
        if(hidePos == null)
        {
            // Use the actual transform for the exclusion check, not just position
            hidePos = GetClosestHide(currentSpot != null ? currentSpot.position : (Vector3?)null);
            
            if(hidePos != null)
            {
                hideTimer = 0.001f; // Start the timer so we don't re-pick next frame
                ai.SetDestination(hidePos.position);
                currentSpot = hidePos; // Mark this as our "active" spot
            }
        }

        if(hidePos!= null && !ai.pathPending && ai.remainingDistance <= ai.stoppingDistance)
        {
            hideTimer+=Time.deltaTime;

            if (hideTimer >= maxHideTimer)
            {
                cooldownTimer = 5f;
                hidePos = null;
                hideTimer = 0f;
            }
        }
    }

    private Transform GetClosestHide(Vector3? currentSpot = null)
    {
        Transform closest = null;
        float maxMinDist = float.MinValue;

        foreach (Transform t in hidePosList)
        {
            if (t == null) continue;
            float dist = Vector3.Distance(transform.position, t.position);
            if(dist < 1.5f || (currentSpot.HasValue && t.position == currentSpot.Value))
                continue;

            float closestEnemyDistance = float.MaxValue;
            foreach(Transform enemy in enemyAi)
            {
                float d = Vector3.Distance(t.position, enemy.position);
                if(d < closestEnemyDistance) closestEnemyDistance = d;
            }
            if (closestEnemyDistance > maxMinDist)
            {
                maxMinDist = closestEnemyDistance;
                closest = t;
            }
        }
        return closest;
    }

    public void ResetTimer()
    {
        if(cooldownTimer <= 0)
        {
            hideTimer = 0.0f;
        }
        hidePos = null;
    }

    public float getCooldown()
    {
        return cooldownTimer;
    }
}
