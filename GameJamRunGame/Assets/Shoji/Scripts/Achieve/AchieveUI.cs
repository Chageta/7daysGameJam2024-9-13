using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchieveUI : MonoBehaviour
{
    public bool ExternalStop;

    readonly string[] NameText = new string[]
    {
        "�Ȃ���^�E���ւ悤����",
        "�悭������I",
        "�ɂ��ڂ�����",
        "�`���[�g���A���͏I���",
        "�E���č�����...",
        "�����X�}�z�͏I���",
        "���������Ȃ�",
        "���b�h�J�[�y�b�g",
        "�及��",
        "�g�}�g�p�[�e�B�[�I",
        "�����̐g",
        "����X�}�z",
        "�n���̋Z",
        "�B�l�̈�",
        "�����ԉ�",
        "�h��ȉԉ�",
        "�X�p�C�V�[�I",
        "�X�p�C�V�[�z�b�g�I",
        "�X�p�C�V�[�X�s�[�h�I",
        "���񂪂�I"
    };
    [SerializeField]
    TMP_Text achieveName;
    [SerializeField]
    Animator anim;
    [SerializeField]
    AudioSource source;
    private void Awake()
    {
        AchieveManager.UI = this;

        ExternalStop = true;
        AchieveManager.LoadAchieve();
        ExternalStop = false;
    }
    public void ActiveAchieveUI(int num)
    {
        if (ExternalStop) return;
        achieveName.text = NameText[num];
        CancelInvoke();
        Invoke(nameof(StartUI), 1);
    }
    void StartUI()
    {
        anim.Play("AchieveAnim", 0, 0);
        source.Play();
    }
}
