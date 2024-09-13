using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class TeleportAreaCanvas : MonoBehaviour
{
    [SerializeField]
    private GameObject Panel;
    [SerializeField]
    public GameObject player;
    
    private Vector3 startScale;
    private float dis = 12f;
    private void Start()
    {
        this.startScale = this.Panel.transform.localScale;
    }
    public void SetScale()
    {
        float distance = (float)Math.Round(Vector3.Distance(this.transform.position, this.player.transform.position), 2);
        this.gameObject.transform.LookAt(new Vector3(this.player.transform.position.x, this.transform.position.y, this.player.transform.position.z));
        Vector3 newScale = startScale * distance / dis;        
        this.Panel.transform.localScale = newScale;
    }
}
