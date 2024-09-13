using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseCanvas : MonoBehaviour
{
    [SerializeField]
    private Button btnOption;
    [SerializeField]
    private Button btnRestart;
    [SerializeField]
    private Button btnQuit;

    public System.Action Option;
    public System.Action ReStart;
    public System.Action Quit;
    private void Start()
    {
        this.btnOption.onClick.AddListener(() =>
        {
            this.Option();
        });

        this.btnRestart.onClick.AddListener(() =>
        {
            this.ReStart();
        });
        
        this.btnQuit.onClick.AddListener(() =>
        {
            this.Quit();
        });
    }
}
