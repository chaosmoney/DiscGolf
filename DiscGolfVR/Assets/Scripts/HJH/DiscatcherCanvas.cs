using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class DiscatcherCanvas : MonoBehaviour
{
    [SerializeField]
    public TMP_Text textDistance;
    [SerializeField]
    private GameObject Panel;
    [SerializeField]
    public GameObject player;

    private Transform tr;
    private float dis = 6;
    private Vector3 startScale;
    // Start is called before the first frame update
    void Start()
    {                
        this.tr = this.transform;
        this.tr.position = this.transform.position;
        this.startScale = this.Panel.transform.localScale;
        StartCoroutine(CoLook());

    }
    private IEnumerator CoLook()
    {
        while (true)
        {
            float distance = (float)Math.Round(Vector3.Distance(this.transform.position, this.player.transform.position), 2);
            this.textDistance.text = distance.ToString();        
            this.gameObject.transform.LookAt(new Vector3(this.player.transform.position.x,this.transform.position.y, this.player.transform.position.z));
            Vector3 newScale = startScale * distance / dis;

            this.Panel.transform.localScale = newScale;
            yield return null;
        }
        
    }
    public void LookPlayer()
    {
        StartCoroutine(CoLook());
    }
}
