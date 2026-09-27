using UnityEngine;

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

    [Header("¬еро€тности выпадени€ бонусов (веса)")]
    public float weightPlus100 = 20f;
    public float weightSlow = 15f;
    public float weightFast = 15f;
    public float weightBall = 15f;
    public float weightPlus2 = 20f;
    public float weightPlus10 = 15f;

    public System.Type GetRandomBonusType()
    {
        // ≈сли включен режим тестовых уровней Ч гарантируем конкретный бонус на каждом уровне
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
                default: return typeof(BonusBase);
            }
        }

        // —тандартный расчет по весам
        float totalWeight = weightPlus100 + weightSlow + weightFast + weightBall + weightPlus2 + weightPlus10;
        if (totalWeight <= 0) return typeof(BonusBase);

        float rnd = Random.Range(0, totalWeight);

        if (rnd < weightPlus100) return typeof(BonusBase);
        rnd -= weightPlus100;

        if (rnd < weightSlow) return typeof(BonusSlow);
        rnd -= weightSlow;

        if (rnd < weightFast) return typeof(BonusFast);
        rnd -= weightFast;

        if (rnd < weightBall) return typeof(BonusBall);
        rnd -= weightBall;

        if (rnd < weightPlus2) return typeof(BonusPlus2);

        return typeof(BonusPlus10);
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