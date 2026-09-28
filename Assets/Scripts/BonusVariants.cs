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
            player.CreateBalls(2, bonus: true);
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
            player.CreateBalls(10, bonus: true);
    }
}

public class BonusFire : BonusBase
{
    protected override void Awake()
    {
        base.Awake();
        text = "Fire";
        textColor = Color.black;
        backgroundColor = new Color(1.0f, 0.5f, 0.0f, 1.0f);
    }

    public override void BonusActivate()
    {
        BallScript.SetAllBallsType(BallScript.BallType.Fire);
    }
}

public class BonusSteel : BonusBase
{
    protected override void Awake()
    {
        base.Awake();
        text = "Steel";
        textColor = Color.black;
        backgroundColor = Color.gray;
    }

    public override void BonusActivate()
    {
        BallScript.SetAllBallsType(BallScript.BallType.Steel);
    }
}

public class BonusNorm : BonusBase
{
    protected override void Awake()
    {
        base.Awake();
        text = "Norm";
        textColor = Color.black;
        backgroundColor = Color.white;
    }

    public override void BonusActivate()
    {
        BallScript.SetAllBallsType(BallScript.BallType.Normal);
    }
}
