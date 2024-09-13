using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{
    public float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var go = this.transform.forward * (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger));
        var back = -this.transform.forward * (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger));
        var up = this.transform.up * (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger));
        var down = -this.transform.up * (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger));

        var moveDir = (go + back + up + down);
        this.transform.Translate(moveDir * speed);

        if(OVRInput.Get(OVRInput.Button.One)) {
            this.transform.Rotate(Vector3.up * speed);
        }

        else if (OVRInput.Get(OVRInput.Button.Two))
        {
            this.transform.Rotate(-Vector3.up * speed);
        }

    }
}
