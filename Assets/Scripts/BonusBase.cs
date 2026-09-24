using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AI;

public class BonusBase : MonoBehaviour
{
    public TMP_Text textTMP;
    public Color textColor = Color.black;
    public Color backgroundColor = Color.yellow;
    public string text = "+100"; 
    public Vector2 bonusInitialForce = new Vector2(0, -200);
    
    public GameDataScript gameData;

    void Awake()
    {
        textTMP = GetComponentInChildren<TMP_Text>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        BonusActivate();
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    public void Apply()
    {
        textTMP.text = text;
        textTMP.color = textColor;
        GetComponent<SpriteRenderer>().color = backgroundColor;

        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        if (rigidbody2D.velocity == Vector2.zero)
            rigidbody2D.AddForce(bonusInitialForce);
    }

    public virtual void BonusActivate()
    {
        gameData.points += 100;
    }
}
