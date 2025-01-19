using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cross : MonoBehaviour
{
    bool[] lights = new bool[4] { false, false, false, false };
    [SerializeField]
    ParticleSystem[] lightParticles;
    int GetTriggerIndex(Transform trigger)
    {
        for (int i = 0; i < 4; i++)
        {
            if (transform.GetChild(i) != trigger) continue;
            return i;
        }
        return 0;
    }
    public void BeginLights(Transform trigger)
    {
        int index = GetTriggerIndex(trigger);
        lights[index] = true;
        CalcLights();
    }
    public void EndLights(Transform trigger)
    {
        int index = GetTriggerIndex(trigger);
        lights[index] = false;
        CalcLights();
    }
    void CalcLights()
    {
        bool[] lightCalced = new bool[2];
        for (int i = 0; i < 4; i++)
        {
            lightCalced[i % 2] |= lights[i];
        }
        lightParticles[0].gameObject.SetActive(lightCalced[0]);
        lightParticles[1].gameObject.SetActive(lightCalced[1]);
    }
}
