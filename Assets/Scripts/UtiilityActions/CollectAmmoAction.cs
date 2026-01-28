using System.Collections.Generic;
using UnityEngine;

//Made with help of AI

public class CollectAmmoAction : UtilityActionAI
{
    [SerializeField] float weight;
    public List<Transform> ammoPos;
    public Transform ai;
    [SerializeField] float ammoLevel;
    float maxAmmo = 30f;

    public override float GetUtilityScore()
    {
        float desire = 1f - (ammoLevel / maxAmmo);
        if(ammoPos is null || ammoPos.Count <= 0) return float.MinValue;
        float distanceToAmmo = Vector3.Distance(ai.position, ammoPos[Random.Range(0, ammoPos.Count)].position);
        return (desire * weight) / distanceToAmmo;
    }

    public override void Execute()
    {
        transform.position = Vector3.MoveTowards(transform.position, ammoPos[Random.Range(0, ammoPos.Count)].position, 5f * Time.deltaTime);
        Debug.Log("Get ammo");
    }

    public void AddAmmo(float newValue)
    {
        ammoLevel += newValue;
        Debug.Log("Add ammo");
    }
}
