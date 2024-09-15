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
        hit.GetComponent<CrowdControler>().Stop();
        OnCrowdEnter.Invoke();
        gameObject.SetActive(false);
    }
}
