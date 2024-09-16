using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleTriggerSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject vehicleTriggerPrefab;
    readonly Vector3[] offsets = new Vector3[4]
        {new Vector3(0,0,8),new Vector3(8,0,0),new Vector3(0,0,-8),new Vector3(-8,0,0)};
    private void Awake()
    {
        for (int i = -3; i < 4; i++)
        {
            for (int k = -3; k < 4; k++)
            {
                Vector3 tilePosition = new Vector3(i * 80, 0, k * 80);
                for (int m = 0; m < 4; m++)
                {
                    Vector3 position = tilePosition + offsets[m];
                    Quaternion rotation = Quaternion.Euler(0, m * 90, 0);
                    Instantiate(vehicleTriggerPrefab, position, rotation, transform);
                }
            }
        }
    }
}
