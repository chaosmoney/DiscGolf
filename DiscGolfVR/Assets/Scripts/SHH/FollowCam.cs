using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FollowTarget());
    }

    private IEnumerator FollowTarget()
    {
        while (true)
        {
            this.transform.position = target.position - target.forward*5 + target.up*3;
            this.transform.LookAt(target.position);
            yield return null;
        }
        
    }
}
