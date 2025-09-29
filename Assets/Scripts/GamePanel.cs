using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    private static GamePanel instance;
    public static GamePanel Instance => instance;
    private GamePanel() { }

    public Text txtScore;

    private void Awake()
    {
        instance = this;
        txtScore.text = DataMgr.Instance.LoadScore().ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore()
    {
        txtScore.text = (int.Parse(txtScore.text) + 1).ToString();
        DataMgr.Instance.SaveScore(int.Parse(txtScore.text));
    }
}
