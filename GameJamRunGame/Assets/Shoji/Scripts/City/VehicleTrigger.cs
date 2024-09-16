using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleTrigger : MonoBehaviour
{
    [SerializeField]
    int spawnDistance;
    [SerializeField]
    Vehicle vehiclePrefab;
    bool coolTime = false;
    [SerializeField]
    bool outer;
    private void OnTriggerEnter(Collider hit)
    {
        if (coolTime) return;
        if (hit.gameObject.layer != 9) return;
        CrowdActor actor = hit.GetComponent<CrowdActor>();
        if ((!actor.IsTexting && !outer) || actor.IsDead) return;

        coolTime = true;
        StartCoroutine(CoolTime());
        StartCoroutine(SpawnVehicles());
    }
    IEnumerator CoolTime()
    {
        yield return new WaitForSeconds(3);
        coolTime = false;
    }
    IEnumerator SpawnVehicles()
    {
        int vehicleCount = 10;
        while (vehicleCount > 0)
        {
            int dir = vehicleCount % 2 == 0 ? -1 : 1;
            Vector3 spawnPos = transform.TransformPoint(-dir * 3.4f, 0, spawnDistance * dir);
            Transform vehicle = Instantiate(vehiclePrefab, spawnPos, Quaternion.identity, transform).transform;
            vehicle.localEulerAngles = new Vector3(0, vehicleCount % 2 == 0 ? 0 : 180);
            vehicleCount--;
            float wait = Random.Range(0.1f, 0.5f);
            yield return new WaitForSeconds(wait);
        }
    }
}
