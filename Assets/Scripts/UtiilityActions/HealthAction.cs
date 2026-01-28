using UnityEngine;

//Made with help of AI

public class HealAction: UtilityActionAI
{
    [SerializeField] float weight;
    public Transform healthPackPos;
    public Transform ai;
    [SerializeField] float healthLevel;
    float maxHealth = 100f;

    public override float GetUtilityScore()
    {
        float desire = 1f - (healthLevel/maxHealth);
        if(healthPackPos is null) return float.MinValue;
        float distanceToHealth = Vector3.Distance(ai.position, healthPackPos.position);
        return (desire * weight) / distanceToHealth;
    }

    public override void Execute()
    {
        transform.position = Vector3.MoveTowards(transform.position, healthPackPos.position, 5f * Time.deltaTime);
        Debug.Log("Get health");
    }

    public void AddHealth(float newValue)
    {
        healthLevel += newValue;
    }
}
