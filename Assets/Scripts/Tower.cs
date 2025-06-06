using TMPro;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject bulletPrefabs;
    public float fireRate = 1f;
    public float fireRange = 10.0f;
    private float fireCountDown = 0f;
    public float bulletDamage = 1f;

    // Upgrade Tower

    public int level = 1;
    public float rangeUpgrade = 2f;
    public float fireRateUpgrade = 0.5f;
    public int upgradeCost = 30;
    public int maxLevel = 20;

    public float bulletDamageUpgrade = 0.5f;
    public float splashRadiusUpgrade = 0.3f;
    public float slowPowerUpgrade = 0.05f;
    public float poisonDamageUpgrade = 0.5f;
    public float poisonDurationUpgrade = 0.5f;

    public float upgradeCostMultiplier = 1.5f;


    // Tower Effects

    public bool isSplash = false;
    public float splashRadius = 2f;

    public bool isSlow = false;
    public float slowSpeed = 0.5f;
    public float slowDuration = 2f;

    public bool isPoison = false;
    public float poisonDamage = 1f;
    public float poisonDuration = 3f;

    // Tower Level

    public TextMeshPro levelText;

    public static Tower selectedTower;

    public GameObject buildSpotPrefab;
    public Transform buildSpotOrigin;

    public GameObject shootEffect;
    public GameObject upgradeEffect;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelText = GetComponentInChildren<TextMeshPro>();
        if (levelText != null)
        {
            Debug.Log("LevelText found!");
            levelText.text = "Lv. " + level;
        }
        else
        {
            Debug.LogWarning("LevelText NOT FOUND on: " + gameObject.name);
        }
    }
    // Update is called once per frame
    void Update()
    {

        fireCountDown -= Time.deltaTime;
        GameObject target = FindClosestEnemy();

        if (target != null && fireCountDown <= 0)
        {
            Shoot(target);
            fireCountDown = 1f / fireRate;
        }
    }
    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float shortestDis = Mathf.Infinity;
        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < shortestDis && dist <= fireRange)
            {
                shortestDis = dist;
                closest = enemy;
            }
        }
        return closest;
    }

    void Shoot(GameObject enemy)
    {

        GameObject bullet = Instantiate(bulletPrefabs, transform.position, Quaternion.identity);
        if (shootEffect != null)
            Instantiate(shootEffect, transform.position, Quaternion.identity);

        Bullet bulletObject = bullet.GetComponent<Bullet>();

        if (bulletObject != null)
        {
            bulletObject.SetTarget(enemy.transform);
            Debug.Log("Bullet launched at " + enemy.name);

            bulletObject.damage = bulletDamage;


            bulletObject.splash = isSplash;
            bulletObject.splashRadius = splashRadius;

            bulletObject.slow = isSlow;
            bulletObject.slowSpeed = slowSpeed;
            bulletObject.slowDuration = slowDuration;

            bulletObject.poison = isPoison;
            bulletObject.poisonDuration = poisonDuration;
            bulletObject.poisonDamage = poisonDamage;

        }
        else
        {
            Debug.LogWarning("Bullet script not found on bullet prefab!");
        }

    }
    public void Upgrade()
    {
        if (level >= maxLevel)
        {
            Debug.Log("Tower is at max level.");
            return;
        }

        if (GoldManager.instance.currentGold >= upgradeCost)
        {
            level++;
            transform.localScale *= 1.1f;
            if (upgradeEffect != null)
                Instantiate(upgradeEffect, transform.position, Quaternion.identity);
            GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.yellow, level / (float)maxLevel);

            if (levelText != null)
            {
                levelText.text = "Lv. " + level;
                Debug.Log("Level text updated to: Lv. " + level);
            }
            else
            {
                Debug.LogWarning("levelText is null in Upgrade()");
            }


            // Scale core stats
            fireRange += rangeUpgrade;
            fireRate += fireRateUpgrade;
            bulletDamage += bulletDamageUpgrade;

            // Scale effect stats
            if (isSplash)
                splashRadius += splashRadiusUpgrade;

            if (isSlow)
                slowSpeed += slowPowerUpgrade; // Note: lower = slower

            if (isPoison)
            {
                poisonDamage += poisonDamageUpgrade;
                poisonDuration += poisonDurationUpgrade;
            }

            GoldManager.instance.AddGold(-upgradeCost);

            // Increase next upgrade cost
            upgradeCost = Mathf.CeilToInt(upgradeCost * upgradeCostMultiplier);

            Debug.Log($"Tower upgraded to level {level}. New fireRate: {fireRate}, damage: {bulletDamage}, next cost: {upgradeCost}");
        }
        else
        {
            Debug.Log("Not enough gold to upgrade.");
        }
    }

    void OnMouseOver()
    {
        TowerUIManager.instance.Show(this);
        if (Input.GetMouseButtonDown(1))
        {
            Upgrade();
        }
    }


}
