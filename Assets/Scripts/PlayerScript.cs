using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    const int maxLevel = 30;

    [Range(1, maxLevel)]
    public int level = 1;
    public float ballVolicityMult = 0.02f;
    public GameObject bluePrefab;
    public GameObject redPrefab;
    public GameObject yellowPrefab;
    public GameObject greenPrefab;
    public GameObject ballPrefab;
    static Collider2D[] colliders = new Collider2D [50];
    static ContactFilter2D contactFilter = new ContactFilter2D();
    public GameDataScript gameData;
    static bool gameStarted = false;
    AudioSource audioSrc;
    public AudioClip pointSound;
    int requiredPointsToBall {get {return 400 + (level - 1) * 20;}}
    void SetBackground()
    {
        SpriteRenderer bg = GameObject.Find("Background").GetComponent<SpriteRenderer>();
        bg.sprite = Resources.Load(level.ToString("d2"), typeof(Sprite)) as Sprite;
    }

    void CreateBlocks(GameObject prefab, float xMax, float yMax, int count, int maxCount)
    {
        if (count > maxCount)
            count = maxCount;

        for(int i = 0; i < maxCount; i++)
        {
            for (int k = 0; k < 20; k++)
            {
                GameObject obj = Instantiate(prefab, new Vector3((Random.value * 2 - 1) * xMax,
                                                                  Random.value * yMax, 0),
                                                                Quaternion.identity);
                
                if (obj.GetComponent<Collider2D>().OverlapCollider(contactFilter.NoFilter(), colliders) == 0)
                    break;
                Destroy(obj);

            }
        }
    }

    void CreateBalls()
    {
        int count = 2;

        if  (gameData.balls == 1)
            count = 1;

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(ballPrefab);
            BallScript ball = obj.GetComponent<BallScript>();
            ball.ballInitialForce += new Vector2(10 * i, 0);
            ball.ballInitialForce *= 1 + level + ballVolicityMult;
        }
    }

    void StartLevel()
    {
        SetBackground();
        float yMax = Camera.main.orthographicSize * 0.8f;
        float xMax = Camera.main.orthographicSize * Camera.main.aspect * 0.85f;

        CreateBlocks(bluePrefab, xMax, yMax, level, 8);
        CreateBlocks(redPrefab, xMax, yMax, level + 1, 10);
        CreateBlocks(greenPrefab, xMax, yMax, level + 1, 12);
        CreateBlocks(yellowPrefab, xMax, yMax, level + 2, 15);

        CreateBalls();
    }

    void Start()
    {
        audioSrc = Camera.main.GetComponent<AudioSource>();
        Cursor.visible = false;

        if (!gameStarted)
        {
            gameStarted = true;
            if (gameData.resetOnStart)
                gameData.Reset();
        }

        level = gameData.level;
        
        SetMusic();
        StartLevel();
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 pos = transform.position;
            pos.x = mousePos.x;
            transform.position = pos;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            gameData.music = !gameData.music;
            SetMusic();
        }

        if (Input.GetKeyDown(KeyCode.S))
            gameData.sound = !gameData.sound;
        
        if (Input.GetButtonDown("Pause"))
        {
            if (Time.timeScale > 0)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }
    }

    public void BallDestroyed()
    {
        gameData.balls--;
        StartCoroutine(BallDestroyedCoroutine());
    }

    IEnumerator BallDestroyedCoroutine()
    {
        yield return new WaitForSeconds(0.01f);
        if (GameObject.FindGameObjectsWithTag("Ball").Length == 0)
            if (gameData.balls > 0)
                CreateBalls();
            else
            {
                gameData.Reset();
                SceneManager.LoadScene("MainScene");
            }
    }

    public void BlockDestroyed(int points)
    {
        gameData.points += points;

        if (gameData.sound)
            audioSrc.PlayOneShot(pointSound, 5);

        gameData.pointsToBall += points;
        if (gameData.pointsToBall >= requiredPointsToBall)
        {
            gameData.balls++;
            gameData.pointsToBall -= requiredPointsToBall;

            if (gameData.sound)
                StartCoroutine(BlockDestroyedCoroutine2());
        }
        StartCoroutine(BlockDestroyedCoroutine());
    }
    
    IEnumerator BlockDestroyedCoroutine()
    {
        yield return new WaitForSeconds(0.01f);
        if (GameObject.FindGameObjectsWithTag("Ball").Length == 0)
        {
            if (level < maxLevel)
                gameData.level++;
            SceneManager.LoadScene("MainScene");
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(5, 4, Screen.width - 10, 100),
            string.Format(
                        "<color=yellow><size=30>Level <b>{0}</b> Balls <b>{1}</b>"+
                        " Score <b>{2}</b></size></color>",
                        gameData.level, gameData.balls, gameData.points));
    }

    void SetMusic()
    {
        if (gameData.music)
            audioSrc.Play();
        else
            audioSrc.Stop();
    }

    IEnumerator BlockDestroyedCoroutine2()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.2f);
            audioSrc.PlayOneShot(pointSound, 5);
        }
    }
 

}
