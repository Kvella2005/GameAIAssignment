using UnityEngine;

//Made with help of AI

public abstract class UtilityActionAI : MonoBehaviour
{
    public abstract float GetUtilityScore();
    public abstract void Execute();
}