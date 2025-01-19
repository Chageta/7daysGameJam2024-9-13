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

    static bool useArrowKey = false;
    static readonly KeyCode[] keys1 = new KeyCode[4]
    {KeyCode.W,KeyCode.S,KeyCode.A,KeyCode.D};
    static readonly KeyCode[] keys2 = new KeyCode[4]
    {KeyCode.UpArrow,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.RightArrow};
    public static KeyCode[] Keys => useArrowKey ? keys2 : keys1;
    public static KeyCode[] Keys1 => keys1;
    public static KeyCode[] Keys2 => keys2;
    public static bool UseArrowKey { get => useArrowKey; set => useArrowKey = value; }

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