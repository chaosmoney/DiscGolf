using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    public string ignoredLayer = "Basket";

    [SerializeField]
    private Rigidbody[] links;

    private void Start()
    {
        Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(ignoredLayer), true);
        links = GetComponentsInChildren<Rigidbody>();
        for (int i =0; i < links.Length; i++)
        {
            links[i].mass = 0.05f;
        }
    }
}
