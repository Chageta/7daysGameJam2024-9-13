using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneWarn : MonoBehaviour
{
    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip warnClip;
    [SerializeField]
    Animator warnAnim;

    public void InitialWarn()
    {
        if (DifficultyManager.Instance.Difficulty != 0) return;
        Invoke("Warn", DifficultyManager.Instance.CommandMinWait - 2);
    }
    public void BeginWarn()
    {
        if (DifficultyManager.Instance.Difficulty != 0) return;
        Invoke("Warn", DifficultyManager.Instance.CommandMinWait - 1);
    }
    public void EndWarn()
    {
        CancelInvoke();
    }
    void Warn()
    {
        source.PlayOneShot(warnClip);
        warnAnim.SetTrigger("Warn");
    }
}
