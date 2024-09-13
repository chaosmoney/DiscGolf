using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ScoreCanvas : MonoBehaviour
{
    [SerializeField]
    private Button btnRestart;
    [SerializeField]
    private Button btnNextHole;
    [SerializeField]
    private Button btnQuit;
    
    public TMP_Text[] textHoles;
    public TMP_Text textTotal;
    public TMP_Text textPar;

    public Action ReStart;
    public Action NextHole;
    public Action Quit;
    private void Start()
    {
        this.btnRestart.onClick.AddListener(() =>
        {
            //Debug.Log("btnRestart");
            this.ReStart();
        });
        this.btnNextHole.onClick.AddListener(() =>
        {
            //Debug.Log("btnNextHole");
            this.NextHole();
        });
        this.btnQuit.onClick.AddListener(() =>
        {
            //Debug.Log("btnQuit");
            this.Quit();
        });
    }
}
