using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    public static string Path => Application.persistentDataPath + "/Achieve.txt";

    static readonly int kNumberOfAchieve = 20;
    public static int NumberOfAchieve => kNumberOfAchieve;
    public enum Achievement
    {
        EasyDead = 0x0001,      //�Ȃ���^�E���ւ悤����(�C�[�W�[�S��)
        EasyClear = 0x0002,     //�悭������I(�C�[�W�[�N���A)
        NormalDead = 0x0004,    //�ɂ��ڂ�����(�m�[�}���S��)
        NormalClear = 0x0008,   //�`���[�g���A���͏I���(�m�[�}���N���A)

        HardDead = 0x0010,      //�E���č�����...(�n�[�h�S��)
        HardClear = 0x0020,     //�����X�}�z�͏I���(�n�[�h�N���A)
        NoProblem = 0x0040,     //���������Ȃ�(20�l�ȏ㎀�S���ăN���A)
        RedCarpet = 0x0080,     //���b�h�J�[�y�b�g(�X��15%�ȏ�����Ő��߂�)

        Family = 0x0100,        //�及��(��l�����Ȃ���30�l�ȏ�ŃN���A)
        TomatoParty = 0x0200,   //�g�}�g�p�[�e�B�[�I(�X��30%�ȏ�����Ő��߂�)
        OutOfBounds = 0x0400,   //�����̐g(�}�b�v�O�őS��)
        SpeedStar = 0x0800,     //����X�}�z(�m�[�}���ȏ��1:30�ȓ��ɃN���A����)

        Score = 0x1000,         //�n���̋Z(20,000�ȏ�̃X�R�A�����)
        MegaScore = 0x2000,     //�B�l�̈�(40,000�ȏ�̃X�R�A�����)
        Gore = 0x4000,          //�����ԉ�(-5,000�ȉ��̃X�R�A�����)
        MoreGore = 0x8000,      //�h��ȉԉ�(-20,000�ȉ��̃X�R�A�����)

        Spicy = 0x10000,        //�X�p�C�V�[�I(�X�p�C�V�[�`�L�����[�h�őS��)
        SpicyHot = 0x20000,     //�X�p�C�V�[�z�b�g�I(�X�p�C�V�[�`�L�����[�h��30�l�ȏ�ŃN���A)
        SpicySpeed = 0x40000,   //�X�p�C�V�[�X�s�[�h�I(�X�p�C�V�[�`�L�����[�h��1���ȓ��ɃN���A)
        SpicyGore = 0x80000,    //���񂪂�I(�X�p�C�V�[�`�L�����[�h��30�l�ȏ㎀�S�A�X��30%�ȏ�����Ő��߂�)
    }
    static Achievement currentAchievement;

    public static AchieveUI UI;

    /// <summary>
    /// �A�`�[�u���Ă��Ȃ����false, ���ɃA�`�[�u���Ă�����true��Ԃ�
    /// </summary>
    /// <param name="achieve">�A�`�[�u������������</param>
    /// <returns></returns>
    public static bool Achieve(Achievement achieve)
    {
        if (IsAlreadyAchieved(achieve)) return true;
        currentAchievement |= achieve;

        int num = ConvertAchieveEnumToInt(achieve);
        UI.ActiveAchieveUI(num);
        return false;
    }
    /// <summary>
    /// �A�`�[�u���Ă��Ȃ����false, ���ɃA�`�[�u���Ă�����true��Ԃ�
    /// </summary>
    /// <param name="achieve"></param>
    /// <returns></returns>
    public static bool IsAlreadyAchieved(Achievement achieve)
    {
        if ((currentAchievement & achieve) == achieve) return true;
        return false;
    }
    public static void SaveAchieve()
    {
        using (var fs = new StreamWriter(Path))
        {
            fs.Write(((int)currentAchievement).ToString());
        }
    }
    public static void LoadAchieve()
    {
        if (!File.Exists(Path)) return;

        string save;
        using (var fs = new StreamReader(Path))
        {
            save = fs.ReadToEnd();
        }
        int data = int.Parse(save);
        for (int i = 0; i < kNumberOfAchieve; i++)
        {
            if ((data & (1 << i)) == 0) continue;
            Achieve(ConvertIntToAchieveEnum(i));
        }
    }

    public static int ConvertAchieveEnumToInt(Achievement achieve)
    {
        int num = 0;
        uint aciv = (uint)achieve;
        for (int i = 0; i < kNumberOfAchieve; i++)
        {
            if (aciv == Mathf.Pow(2, i))
            {
                num = i;
                break;
            }
        }
        return num;
    }
    public static Achievement ConvertIntToAchieveEnum(int value)
    {
        return (Achievement)Mathf.Pow(2, value);
    }
}
