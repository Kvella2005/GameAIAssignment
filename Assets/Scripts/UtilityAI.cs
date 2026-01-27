using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class UtilityAction : MonoBehaviour
{
    public abstract float GetUtilityScore();
    public abstract void Execute();
}

public abstract class HealAction: UtilityAction
{
    public Transform healthPackPos;
    public Transform ai;
    float healthLevel;

    public override float GetUtilityScore()
    {
        float distanceToHealth = Vector3.Distance(ai.position, healthPackPos.position);
        return healthLevel / distanceToHealth;
    }
}

public abstract class CollectAmmoAction : UtilityAction
{
    public Transform ammoPos;
    public Transform ai;
    public float ammoLevel;

    public override float GetUtilityScore()
    {
        float distanceToAmmo = Vector3.Distance(ai.position, ammoPos.position);
        return ammoLevel / distanceToAmmo;
    }
}

public abstract class HideAction : UtilityAction
{
    public Transform hidePos;
    public Transform ai;
    public float dangerLevel;

    public override float GetUtilityScore()
    {
        float distanceToCover = Vector3.Distance(ai.position, hidePos.position);
        return dangerLevel / distanceToCover;
    }
}

//source: https://medium.com/@lemapp09/beginning-game-development-creating-ai-systems-70ee79d1ba5c

public class UtilityAI : MonoBehaviour
{
    public UtilityAction[] actions;

    // Update is called once per frame
    void Update()
    {
        UtilityAction bestAction = null;
        float highestScore = float.MinValue;

        foreach (var action in actions)
        {
            float score = action.GetUtilityScore();
            if(score > highestScore)
            {
                highestScore = score;
                bestAction = action;
            }
        }

        if(bestAction != null)
        {
            bestAction.Execute();
        }
    }
}
