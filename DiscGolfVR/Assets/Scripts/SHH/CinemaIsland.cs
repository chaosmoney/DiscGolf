using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CinemaIsland : MonoBehaviour
{
    public Transform boatTr;

    public void MoveToPoint5()
    {
        this.transform.position = boatTr.position - Vector3.up*2;
        this.transform.SetParent(boatTr);
    }

    public void MoveToPoint6()
    {
        this.transform.SetParent(null);
    }
}

