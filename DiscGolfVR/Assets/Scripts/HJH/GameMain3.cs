using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using Oculus.Interaction;

public class GameMain3 : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject discPrefab;
    [SerializeField] private GameObject[] discPrefabs;
    [SerializeField] private GameObject[] discatcheres;
    [SerializeField] private GameObject[] startPoints;
    [SerializeField] private GameObject uiScore;
    [SerializeField] private ScoreCanvas scoreCanvas;
    [SerializeField] private PauseCanvas pauseCanvas;
    [SerializeField] private GameObject uiDistance;
    [SerializeField] private GameObject uiDisc;
    [SerializeField] private GameObject uiPause;
    [SerializeField] private GameObject uiSound;
    [SerializeField] private GameObject carGO;
    [SerializeField] private OVRInput.Button controller; // 컨트롤러
    [SerializeField] private OVRInput.Button controller2; // 컨트롤러
    [SerializeField] private OVRInput.Button controller3; // 컨트롤러
    [SerializeField] private OVRInput.Button controller4; // 컨트롤러
    [SerializeField] private OVRInput.Button controller5; // 컨트롤러
    [SerializeField] private OVRInput.Button controller6; // 컨트롤러
    [SerializeField] private OVRInput.Button controller7; // 컨트롤러
    [SerializeField] private int[] par;
    [SerializeField] private Water2[] water2;
    [SerializeField] private GameObject[] discImages;
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private Animator animRightBtn;
    [SerializeField] private Animator animLeftBtn;
    [SerializeField] private GameObject teleportArea;
    [SerializeField] private GameObject hologramController_R;
    [SerializeField] private GameObject hologramController_L;
    [SerializeField] private AudioSource goalIn;

    private GameObject disc;
    private DiscSimulator discController;
    private int count; // 던진 횟수
    private int totalCount; // 총 던진 횟수
    private int totalPar; // 총 Par
    private int holeNum = 0; // 홀 넘버
    private int index = 0;
    private Vector3 landDiscPos; // 원반 착지 위치
    private Vector3 playerPos; // 플레이어 위치
    private bool isLand = false; // 착지 여부
    private bool isInWater = false; // 페널티 여부
    private bool isStanding; 
    private Coroutine runningCoroutine = null;    
    public System.Action onLoadLobbyScene;
    private GameObject holoPrefab_L;
    private GameObject holoPrefab_R;
    private GameObject discatcherCanvasGo;

    // Start is called before the first frame update
    void Start()
    {
        if (playableDirector.gameObject.activeSelf)
        {
            playableDirector.stopped += StartGame;
            player.transform.LookAt(Vector3.left);
        }
        else
        {
            StartGame(holeNum);
        }
        StartCoroutine(CoPressOVRBtn());
        this.isStanding = true;

        foreach (var discatcher in discatcheres)
        {
            var Goal = discatcher.GetComponentInChildren<Goal>();
            Goal.DiscGoal = () =>
            {
                this.discatcherCanvasGo = this.discatcheres[holeNum].GetComponentInChildren<DiscatcherCanvas>().gameObject;
                this.discatcherCanvasGo.SetActive(false);
                this.isStanding = false;
                this.disc.GetComponent<Rigidbody>().useGravity = true;
                ++this.count;
                this.totalCount = this.totalCount + this.count;
                this.totalPar = this.totalPar + this.count - this.par[holeNum];
                this.scoreCanvas.textHoles[holeNum].text = this.count.ToString();
                if (totalPar > 0)
                    this.scoreCanvas.textPar.text = "+" + this.totalPar.ToString();
                else if (totalPar <= 0)
                    this.scoreCanvas.textPar.text = this.totalPar.ToString();
                this.scoreCanvas.textTotal.text = this.totalCount.ToString();
                this.uiScore.SetActive(true);
                var image = this.scoreCanvas.textHoles[holeNum].GetComponentInParent<Image>();
                if (this.count > this.par[holeNum])
                {
                    image.color = Color.red;
                }
                else
                {
                    image.color = Color.green;
                }
                this.uiScore.transform.position = this.discatcheres[holeNum].transform.position + this.player.transform.up * 2f;

                if (Vector3.Distance(discatcheres[holeNum].transform.position, this.playerPos) > 3.5f)
                {
                    TeleportPlayer(this.discatcheres[holeNum].transform.position + this.discatcheres[holeNum].transform.forward * 3f);
                }
                else if (Vector3.Distance(discatcheres[holeNum].transform.position, this.playerPos) < 2f)
                {
                    TeleportPlayer(this.discatcheres[holeNum].transform.position + this.discatcheres[holeNum].transform.forward * 3f);
                }
                this.uiScore.transform.rotation = this.player.transform.rotation;

                discatcher.GetComponentInChildren<AudioSource>().Play();
                this.goalIn.Play();
            };
        }

        this.scoreCanvas.ReStart = () =>
        {
            this.discatcherCanvasGo.SetActive(true);
            this.discatcheres[holeNum].GetComponentInChildren<DiscatcherCanvas>().LookPlayer();
            var trigger = this.discatcheres[holeNum].GetComponentInChildren<Goal>().gameObject;
            var v = trigger.GetComponents<Collider>();
            v[0].enabled = true;
            v[1].enabled = true;
            //Debug.LogFormat("<color=red>{0}</color>", trigger);
            this.totalCount = this.totalCount - this.count;

            StartGame(holeNum);
        };

        this.scoreCanvas.NextHole = () =>
        {
            if (holeNum != 4)
            {
                this.discatcheres[holeNum].SetActive(false);
                ++this.holeNum;
                this.isStanding = true;
                this.isLand = false;
                StartGame(holeNum);
            }
        };
        this.scoreCanvas.Quit = () =>
        {
            onLoadLobbyScene();
        };
        this.pauseCanvas.Quit = () =>
        {
            onLoadLobbyScene();
        };
        this.pauseCanvas.ReStart = () =>
        {
            this.teleportArea.SetActive(false);
            this.totalCount = this.totalCount - this.count;
            StartGame(holeNum);
        };

        this.pauseCanvas.Option = () =>
        {
            if (this.uiDisc.active)
            {
                this.uiDisc.SetActive(false);
                //Debug.Log(0);
            }
            if (this.uiPause.active)
            {
                this.uiPause.SetActive(false);
                //Debug.Log(1);
            }
            if (this.uiSound.active)
            {
                this.uiSound.SetActive(false);
                //Debug.Log(2);
            }
            else if (!this.uiSound.active)
            {
                this.uiSound.SetActive(true);
                //Debug.Log(3);
            }
        };

        foreach (var hazard in water2)
        {
            hazard.InWater = () =>
            {
                this.isInWater = true;
                // UI 생성
                if (runningCoroutine != null)
                {
                    this.uiDistance.SetActive(false);
                    StopCoroutine(runningCoroutine);
                }
                runningCoroutine = StartCoroutine(CoTextAnimation(this.playerPos, "Penalty "));
                // 카운트 증가
                this.count++;
                // 디스크 리스폰
                RespawnDisc(this.playerPos + this.player.transform.forward * 0.4f + this.player.transform.up * 1.3f, index);
            };
        }        
    }
    private void StartGame(PlayableDirector playableDirector)
    {

        this.count = 0;
        this.discatcheres[0].SetActive(true);
        // 플레이어 생성
        TeleportPlayer(startPoints[0].transform.position);
        // 디스크 생성      
        RespawnDisc(this.playerPos + this.player.transform.forward * 0.4f + this.player.transform.up * 1.3f, index);
        // 홀로그램 생성
        this.holoPrefab_L = Instantiate(hologramController_L);
        this.holoPrefab_L.transform.position = this.disc.transform.position - this.disc.transform.right * 0.2f + this.disc.transform.up * 0.05f;
        this.holoPrefab_L.transform.LookAt(this.discatcheres[holeNum].transform);
        this.holoPrefab_R = Instantiate(hologramController_R);
        this.holoPrefab_R.transform.position = this.disc.transform.position + this.disc.transform.right * 0.15f + this.disc.transform.up * 0.06f;
        this.holoPrefab_R.transform.LookAt(this.discatcheres[holeNum].transform);

        this.carGO.SetActive(false);

    }
    private void StartGame(int holeNum)
    {
        this.count = 0;
        this.uiScore.SetActive(false);
        this.discatcheres[holeNum].SetActive(true);
        // 플레이어 소환
        TeleportPlayer(startPoints[holeNum].transform.position);
        // 디스크 소환        
        RespawnDisc(this.playerPos + this.player.transform.forward * 0.4f + this.player.transform.up * 1.3f, index);
        // 홀로그램 생성
        this.holoPrefab_L = Instantiate(hologramController_L);
        this.holoPrefab_L.transform.position = this.disc.transform.position - this.disc.transform.right * 0.2f + this.disc.transform.up * 0.05f;
        this.holoPrefab_L.transform.LookAt(this.discatcheres[holeNum].transform);
        this.holoPrefab_R = Instantiate(hologramController_R);
        this.holoPrefab_R.transform.position = this.disc.transform.position + this.disc.transform.right * 0.15f + this.disc.transform.up * 0.06f;
        this.holoPrefab_R.transform.LookAt(this.discatcheres[holeNum].transform);
        this.isStanding = true;
    }
    private void TeleportPlayer(Vector3 pos)
    {
        this.player.transform.position = pos;
        this.player.transform.LookAt(this.discatcheres[holeNum].transform);
        this.playerPos = pos;
    }
    private void RespawnDisc(Vector3 pos, int index)
    {
        this.isInWater = false;
        Destroy(this.disc);
        this.disc = Instantiate(discPrefabs[index]);
        this.disc.transform.position = pos;
        this.disc.transform.LookAt(this.discatcheres[holeNum].transform);
        this.disc.GetComponent<Rigidbody>().useGravity = false;
        this.discController = disc.GetComponent<DiscSimulator>();
        this.discController.Land = (pos) =>
        {
            float dis1 = (float)Math.Round(Vector3.Distance(this.discatcheres[holeNum].transform.position, this.landDiscPos), 2);
            if (dis1 < 2.5f)
            {
                this.landDiscPos = pos + new Vector3(0, 0, -2f);
            }
            else
            {
                this.landDiscPos = pos;
            }
            if (!this.isLand)
            {
                if (!isInWater)
                    this.isLand = true;
                float dis = (float)Math.Round(Vector3.Distance(this.player.transform.position, pos), 2);
                // UI 출력                                
                StartCoroutine(CoTextAnimation(pos, ""));
                // teleprotArea 생성
                if (isStanding)
                {                    
                    this.teleportArea.SetActive(true);
                    this.teleportArea.transform.position = pos;
                    this.teleportArea.GetComponentInChildren<TeleportAreaCanvas>().SetScale();
                    
                }
            }
        };
        this.disc.GetComponent<Grabbable>().isGrab = () =>
        {
            Destroy(this.holoPrefab_L);
            Destroy(this.holoPrefab_R);
        };

    }
    private IEnumerator CoPressOVRBtn()
    {
        while (true)
        {
            if (OVRInput.GetDown(controller) && isLand == true)
            {
                // Next                
                TeleportPlayer(landDiscPos);
                RespawnDisc(this.playerPos + this.player.transform.forward * 0.4f + this.player.transform.up * 1.3f, index);
                this.teleportArea.SetActive(false);
                this.count++;
                this.isLand = false;
                this.isStanding = true;
            }
            else if (OVRInput.GetDown(controller2) && isLand == true)
            {
                // Retry                
                this.teleportArea.SetActive(false);
                RespawnDisc(this.playerPos + this.player.transform.forward * 0.4f + this.player.transform.up * 1.3f, index);
                this.isLand = false;
                this.isStanding = true;
            }
            else if (OVRInput.GetDown(controller3))
            {
                if (this.uiPause.active)
                {
                    this.uiPause.SetActive(false);
                }

                if (this.uiSound.active)
                {
                    this.uiSound.SetActive(false);
                }

                if (this.uiDisc.active)
                {
                    //Debug.LogFormat("<color=yellow>트리거 버튼 클릭</color>");
                    this.uiDisc.SetActive(false);
                }
                else if (!this.uiDisc.active)
                {
                    //Debug.LogFormat("<color=yellow>트리거 버튼 클릭</color>");
                    this.uiDisc.SetActive(true);
                }
            }
            else if (OVRInput.GetDown(controller4))
            {
                this.animLeftBtn.SetTrigger("Push");
                if (index != 0)
                {
                    this.discImages[index].SetActive(false);
                    --index;
                    this.discImages[index].SetActive(true);
                }
            }
            else if (OVRInput.GetDown(controller5) && this.uiDisc.active)
            {
                this.animRightBtn.SetTrigger("Push");
                if (index != 3)
                {
                    this.discImages[index].SetActive(false);
                    ++index;
                    this.discImages[index].SetActive(true);
                }
            }
            else if (OVRInput.GetDown(controller6) && this.uiDisc.active)
            {
                RespawnDisc(this.playerPos + this.player.transform.forward * 0.4f + this.player.transform.up * 1.3f, index);
            }
            else if (OVRInput.GetDown(controller7))
            {
                if (this.uiDisc.active)
                {
                    this.uiDisc.SetActive(false);
                }
                if (this.uiSound.active)
                {
                    this.uiSound.SetActive(false);
                }
                if (this.uiPause.active)
                {
                    //Debug.LogFormat("<color=yellow>트리거 버튼 클릭</color>");
                    this.uiPause.SetActive(false);
                }
                else if (!this.uiPause.active)
                {
                    //Debug.LogFormat("<color=yellow>트리거 버튼 클릭</color>");
                    this.uiPause.SetActive(true);
                }

            }
            // 버튼을 누르면 UI 활성화


            yield return null;
        }
    }
    private IEnumerator CoTextAnimation(Vector3 pos, string message)
    {
        float time = 0;
        // 텍스트 변경
        float dis = (float)Math.Round(Vector3.Distance(this.player.transform.position, pos), 2);
        var text = uiDistance.GetComponentInChildren<TMP_Text>();
        if (dis == 0)
        {
            text.text = message + "+1";
        }
        else
        {
            text.text = message + " " + dis.ToString() + "m";
        }
        this.uiDistance.transform.position = this.playerPos + this.player.transform.forward * 2f + this.player.transform.up;
        this.uiDistance.transform.LookAt(new Vector3(this.player.transform.position.x, this.uiDistance.transform.position.y, this.player.transform.position.z));
        this.uiDistance.transform.position -= this.uiDistance.transform.right;
        // 텍스트 활성화
        this.uiDistance.SetActive(true);
        // 텍스트 이동
        while (true)
        {
            time += Time.deltaTime;
            this.uiDistance.transform.Translate(Vector3.right * Time.deltaTime);
            if (time > 3f)
            {
                this.uiDistance.transform.Translate(Vector3.zero);
                break;
            }
            yield return null;
        }
        // 텍스트 비활성화
        this.uiDistance.SetActive(false);
        yield return null;
    }
}
