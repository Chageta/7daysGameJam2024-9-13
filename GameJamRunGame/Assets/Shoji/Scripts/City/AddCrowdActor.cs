using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCrowdActor : MonoBehaviour
{
    [SerializeField]
    GameObject actorPrefab;
    const int kActorCount = 10;

    CrowdActor[] actors = new CrowdActor[kActorCount];

    private void Awake()
    {
        for (int i = 0; i < kActorCount; i++)
        {
            Vector3 actorPosition = transform.position;
            actorPosition += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            actors[i] = Instantiate(actorPrefab, actorPosition, Quaternion.Euler(0, Random.Range(0, 360), 0), transform).GetComponent<CrowdActor>();
        }
    }
    public void AddActors(CrowdControler crowd)
    {
        crowd.AddCrowdActors(actors);
        enabled = false;
    }
}
