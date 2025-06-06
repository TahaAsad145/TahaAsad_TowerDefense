using UnityEngine;

public class BaseTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            BaseHealth.Instance.LoseLife();
            Destroy(other.gameObject);
            Debug.Log("Enemy reached base. Life lost.");
        }
    }
}
