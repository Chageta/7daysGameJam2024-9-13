using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKeyAuto : MonoBehaviour
{
    [SerializeField]
    Animator anim;
    void Start()
    {
        StartCoroutine(ObserveKeyChange());
    }
    IEnumerator ObserveKeyChange()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.anyKeyDown);
            if (!IsKeyChange()) continue;
            PlayerInput.UseArrowKey ^= true;
            anim.SetTrigger("KeyChange");
        }
    }
    bool IsKeyChange()
    {
        KeyCode[] keys = PlayerInput.UseArrowKey ? PlayerInput.Keys1 : PlayerInput.Keys2;

        foreach (var key in keys)
        {
            if (!Input.GetKeyDown(key)) continue;
            return true;
        }
        return false;
    }
}
