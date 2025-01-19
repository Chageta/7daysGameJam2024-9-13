using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdScreamer : MonoBehaviour
{
    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip[] screams, collideClips, hornClips;

    static CrowdScreamer instance;
    public static CrowdScreamer Instance => instance;

    int screamCount = 0;
    private void Awake()
    {
        instance = this;
    }
    public void Scream(bool hasHone = true)
    {
        if (screamCount >= 5) return;
        if (screamCount == 0) Invoke("ScreamResetWait", 3);
        screamCount++;
        source.PlayOneShot(screams[Random.Range(0, screams.Length)]);
        source.PlayOneShot(collideClips[Random.Range(0, collideClips.Length)]);
        if (!hasHone) return;
        source.PlayOneShot(hornClips[Random.Range(0, hornClips.Length)]);
    }
    void ScreamResetWait()
    {
        screamCount = 0;
    }
}
