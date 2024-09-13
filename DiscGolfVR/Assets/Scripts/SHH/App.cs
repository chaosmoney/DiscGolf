using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour
{
    private int selectedGameSceneNum;
    private Fade sceneFade;

    private static App instance = null;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        AsyncOperation oper = SceneManager.LoadSceneAsync("TitleScene");
        oper.completed += OnLoadCompleteTitleScene;
    }
    private void OnLoadCompleteTitleScene(AsyncOperation obj)
    {
        sceneFade = GameObject.FindObjectOfType<Fade>();
        //sceneFade.FadeIn();
        TitleMain titleMain = GameObject.FindObjectOfType<TitleMain>();
        titleMain.onLoadLobbyScene = () =>
        {
            StartCoroutine(LoadLobbyScene());
        };
    }

    private IEnumerator LoadLobbyScene()
    {
        sceneFade.FadeOut();
        AsyncOperation oper = SceneManager.LoadSceneAsync("LobbyScene");
        oper.completed += OnLoadCompleteLobbyScene;
        oper.allowSceneActivation = false;

        float timer = 0;
        while (timer <= sceneFade.fadeDuration && !oper.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        oper.allowSceneActivation = true;
    }

    private void OnLoadCompleteLobbyScene(AsyncOperation obj)
    {
        sceneFade = GameObject.FindObjectOfType<Fade>();
        //sceneFade.FadeIn();
        LobbyMain lobbyMain = GameObject.FindObjectOfType<LobbyMain>();
        lobbyMain.onLoadGameScene = () =>
        {
            StartCoroutine(LoadGameScene());
        };

        lobbyMain.onSelectGameScene = (int num) =>
        {
            SelectGameScene(num);
        };
    }

    private void SelectGameScene(int num)
    {
        this.selectedGameSceneNum = num;
    }

    private IEnumerator LoadGameScene()
    {
        float timer = 0;
        switch (this.selectedGameSceneNum)
        {
            case 0:
                sceneFade.FadeOut();
                AsyncOperation oper0 = SceneManager.LoadSceneAsync("GameScene");
                oper0.completed += OnLoadCompleteGameScene;
                oper0.allowSceneActivation = false;

                while (timer <= sceneFade.fadeDuration && !oper0.isDone)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }

                oper0.allowSceneActivation = true;
                break;
            case 1:
                sceneFade.FadeOut();
                AsyncOperation oper1 = SceneManager.LoadSceneAsync("GameScene2");
                oper1.completed += OnLoadCompleteGameScene;
                oper1.allowSceneActivation = false;

                while (timer <= sceneFade.fadeDuration && !oper1.isDone)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }

                oper1.allowSceneActivation = true;
                break;
            default:
                Debug.Log("¾ÆÁ÷ ¾È ¸¸µë");
                break;
        }
    }

    private void OnLoadCompleteGameScene(AsyncOperation obj)
    {
        sceneFade = GameObject.FindObjectOfType<Fade>();
        //sceneFade.FadeIn();
        GameMain3 gameMain = GameObject.FindObjectOfType<GameMain3>();
        gameMain.onLoadLobbyScene = () =>
        {
            StartCoroutine(LoadLobbyScene());
        };

    }
}
