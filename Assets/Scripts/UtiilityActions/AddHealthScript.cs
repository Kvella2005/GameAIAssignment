using UnityEngine;

public class AddHealthScript : MonoBehaviour
{
    [SerializeField] float addHealth;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with object");
        if(collision.gameObject.GetComponent<UtilityAI>())
        {
            collision.gameObject.GetComponent<HealAction>().AddHealth(addHealth);
            Destroy(gameObject);
        }
    }
}
