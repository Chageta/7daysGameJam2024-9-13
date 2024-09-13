using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommandUI : MonoBehaviour
{
    [SerializeField]
    InputCommand inputCommand;
    [SerializeField]
    CanvasGroup commandGroup;
    [SerializeField]
    TMP_Text commandText;

    string commandString;
    readonly char[] commandKeys = new char[4]
        {'Å™','Å´','Å©','Å®' };

    public void SetCommand(ref int[] newCommands)
    {
        commandString = "";
        foreach (var command in newCommands)
        {
            commandString += commandKeys[command];
        }
        commandText.text = commandString;
    }
    public void SetActive(bool active)
    {
        commandGroup.alpha = active ? 1 : 0;
    }
    public void CommandHit(int index)
    {
        commandText.text = $"<#ffff00>{commandString[..index]}<#ffffff>{commandString[index..]}";
    }
    public void CommandFailure()
    {
        commandText.text = $"<#ff0000>{commandString}";
        StopAllCoroutines();
        StartCoroutine("FailureShake");
    }
    IEnumerator FailureShake()
    {
        float shakeTime = 0.75f;
        while (shakeTime>0)
        {
            Vector2 shakePosition = Mathf.Clamp01(shakeTime * 4) * new Vector2(Mathf.Sin(Time.timeSinceLevelLoad * 7) * Random.Range(-6, 6), Mathf.Sin(Time.time * 5) * Random.Range(-6, 6));
            commandText.rectTransform.anchoredPosition = shakePosition;
            yield return null;
            shakeTime -= Time.deltaTime;
        }
        commandText.rectTransform.anchoredPosition = Vector2.zero;
        inputCommand.EndCommandFailureWait();
        if (commandGroup.alpha <= 0) yield break;
        inputCommand.BeginCommand();
    }
}