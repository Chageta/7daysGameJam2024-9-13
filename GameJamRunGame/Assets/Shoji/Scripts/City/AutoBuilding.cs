using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBuilding : MonoBehaviour
{
    [SerializeField]
    Transform buildingRoot;
    [SerializeField]
    GameObject[] commonBuildings;

    [SerializeField]
    Transform objectiveRoot;
    [SerializeField]
    GameObject stadium;
    [SerializeField]
    GameObject subObjective;
    [SerializeField]
    CrowdControler crowd;

    Vector2Int stadiumAddress;
    Vector2Int subObjectiveAddress;
    private void Awake()
    {
        InitializeCity();
        StartGame();
    }
    void InitializeCity()
    {
        stadiumAddress = new Vector2Int(Random.Range(-3, 3), Random.Range(-3, 3));
        subObjectiveAddress = stadiumAddress + new Vector2Int(3 + Random.Range(1, 7), 3 + Random.Range(1, 7));
        subObjectiveAddress = new Vector2Int(subObjectiveAddress.x % 7 - 3, subObjectiveAddress.y % 7 - 3);
        bool hasSubObjective = DifficultyManager.Instance.Difficulty != 0;
        subObjective.SetActive(hasSubObjective);
        for (int i = -3; i < 3; i++)
        {
            for (int k = -3; k < 3; k++)
            {
                Vector3 position = new Vector3(i * 80 + 40, 0, k * 80 + 40);
                Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
                bool isStadium = i == stadiumAddress.x && k == stadiumAddress.y;
                bool isSubObjective = i == subObjectiveAddress.x && k == subObjectiveAddress.y && hasSubObjective;
                GameObject building = isStadium ? stadium :
                    isSubObjective ? subObjective :
                    Instantiate(commonBuildings[Random.Range(0, commonBuildings.Length)], position, rotation, buildingRoot);
                building.transform.SetPositionAndRotation(position, rotation);
            }
        }
    }
    void StartGame()
    {
        InitializeCrowd();
    }
    void InitializeCrowd()
    {
        Vector3 position = InitCrowdPosition();
        int direction = Random.Range(0, 4);
        Vector2 adjust = new Vector2(Random.Range(0, 1) == 0 ? -1 : 1, Random.Range(0, 1) == 0 ? -1 : 1);
        position += new Vector3(adjust.x * 32, 0, adjust.y * 32);
        crowd.InitializeCrowd(position, direction);
    }
    Vector3 InitCrowdPosition()
    {
        Vector2Int address = new Vector2Int(Random.Range(-2, 2), Random.Range(-2, 2));
        bool hasSubObjective = address == subObjectiveAddress && DifficultyManager.Instance.Difficulty != 0;
        if (address == stadiumAddress || hasSubObjective) return InitCrowdPosition();
        return new Vector3(address.x * 80 + 40, 0, address.y * 80 + 40);
    }
}
