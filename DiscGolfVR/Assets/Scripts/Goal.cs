using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    
    public Action DiscGoal;
    private ParticleSystem effect;

    private void Start()
    {
        this.effect = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Disc")
        {
            //Debug.Log("Goal");
            other.GetComponent<Rigidbody>().useGravity = true;
            this.effect.Play();
            DiscGoal();
            var v = this.gameObject.GetComponents<Collider>();
            //Debug.Log(v.Length);
            v[0].enabled = false;
            v[1].enabled = false;
            //this.gameObject.SetActive(false);
        }
    }

}