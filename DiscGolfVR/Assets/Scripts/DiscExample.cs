using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscExample : MonoBehaviour
{
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        this.rb.AddForce(Vector3.right * -200f);
    }
}
