using UnityEngine;

[System.Serializable]
public class TowerVariant
{
    public string name;
    public GameObject prefab;
    public int cost;
}
public class TowerPlacer : MonoBehaviour
{
    public TowerVariant[] towerVariants;
    public LayerMask buildSpotMask;

    public int selectedIndex = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray , out hit , 100f , buildSpotMask))
            {
                TowerVariant tower = towerVariants[selectedIndex];
                if(GoldManager.instance.currentGold >= tower.cost)
                {
                    Vector3 placementPosition = hit.transform.position + new Vector3(0, 0.5500002f, 0); 
                    GoldManager.instance.AddGold(-tower.cost);
                    GameObject placedTower = Instantiate(tower.prefab, placementPosition, Quaternion.identity);
                    Tower towerScript = placedTower.GetComponent<Tower>();
                    towerScript.buildSpotOrigin = hit.transform;
                    towerScript.buildSpotPrefab = hit.transform.gameObject;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) && towerVariants.Length > 1) selectedIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3) && towerVariants.Length > 2) selectedIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4) && towerVariants.Length > 3) selectedIndex = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5) && towerVariants.Length > 4) selectedIndex = 4;
    }
    public void SelectTower(int index)
    {
        if (index >= 0 && index < towerVariants.Length)
        {
            selectedIndex = index;
            Debug.Log("Selected tower: " + towerVariants[index].name);
        }
    }

}
