using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public Vector2 ballInitialForce;
    Rigidbody2D rb;
    GameObject playerObj;
    float deltaX;
    AudioSource audioSrc;
    public AudioClip hitSound;
    public AudioClip loseSound;
    public GameDataScript gameData;

    public static int powerOfHit = 1;
    public enum BallType { Normal, Fire, Steel }
    public static BallType currentType = BallType.Normal;

    SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        deltaX = transform.position.x;
        audioSrc = Camera.main.GetComponent<AudioSource>();

        sr = GetComponent<SpriteRenderer>();

        ApplyType(currentType);
    }

    void Update()
    {
        if (rb.isKinematic)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                rb.isKinematic = false;
                rb.AddForce(ballInitialForce);
            }
            else
            {
                var pos = transform.position;
                pos.x = playerObj.transform.position.x + deltaX;
                transform.position = pos;
            }
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            Vector2 v = rb.velocity;
            if (Random.Range(0, 2) == 0)
                v.Set(v.x - 0.1f, v.y + 1f);
            else
                v.Set(v.x + 0.1f, v.y - 1f);
            rb.velocity = v;            
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameData.sound)
            audioSrc.PlayOneShot(loseSound, 5);
        Destroy(gameObject);
        playerObj.GetComponent<PlayerScript>().BallDestroyed();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameData.sound)
            audioSrc.PlayOneShot(hitSound, 5);
    }

    public static void SetAllBallsType(BallType type)
    {
        currentType = type;

        switch (type)
        {
            case BallType.Normal: powerOfHit = 1; break;
            case BallType.Fire: powerOfHit = 4; break;
            case BallType.Steel: powerOfHit = 40; break;
        }

        foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
        {
            BallScript bs = ball.GetComponent<BallScript>();
            if (bs != null) bs.ApplyType(type);
        }
    }

    public void ApplyType(BallType type)
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        switch (type)
        {
            case BallType.Normal: sr.color = Color.white; break;
            case BallType.Fire: sr.color = new Color(1f, 0.5f, 0f, 1f); break;
            case BallType.Steel: sr.color = Color.gray; break;
        }
    }
}
