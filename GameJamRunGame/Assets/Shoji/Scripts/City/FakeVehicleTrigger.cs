using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeVehicleTrigger : MonoBehaviour
{
    [SerializeField]
    int spawnDistance;
    [SerializeField]
    Vehicle vehiclePrefab;
    [SerializeField]
    Transform[] vehicleParents;
    const float kProbability = 0.5f;
    [SerializeField]
    float maxInstantiateWait;

    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip[] carDriveBy;

    private void OnTriggerEnter(Collider hit)
    {
        if (Random.Range(0f, 1f) > kProbability) return;
        Invoke("SpawnVehicle", Random.Range(0, maxInstantiateWait));
    }
    void SpawnVehicle()
    {
        int dir = Random.Range(0, 2) == 0 ? -1 : 1;
        Transform parent = vehicleParents[Random.Range(0, vehicleParents.Length)];
        Vector3 spawnPos = parent.TransformPoint(-dir * 3.4f, 0, spawnDistance * dir);
        Transform vehicle = Instantiate(vehiclePrefab, spawnPos, Quaternion.identity, parent).transform;
        vehicle.localEulerAngles = new Vector3(0, dir == -1 ? 0 : 180);
        source.PlayOneShot(carDriveBy[Random.Range(0, carDriveBy.Length)]);
    }
}
