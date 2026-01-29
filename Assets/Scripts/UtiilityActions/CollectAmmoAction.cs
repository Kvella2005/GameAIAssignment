using System.Collections.Generic;
using UnityEngine;

//Made with help of AI

public class CollectAmmoAction : UtilityActionAI
{
    [SerializeField] float weight;
    public List<Transform> ammoPosList;
    public Transform ai;
    [SerializeField] float ammoLevel;
    float maxAmmo = 30f;
    Transform ammoPos;

    public override float GetUtilityScore()
    {
        float desire = 1f - (ammoLevel / maxAmmo);

        if(ammoPosList.Count > 0)
        {
            ammoPos = ammoPosList[Random.Range(0, ammoPosList.Count)];
        }
        else return float.MinValue;

        if(ammoPos == null || ammoPos.position == Vector3.zero) return float.MinValue;
        float distanceToAmmo = Vector3.Distance(ai.position, ammoPos.position);
        return (desire * weight) / distanceToAmmo;
    }

    public override void Execute()
    {
        transform.position = Vector3.MoveTowards(transform.position, ammoPos.position, 5f * Time.deltaTime);
        Debug.Log("Get ammo");
    }

    public void AddAmmo(float newValue)
    {
        ammoLevel += newValue;
        ammoPosList.Remove(ammoPos);
        Debug.Log("Add ammo");
    }
}
