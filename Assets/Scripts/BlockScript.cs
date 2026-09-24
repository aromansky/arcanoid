using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public enum BlockTypes
    {
        Red,
        Green,
        Blue,
        Yellow
    }

    public GameObject textObj;
    TMP_Text textComponent;
    public int hitsToDestroy;
    public int points;
    PlayerScript playerScript;

    public BlockTypes blockType;

    void Start()
    {
        if (textObj != null)
        {
            textComponent = textObj.GetComponent<TMP_Text>();
            textComponent.text = hitsToDestroy.ToString();
        }

        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        hitsToDestroy--;
        if (hitsToDestroy == 0)
        {
            if (blockType == BlockTypes.Green)
            {
                GameDataScript gameData = playerScript.gameData;
                GameObject obj = Instantiate(gameData.bonusPrefab, transform.position, Quaternion.identity);
                BonusBase bonus = obj.AddComponent<BonusBase>();
                bonus.gameData = gameData;
                bonus.Apply();
            }

            Destroy(gameObject);
            playerScript.BlockDestroyed(points);            
        }
        else if (textComponent != null)
        {
            textComponent.text = hitsToDestroy.ToString();
        }
    }
}
