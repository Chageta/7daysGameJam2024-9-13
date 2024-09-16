using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveEvent : MonoBehaviour
{
    [SerializeField]
    UnityEvent OnCrowdEnter;
    
    private void OnTriggerEnter(Collider hit)
    {
        OnCrowdEnter.Invoke();
        gameObject.SetActive(false);
    }
}
