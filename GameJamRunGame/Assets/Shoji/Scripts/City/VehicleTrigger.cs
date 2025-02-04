using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleTrigger : MonoBehaviour
{
    [SerializeField]
    Cross cross;

    [SerializeField]
    int spawnDistance;
    [SerializeField]
    Vehicle vehiclePrefab;
    bool coolTime = false;
    [SerializeField]
    bool outer;

    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip[] carDriveBy;

    const float kForceKillTime = 6;
    [SerializeField]
    SphereCollider sphere;

    private void OnTriggerEnter(Collider hit)
    {
        if (coolTime) return;
        if (hit.gameObject.layer != 9) return;
        CrowdActor actor = hit.GetComponent<CrowdActor>();
        Invoke(nameof(ForceKillScan), kForceKillTime);
        if ((!actor.IsTexting && !outer) || actor.IsDead) return;

        BeginSpawnVehicles();
    }
    void BeginSpawnVehicles()
    {
        coolTime = true;
        StartCoroutine(CoolTime());
        StartCoroutine(SpawnVehicles());
    }
    void ForceKillScan()
    {
        CancelInvoke();
        Collider[] result = new Collider[1];
        Physics.OverlapSphereNonAlloc(transform.position, sphere.radius, result, 1 << 9);
        if (result[0] == null) return;

        StopAllCoroutines();
        BeginSpawnVehicles();
    }
    IEnumerator CoolTime()
    {
        cross.BeginLights(transform);
        yield return new WaitForSeconds(3);
        coolTime = false;
        cross.EndLights(transform);
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
            if (vehicleCount % 2 == 0) source.PlayOneShot(carDriveBy[Random.Range(0, carDriveBy.Length)]);
            vehicleCount--;
            float wait = Random.Range(0.1f, 0.5f);
            yield return new WaitForSeconds(wait);
        }
    }
}
