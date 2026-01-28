using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.InputSystem;

//source: https://medium.com/@lemapp09/beginning-game-development-creating-ai-systems-70ee79d1ba5c

public class UtilityAI : MonoBehaviour
{
    [SerializeField] List<UtilityActionAI> actions = new List<UtilityActionAI>();

    // Update is called once per frame
    void Update()
    {
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
