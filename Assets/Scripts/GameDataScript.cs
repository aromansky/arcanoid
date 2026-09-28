using UnityEngine;
using System.Collections.Generic;

public enum BonusType { Plus100, Slow, Fast, Ball, Plus2, Plus10, Fire, Steel, Norm }

[System.Serializable]
public class BonusWeightEntry
{
    public BonusType type;
    public float weight = 10f;
}

[CreateAssetMenu(fileName = "Game Data", menuName = "Game Data", order = 51)]
public class GameDataScript : ScriptableObject
{
    public bool resetOnStart;
    public int level = 1;
    public int balls = 6;
    public int points = 0;
    public bool music = true;
    public bool sound = true;
    public int pointsToBall = 0;
    public GameObject bonusPrefab;
    public bool testlevels = false;

    [Header("Веса бонусов")]
    public List<BonusWeightEntry> bonusWeights = new List<BonusWeightEntry>
    {
        new BonusWeightEntry { type = BonusType.Plus100, weight = 20f },
        new BonusWeightEntry { type = BonusType.Slow,    weight = 15f },
        new BonusWeightEntry { type = BonusType.Fast,    weight = 15f },
        new BonusWeightEntry { type = BonusType.Ball,    weight = 15f },
        new BonusWeightEntry { type = BonusType.Plus2,   weight = 20f },
        new BonusWeightEntry { type = BonusType.Plus10,  weight = 15f },
        new BonusWeightEntry { type = BonusType.Fire,    weight = 15f },
        new BonusWeightEntry { type = BonusType.Steel,   weight = 15f },
        new BonusWeightEntry { type = BonusType.Norm,    weight = 20f },
    };

    public System.Type GetRandomBonusType()
    {
        if (testlevels)
        {
            switch (level)
            {
                case 1: return typeof(BonusBase);   // +100
                case 2: return typeof(BonusSlow);   // Slow
                case 3: return typeof(BonusFast);   // Fast
                case 4: return typeof(BonusBall);   // Ball
                case 5: return typeof(BonusPlus2);  // +2
                case 6: return typeof(BonusPlus10); // +10
                case 7: return typeof(BonusFire);   // fire balls
                case 8: return typeof(BonusSteel);  // steel balls
                case 9: return typeof(BonusNorm);   // norm
                default: return typeof(BonusBase);
            }
        }

        float totalWeight = 0f;
        foreach (var e in bonusWeights) totalWeight += e.weight;
        if (totalWeight <= 0f) return typeof(BonusBase);

        float rnd = Random.Range(0f, totalWeight);
        foreach (var e in bonusWeights)
        {
            if (rnd < e.weight) return BonusTypeToClass(e.type);
            rnd -= e.weight;
        }

        return typeof(BonusBase);
    }

    private System.Type BonusTypeToClass(BonusType t)
    {
        switch (t)
        {
            case BonusType.Slow: return typeof(BonusSlow);
            case BonusType.Fast: return typeof(BonusFast);
            case BonusType.Ball: return typeof(BonusBall);
            case BonusType.Plus2: return typeof(BonusPlus2);
            case BonusType.Plus10: return typeof(BonusPlus10);
            case BonusType.Fire: return typeof(BonusFire);
            case BonusType.Steel: return typeof(BonusSteel);
            case BonusType.Norm: return typeof(BonusNorm);
            default: return typeof(BonusBase); // Plus100
        }
    }

    public void CreateBonus(GameObject bonusObj)
    {
        System.Type bonusType = GetRandomBonusType();
        BonusBase bonus = (BonusBase)bonusObj.AddComponent(bonusType);
        bonus.gameData = this;
        bonus.Apply();
    }

    public void Reset()
    {
        level = 1;
        balls = 6;
        points = 0;
        pointsToBall = 0;
        BallScript.SetAllBallsType(BallScript.BallType.Normal);
    }

    public void Save()
    {
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("balls", balls);
        PlayerPrefs.SetInt("points", points);
        PlayerPrefs.SetInt("pointsToBall", pointsToBall);
        PlayerPrefs.SetInt("music", music ? 1 : 0);
        PlayerPrefs.SetInt("sound", sound ? 1 : 0);
    }

    public void Load()
    {
        level = PlayerPrefs.GetInt("level", 1);
        balls = PlayerPrefs.GetInt("balls", 6);
        points = PlayerPrefs.GetInt("points", 0);
        pointsToBall = PlayerPrefs.GetInt("pointsToBall", 0);
        music = PlayerPrefs.GetInt("music", 1) == 1;
        sound = PlayerPrefs.GetInt("sound", 1) == 1;
    }
}