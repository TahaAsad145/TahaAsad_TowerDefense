using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int lvl = 1;
    public float baseHealth = 3f;
    public float healthPerLvl = 2f;

    private float currentHealth;
    private bool isDead = false; 
    public bool isSpecial = false;
    public int goldValue = 10;

    private bool isSlowed = false;
    private float originalSpeed;
    private Coroutine poisonCoroutine;
    private Coroutine slowCoroutine;

    public GameObject deathEffect;
    public GameObject healthBarPrefab;
    private EnemyHealthBar healthBar;
    public GameObject poisonEffectPrefab;
    public GameObject slowEffectPrefab;
    public AudioClip deathSound;
    private AudioSource audioSource;
    void Start()
    {
        currentHealth = baseHealth + (healthPerLvl * (lvl - 1));
        Debug.Log($"{gameObject.name} spawned with health {currentHealth}");
        audioSource = GetComponent<AudioSource>();

        currentHealth = baseHealth + (healthPerLvl * (lvl - 1));

        GameObject bar = Instantiate(healthBarPrefab, transform.position, Quaternion.identity, transform);
        healthBar = bar.GetComponent<EnemyHealthBar>();
        healthBar.target = transform;
        healthBar.offset = new Vector3(0, 2f, 0);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, baseHealth + (healthPerLvl * (lvl - 1)));
        }
    }

    public void ApplySlow(float slowAmount, float duration)
    {
        if (isDead || isSlowed) return;

        EnemyMovement move = GetComponent<EnemyMovement>();
        if (move != null)
        {
            originalSpeed = move.speed;
            move.speed *= (1 - slowAmount);
            isSlowed = true;
            slowCoroutine = StartCoroutine(RemoveSlowAfterTime(duration, move));

            if (slowEffectPrefab != null)
                Instantiate(slowEffectPrefab, transform.position + Vector3.up * 1.2f, Quaternion.identity, transform);
        }
    }

    IEnumerator RemoveSlowAfterTime(float time, EnemyMovement movement)
    {
        yield return new WaitForSeconds(time);
        if (!isDead && movement != null)
        {
            movement.speed = originalSpeed;
            isSlowed = false;
        }
    }

    public void ApplyPoison(float damagePerSecond, float duration)
    {
        if (isDead) return;

        if (poisonCoroutine != null)
            StopCoroutine(poisonCoroutine);

        poisonCoroutine = StartCoroutine(DoPoison(damagePerSecond, duration));

        if (poisonEffectPrefab != null)
            Instantiate(poisonEffectPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity, transform);

        Debug.Log("Poison applied: " + damagePerSecond + " for " + duration + "s");
    }

    IEnumerator DoPoison(float dps, float time)
    {
        float elapsed = 0f;
        while (elapsed < time)
        {
            if (isDead) yield break;

            TakeDamage(dps * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"{gameObject.name} is killed");

        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);

        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        if (healthBar != null)
            Destroy(healthBar.gameObject);

        GoldManager.instance.AddGold(goldValue);



        Destroy(gameObject);
    }


}
