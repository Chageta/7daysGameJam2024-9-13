using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneIcon : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }
}
