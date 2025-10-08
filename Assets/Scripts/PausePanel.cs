using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    private static PausePanel instance;
    public static PausePanel Instance => instance;
    private PausePanel() { }

    public Button btnContinue;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        btnContinue.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            this.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            GamePanel.isPause = false;
        });
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
