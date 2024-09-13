using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMain : MonoBehaviour
{
    enum eMap
    {
        Mountain,
        Island
    }

    public System.Action<int> onSelectGameScene;
    public System.Action onLoadGameScene;
    public Toggle[] toggles;
    public Button playBtn;
    public Image mapImage;

    private void Start()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            int j = i;
            toggles[j].onValueChanged.AddListener((toggle) =>
            {
                onSelectGameScene(j);
                mapImage.sprite = Resources.Load<Sprite>("Images/" + (eMap)j);
                if(mapImage.sprite != null)
                {
                    mapImage.gameObject.SetActive(true);
                }
                else if(mapImage.sprite == null)
                {
                    mapImage.gameObject.SetActive(false);
                }
            });
        }

        playBtn.onClick.AddListener(() =>
        {
            onLoadGameScene();
        });
    }
}
