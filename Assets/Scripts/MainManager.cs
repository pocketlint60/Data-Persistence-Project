using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{

    public static MainManager Instance;
    public DataManager dManager;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text highScoreText;
    public Text ScoreText;
    public InputField nameEntry;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    public int h_Score;
    public string m_currentName;
    public string m_bestName;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        WriteHighScore();
        InitBricks();
        dManager.LoadBest();
    }

    void InitBricks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                nameEntry.gameObject.SetActive(false);
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void NameInput(string name)
    {
        m_currentName = nameEntry.text;
        name = m_currentName;
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";

        if (m_Points > h_Score)
        {
            h_Score = m_Points;
            m_bestName = m_currentName;
            WriteHighScore();
        }
    }

    public void WriteHighScore()
    {
        highScoreText.text = $"High Score : {h_Score} Name : {m_bestName}";
    }


    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        WriteHighScore();
        dManager.SaveBest();
    }
}
