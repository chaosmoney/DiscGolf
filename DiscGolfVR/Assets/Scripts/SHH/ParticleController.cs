using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem particleSys;
    private PlayableDirector playableDirector;

    void Start()
    {
        particleSys = GetComponentInChildren<ParticleSystem>();
        playableDirector = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        if (playableDirector != null)
        {
            // 타임라인의 상태를 확인
            if (playableDirector.state == PlayState.Playing)
            {

            }
            else
            {
                particleSys.Stop();
            }
        }
        else
        {

        }
    }
}