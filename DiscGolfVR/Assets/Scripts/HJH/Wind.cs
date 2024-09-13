using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField]
    private float windForce = 0f;
    private void OnTriggerStay(Collider other)
    {
        var hitObj = other.gameObject;
        if (hitObj != null && hitObj.tag == "Disc")
        {
            var rb = hitObj.GetComponent<Rigidbody>();
            var dir = this.transform.up;
            rb.AddForce(dir * windForce);
        }
    }
}

