using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public GameObject textObj;
    TMP_Text textComponent;
    public int hitsToDestroy;
    public int points;
    void Start()
    {
        if (textObj != null)
        {
            textComponent = textObj.GetComponent<TMP_Text>();
            textComponent.text = hitsToDestroy.ToString();
        }
    }

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        hitsToDestroy--;
        if (hitsToDestroy == 0)
        {
            print(points);
            Destroy(gameObject);
        }
        else if (textComponent != null)
        {
            textComponent.text = hitsToDestroy.ToString();
        }
    }
}
