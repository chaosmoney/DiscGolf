using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CinemaMountain : MonoBehaviour
{
    public Transform[] point;
    public PlayableDirector playableDirector;
    public TimelineAsset timeline;
    public Transform carTr;
    public Transform startPos;

    private bool isEnd = false;

    public void MoveToPoint1()
    {
        this.transform.position = point[0].position + Vector3.up;
        this.transform.rotation = Quaternion.Euler(0, 135, 0);
    }

    public void MoveToPoint2()
    {
        this.transform.position = point[1].position + Vector3.up;
        this.transform.rotation = Quaternion.Euler(0, 40, 0);
    }

    public void MoveToPoint3()
    {
        //StartCoroutine(CoRideCar());
        carTr.gameObject.SetActive(true);
        this.transform.SetParent(carTr);
        this.transform.position = carTr.position - Vector3.up / 1.5f;
    }

    public void MoveToPoint4()
    {
        this.transform.SetParent(null);
        //isEnd = true;
    }

    //private IEnumerator CoRideCar()
    //{
    //    while(isEnd == false)
    //    {
    //        this.transform.position = carTr.position - Vector3.up / 1.5f;
    //        yield return null;
    //    }
    //}
}

