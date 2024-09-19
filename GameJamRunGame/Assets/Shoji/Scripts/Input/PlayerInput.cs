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

    [SerializeField]
    PhoneWarn warn;

    public void BeginInput()
    {
        crowd.BeginMove();
        StartCoroutine(ObserveModeChange());
        warn.InitialWarn();
    }
    public void EndInput()
    {
        crowd.EndControl();
        crowd.Stop();
        command.ForceEndCommand();
        StopAllCoroutines();
        warn.EndWarn();
    }
    IEnumerator ObserveModeChange()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.anyKeyDown);
            if (!Input.GetKeyDown(KeyCode.Space)) continue;

            if (!commandMode)
            {
                if (!crowd.HasZombie) continue;
                crowd.EndControl();
                command.BeginCommand();
                commandMode = true;
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