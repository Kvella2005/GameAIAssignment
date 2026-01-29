using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

//source: https://medium.com/@lemapp09/beginning-game-development-creating-ai-systems-70ee79d1ba5c
//also helped with AI
public class UtilityAI : MonoBehaviour
{
    [SerializeField] List<UtilityActionAI> actions = new List<UtilityActionAI>(); //standard action

    [SerializeField] HealAction healAction; // special action
    HideAction hideAction; // to reset timer
    float minHealth = 30f;

    void Awake()
    {
        //get it in awake
        hideAction = GetComponent<HideAction>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isSurvivalCritical()) // if health is low 
        {
            healAction.Execute(); // override standard action and execute heal action
            return;
        }

        ExecuteBestAction(); //get best action to execture
    }

    bool isSurvivalCritical()
    {
        return healAction.getHealth() <= minHealth; // if health is less than minimum recommended health
    }

    void ExecuteBestAction()
    {
        // utility ai code structure
        UtilityActionAI bestAction = null;
        float highestScore = float.MinValue;

        foreach (var action in actions)
        {
            float score = action.GetUtilityScore();
            if(score > highestScore) // if utility action is scored the highest, then store action
            {
                highestScore = score;
                bestAction = action;
            }
        }

        if(bestAction != null)
        {
            bestAction.Execute(); //exectue the utility aciton with the highest score
        
            // if(bestAction != hideAction && hideAction.getCooldown() <= 0) hideAction.ResetTimer();
        }
    }
}
