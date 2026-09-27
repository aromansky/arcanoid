using UnityEngine;


public class BonusSlow : BonusBase
{
    protected override void Awake()
    {
        base.Awake();
        text = "Slow";
        textColor = Color.white;
        backgroundColor = Color.green;
    }

    public override void BonusActivate()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity *= 0.7f;
        }
    }
}

public class BonusFast : BonusBase
{
    protected override void Awake()
    {
        base.Awake();
        text = "Fast";
        textColor = Color.white;
        backgroundColor = Color.red;
    }

    public override void BonusActivate()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity *= 1.3f;
        }
    }
}

public class BonusBall : BonusBase
{
    protected override void Awake()
    {
        base.Awake();
        text = "Ball";
        textColor = Color.white;
        backgroundColor = Color.green;
    }

    public override void BonusActivate()
    {
        gameData.balls++;
    }
}

public class BonusPlus2 : BonusBase
{
    protected override void Awake()
    {
        base.Awake();
        text = "+2";
        textColor = Color.white;
        backgroundColor = Color.blue;
    }

    public override void BonusActivate()
    {
        gameData.balls += 2;
        PlayerScript player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        if (player != null)
            player.CreateBalls(2);
    }
}

public class BonusPlus10 : BonusBase
{
    protected override void Awake()
    {
        base.Awake();
        text = "+10";
        textColor = Color.white;
        backgroundColor = Color.blue;
    }

    public override void BonusActivate()
    {
        gameData.balls += 10;
        PlayerScript player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        if (player != null)
            player.CreateBalls(10);
    }
}