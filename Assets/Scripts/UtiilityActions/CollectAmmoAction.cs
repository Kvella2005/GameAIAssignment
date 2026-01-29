using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Made with help of AI

public class CollectAmmoAction : UtilityActionAI
{
    [SerializeField] float weight;
    public List<Transform> ammoPosList;
    public NavMeshAgent ai;
    [SerializeField] float ammoLevel;
    float maxAmmo = 30f;
    Transform ammoPos;

    public override float GetUtilityScore()
    {
        float desire = 1f - (ammoLevel / maxAmmo);

        if(ammoPosList.Count > 0)
        {
            ammoPos = GetClosestAmmo();
        }
        else return float.MinValue;

        if(ammoPos == null || ammoPos.position == Vector3.zero) return float.MinValue;
        float distanceToAmmo = Vector3.Distance(transform.position, ammoPos.position);
        return (desire * weight) / distanceToAmmo;
    }

    public override void Execute()
    {
        if(ai.remainingDistance <= ai.stoppingDistance)
        {
            ai.destination = GetClosestAmmo().position;
        }
        Debug.Log("Get ammo");
    }

    public void AddAmmo(float newValue)
    {
        ammoLevel += newValue;
        ammoPosList.Remove(ammoPos);
        Debug.Log("Add ammo");
    }

    private Transform GetClosestAmmo()
    {
        Transform closest = null;
        float minDist = float.MaxValue;
        foreach (Transform t in ammoPosList)
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
