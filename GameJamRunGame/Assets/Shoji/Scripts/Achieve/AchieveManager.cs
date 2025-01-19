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
        EasyDead = 0x0001,      //ながらタウンへようこそ(イージー全滅)
        EasyClear = 0x0002,     //よくやった！(イージークリア)
        NormalDead = 0x0004,    //痛い目を見た(ノーマル全滅)
        NormalClear = 0x0008,   //チュートリアルは終わり(ノーマルクリア)

        HardDead = 0x0010,      //右見て左見て...(ハード全滅)
        HardClear = 0x0020,     //歩きスマホは終わり(ハードクリア)
        NoProblem = 0x0040,     //俺が無事なら(20人以上死亡してクリア)
        RedCarpet = 0x0080,     //レッドカーペット(街の15%以上を血で染める)

        Family = 0x0100,        //大所帯(一人も死なずに30人以上でクリア)
        TomatoParty = 0x0200,   //トマトパーティー！(街の30%以上を血で染める)
        OutOfBounds = 0x0400,   //囚われの身(マップ外で全滅)
        SpeedStar = 0x0800,     //走りスマホ(ノーマル以上で1:30以内にクリアする)

        Score = 0x1000,         //熟練の技(20,000以上のスコアを取る)
        MegaScore = 0x2000,     //達人の域(40,000以上のスコアを取る)
        Gore = 0x4000,          //汚い花火(-5,000以下のスコアを取る)
        MoreGore = 0x8000,      //派手な花火(-20,000以下のスコアを取る)

        Spicy = 0x10000,        //スパイシー！(スパイシーチキンモードで全滅)
        SpicyHot = 0x20000,     //スパイシーホット！(スパイシーチキンモードで30人以上でクリア)
        SpicySpeed = 0x40000,   //スパイシースピード！(スパイシーチキンモードで1分以内にクリア)
        SpicyGore = 0x80000,    //こんがり！(スパイシーチキンモードで30人以上死亡、街の30%以上を血で染める)
    }
    static Achievement currentAchievement;

    public static AchieveUI UI;

    /// <summary>
    /// アチーブしていなければfalse, 既にアチーブしていたらtrueを返す
    /// </summary>
    /// <param name="achieve">アチーブさせたいもの</param>
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
    /// アチーブしていなければfalse, 既にアチーブしていたらtrueを返す
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
