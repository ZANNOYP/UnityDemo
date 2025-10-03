using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TipPanel : MonoBehaviour
{
    private static TipPanel instance;
    public static TipPanel Instance => instance;
    private TipPanel() { }

    public Text txtTip;
    public Button btnReturn;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        btnReturn.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
            SceneManager.LoadScene("BeginScene");
        });
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTip(string text)
    {
        txtTip.text = text + " ”Œœ∑Ω· ¯";
    }
}
