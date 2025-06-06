using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    private Transform target;


    // Bullet Attributes
    public float damage;

    public bool splash;
    public float splashRadius;

    public bool slow;
    public float slowSpeed;
    public float slowDuration;


    public bool poison;
    public float poisonDamage;
    public float poisonDuration;
    public GameObject splashEffectPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }
    public void SetTarget(Transform targetPosition)
    {
        target = targetPosition;
    }
    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            Debug.Log("Target Missed");
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distance = bulletSpeed * Time.deltaTime;

        if (dir.magnitude <= distance)
        {
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized * distance, Space.World);
    }
    void HitTarget()
    {
        EnemyHealth health = target.GetComponent<EnemyHealth>();
        if (splash)
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, splashRadius);
            foreach (var hit in enemies)
            {
                if (hit.CompareTag("Enemy"))
                {
                    ApplyEffects(hit.transform);
                    if (splashEffectPrefab != null)
                        Instantiate(splashEffectPrefab, transform.position, Quaternion.identity);
                }
            }
        }
        else
        {
            ApplyEffects(target);
        }
        Destroy(gameObject);
    }

    void ApplyEffects(Transform enemy)
    {
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }
        if (slow)
        {
            enemyHealth.ApplySlow(slowSpeed, slowDuration);
        }

        if (poison)
        {
            enemyHealth.ApplyPoison(poisonDamage, poisonDuration);
        }
        Debug.Log($"Applied to {enemy.name}: damage={damage}, slow={slow}, poison={poison}");
    }
}
