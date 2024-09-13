using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discatcher : MonoBehaviour
{
    public Action Goal;    

    private void OnCollisionEnter(Collision collision)
    {
        // ∞Ò¿Œ
        if(collision.gameObject.tag == "Disc")
        {
            this.Goal();
        }
    }
}
