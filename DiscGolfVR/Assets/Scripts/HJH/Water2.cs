using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water2 : MonoBehaviour
{
    public System.Action InWater;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Disc")
        {
            InWater();
        }
    }
}
