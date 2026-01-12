using UnityEngine;

public abstract class UtilityAction : MonoBehaviour
{
    public abstract float GetUtilityScore();
}

public abstract class HealAction: UtilityAction
{
    
}

public abstract class CollectAmmoAction : UtilityAction
{

}

public abstract class HideAction : UtilityAction
{

}


public class UtilityAI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
