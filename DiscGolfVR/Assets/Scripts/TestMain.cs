using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMain : MonoBehaviour
{
    [SerializeField]
    private GameObject leftController;
    [SerializeField]
    private GameObject rightController;
    [SerializeField] 
    private OVRInput.Controller controller;

    private int indexTriggerValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 thumbstickValue =
            OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller);
        int indexTriggerValue = (int)OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller);
        Debug.Log("indexTriggerValue : " + this.indexTriggerValue);
        Debug.Log("left : " + this.leftController.transform.position);
        Debug.Log("right : " + this.rightController.transform.position);        
    }
}
