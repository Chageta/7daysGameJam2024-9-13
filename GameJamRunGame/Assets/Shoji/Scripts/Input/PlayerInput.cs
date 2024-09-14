using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    CrowdControler crowd;
    [SerializeField]
    InputCommand command;
    bool commandMode = false;

    private void Awake()
    {
        BeginInput();
    }
    public void BeginInput()
    {
        StartCoroutine("ObserveModeChange");
    }
    public void EndInput()
    {
        StopAllCoroutines();
    }
    IEnumerator ObserveModeChange()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.anyKeyDown);
            if (!Input.GetKeyDown(KeyCode.Space)) continue;
            commandMode ^= true;
            if (commandMode)
            {
                crowd.EndControl();
                command.BeginCommand();
            }
            else
            {
                CrowdControlMode();
            }
        }
    }
    public void CrowdControlMode()
    {
        crowd.BeginControl();
        command.ForceEndCommand();
        commandMode = false;
    }
}
