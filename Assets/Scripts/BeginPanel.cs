using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BeginPanel : MonoBehaviour
{
    private static BeginPanel instance;
    public static BeginPanel Instance => instance;
    private BeginPanel() { }

    public Button btnStart;
    public Button btnQuit;

    private void Awake()
    {
        Time.timeScale = 1;
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        btnStart.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene");
        });
        btnQuit.onClick.AddListener(() =>
        {
            UnityEditor.EditorApplication.isPlaying = false;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
