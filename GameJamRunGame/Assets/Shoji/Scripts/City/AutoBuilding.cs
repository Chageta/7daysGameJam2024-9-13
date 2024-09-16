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
    CrowdControler crowd;

    Vector2Int stadiumAddress;
    private void Awake()
    {
        InitializeCity();
        StartGame();
    }
    void InitializeCity()
    {
        stadiumAddress = new Vector2Int(Random.Range(-3, 3), Random.Range(-3, 3));
        for (int i = -3; i < 3; i++)
        {
            for (int k = -3; k < 3; k++)
            {
                Vector3 position = new Vector3(i * 80 + 40, 0, k * 80 + 40);
                Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
                GameObject building = (i == stadiumAddress.x && k == stadiumAddress.y) ? stadium :
                    Instantiate(commonBuildings[Random.Range(0, commonBuildings.Length)], position, rotation, buildingRoot);
                building.transform.SetPositionAndRotation(position, rotation);
            }
        }
    }
    void StartGame()
    {
        InitializeCrowd();
        ResultManager.StartTimer();
    }
    void InitializeCrowd()
    {
        Vector3 position = InitCrowdPosition();
        int direction = Random.Range(0, 4);
        Vector2 adjust = new Vector2(Random.Range(0, 1) == 0 ? -1 : 1, Random.Range(0, 1) == 0 ? -1 : 1);
        position += new Vector3(adjust.x * 32, 0, adjust.y * 32);
        crowd.InitializeCrowd(position, direction);
        crowd.BeginMove();
    }
    Vector3 InitCrowdPosition()
    {
        Vector2Int address = new Vector2Int(Random.Range(-2, 2), Random.Range(-2, 2));
        if (address == stadiumAddress) return InitCrowdPosition();
        return new Vector3(address.x * 80 + 40, 0, address.y * 80 + 40);
    }
}
