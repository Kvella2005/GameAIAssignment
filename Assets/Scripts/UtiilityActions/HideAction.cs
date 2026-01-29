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
        if(cooldownTimer > 0f) // if the cooldown timer is still running (to prevent frame by frame resetting timer)
        {
            cooldownTimer-=Time.deltaTime;
        }

        if(hidePosList.Count == 0) return float.MinValue; // to check null

        if(cooldownTimer > 0f) // if timer is still going 
        {
            return 0;
        }

        float closestDistance = float.MaxValue; //get closest enemy
        foreach(Transform enemy in enemyAi)
        {
            float distance = Vector3.Distance(transform.position, enemy.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
            }
        }
        float decayFactor = Mathf.Clamp01(1f - (hideTimer / maxHideTimer)); //prevent the ai to hide for too long
        float dangerLevel = 1f / (closestDistance + 1f); // get danger level from enemies closest distance

        return (dangerLevel * weight) * decayFactor; // calculate utility score
    }

    public override void Execute()
    {    
        if(hidePos == null) //if there is no hide position
        {
            // Use the actual transform for the exclusion check, not just position
            hidePos = GetClosestHide(currentSpot != null ? currentSpot.position : (Vector3?)null); // get closest hide positon 
            
            if(hidePos != null) //set destination to that and set timer to .001f
            {
                hideTimer = 0.001f; // Start the timer so we don't re-pick next frame
                ai.SetDestination(hidePos.position);
                currentSpot = hidePos; // Mark this as our "active" spot
            }
        }

        if(hidePos!= null && !ai.pathPending && ai.remainingDistance <= ai.stoppingDistance) //if ai reached destination, the start timer
        {
            hideTimer+=Time.deltaTime;

            if (hideTimer >= maxHideTimer) // if timer is finished, resest everything
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

        foreach (Transform t in hidePosList) // go through every waypoint
        {
            if (t == null) continue; // if it is null, then skip
            float dist = Vector3.Distance(transform.position, t.position); //check distance between position and i
            if(dist < 1.5f || (currentSpot.HasValue && t.position == currentSpot.Value)) //to prevent being in the same spot
                continue;

            //check if the enemy is close to the hide position
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

    public void ResetTimer() //reset timer and hide position
    {
        if(cooldownTimer <= 0)
        {
            hideTimer = 0.0f;
        }
        hidePos = null;
    }

    public float getCooldown() //get cooldown timer
    {
        return cooldownTimer;
    }
}
