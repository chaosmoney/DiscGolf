using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public float fadeDuration = 2;
    private List<Renderer> rend = new List<Renderer>();

    private GameObject loadingUI;


    private void Awake()
    {
        loadingUI = Instantiate(Resources.Load<GameObject>("UILoadingSprite"));
        loadingUI.transform.SetParent(this.transform);
        loadingUI.transform.position = this.transform.position + Vector3.forward *0.35f;

        Renderer[] rendChild = loadingUI.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < rendChild.Length; i++)
        {
            rend.Add(rendChild[i]);
        }
    }

    private void Start()
    {
        FadeIn();
    }

    public void FadeIn()
    {
        FadeScreen(1, 0);
    }

    public void FadeOut()
    {
        FadeScreen(0, 1);
    }

    public void FadeScreen(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        if (alphaOut == 1)
        {
            this.loadingUI.SetActive(true);
        }
        float timer = 0;
        while(timer <= fadeDuration)
        {
            for (int i = 0; i < rend.Count; i++)
            {
                Color newColor = rend[i].material.color;
                newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);
                rend[i].material.SetColor("_Color", newColor);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < rend.Count; i++)
        {
            Color newColor2 = rend[i].material.color;
            newColor2.a = alphaOut;
            rend[i].material.SetColor("_Color", newColor2);
        }

        if(alphaIn == 1)
        {
            //this.loadingUI.transform.position = new Vector3(10000,10000,10000);
            this.loadingUI.SetActive(false);
        }
        

    }
}
