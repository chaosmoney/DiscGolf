using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleMain : MonoBehaviour
{
    public Button btn;
    public System.Action onLoadLobbyScene;

    public Transform player;
    public Transform playerCamera;
    public Transform targetCamera;
    public Transform title;
    public TMP_Text pull;

    public OVRInput.Controller controller;

    private bool isSceneChange = false;

    private void Update()
    {
        this.player.position = this.targetCamera.position;
        this.title.position = this.targetCamera.position + Vector3.forward * 20f;

        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, controller) || OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger, controller))
        {
            if(isSceneChange == false)
            {
                onLoadLobbyScene();
                isSceneChange = true;
            }
        }

        if(Time.time % 6 >0 && Time.time % 6 <= 2)
        {
            this.pull.alpha -= Time.deltaTime / 2f;
        }
        else if(Time.time % 6 >2 && Time.time % 6 <= 4) 
        {
            this.pull.alpha += Time.deltaTime / 2f;
        }
        else if(Time.time%6 > 4 )
        {
            
        }
    }
}
