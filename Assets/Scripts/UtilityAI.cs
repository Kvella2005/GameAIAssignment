using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Playables;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

//source: https://medium.com/@lemapp09/beginning-game-development-creating-ai-systems-70ee79d1ba5c
//also helped with AI
public class UtilityAI : MonoBehaviour
{
    [SerializeField] List<UtilityActionAI> actions = new List<UtilityActionAI>();

    [SerializeField] HealAction healAction;
    float minHealth = 30f;
    // Update is called once per frame
    void Update()
    {
        if(isSurvivalCritical())
        {
            healAction.Execute();
            return;
        }

        ExecuteBestAction();
    }

    bool isSurvivalCritical()
    {
        return healAction.getHealth() <= minHealth;
    }

    void ExecuteBestAction()
    {
        // utility ai code structure
        UtilityActionAI bestAction = null;
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
