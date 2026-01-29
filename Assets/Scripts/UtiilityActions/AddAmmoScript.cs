using UnityEngine;

public class AddAmmoScript : MonoBehaviour
{
    [SerializeField] float addAmmo;

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collided with object");
        if(collision.gameObject.GetComponent<UtilityAI>())
        {
            collision.gameObject.GetComponent<CollectAmmoAction>().AddAmmo(addAmmo);
            Destroy(gameObject);
        }
    }
}
