using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputCommand : MonoBehaviour
{
    [SerializeField]
    UnityEvent OnCommandSuccess, OnCommandFailure;

    [SerializeField]
    CommandUI ui;

    int currentCommandIndex;
    int[] commands;
    bool commandFailureWait = false;
    readonly KeyCode[] kCommandKeys = new KeyCode[4]
    {KeyCode.W,KeyCode.S,KeyCode.A,KeyCode.D};
    
    public void BeginCommand()
    {
        commands = new int[3];//new int[DifficultyManager.Instance.CommandLength];
        for (int i = 0; i < commands.Length; i++)
        {
            commands[i] = Random.Range(0, 4);
        }
        currentCommandIndex = 0;
        ui.SetCommand(ref commands);
        ui.SetActive(true);

        StopAllCoroutines();
        StartCoroutine("CommandInput");
    }
    public void ForceEndCommand()
    {
        StopAllCoroutines();
        ui.SetActive(false);
    }
    IEnumerator CommandInput()
    {
        yield return new WaitWhile(() => commandFailureWait);
        while (currentCommandIndex < commands.Length)
        {
            yield return new WaitUntil(() => Input.anyKeyDown);
            if (!IsValidInput()) continue;

            if (Input.GetKeyDown(kCommandKeys[commands[currentCommandIndex]]))
            {
                ui.CommandHit(++currentCommandIndex);
            }
            else
            {
                OnCommandFailure.Invoke();
                commandFailureWait = true;
                yield break;
            }
        }
        ui.SetActive(false);
        OnCommandSuccess.Invoke();
    }
    public void EndCommandFailureWait()
    {
        commandFailureWait = false;
    }
    bool IsValidInput()
    {
        foreach (var key in kCommandKeys)
        {
            if (Input.GetKeyDown(key)) return true;
        }
        //Debug.Log("[Command]Input Not Valid");
        return false;
    }
}