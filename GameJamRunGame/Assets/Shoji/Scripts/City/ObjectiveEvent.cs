using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveEvent : MonoBehaviour
{
    [SerializeField]
    UnityEvent OnCrowdEnter;
    [SerializeField]
    Collider col;
    [SerializeField]
    ObjectiveIndicator indicator;
    [SerializeField]
    GameObject effect;

    private void OnTriggerEnter(Collider hit)
    {
        OnCrowdEnter.Invoke();
        KillObjective();
    }
    public void SetEnable(bool enable)
    {
        col.enabled = enable;
        indicator.SetEnable(enable);
        effect.SetActive(enable);
    }
    public void KillObjective()
    {
        gameObject.SetActive(false);
        if (indicator != null) indicator.gameObject.SetActive(false);
    }
}
