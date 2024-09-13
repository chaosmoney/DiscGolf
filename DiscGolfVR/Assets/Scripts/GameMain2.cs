using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameMain2 : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private DiscController disc;
    [SerializeField]
    private GameObject discPrefab;
    [SerializeField]
    private OVRInput.Button controller; // 컨트롤러
    [SerializeField]
    private OVRInput.Button controller2; // 컨트롤러
    [SerializeField]
    private OVRInput.Button controller3; // 컨트롤러
    [SerializeField]
    private GameObject[] discatcheres; // 골대
    [SerializeField]
    private GameObject[] startPoints; // 시작 지점(홀)
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private Button btnRestart;
    [SerializeField]
    private Button btnNextHole;
    [SerializeField]
    private Button btnQuit;
    [SerializeField]
    private TMP_Text[] textHoles;
    [SerializeField]
    private TMP_Text textTotal;
    [SerializeField]
    private TMP_Text textPar;

    private int count; // 던진 횟수
    private int totalCount;
    private bool isLand; // 착지 여부
    private Transform discTr;    
    public int hallNum;

    // 골인 했을 때 UI
    // 원반 던지고 착지했을 때

    // Start is called before the first frame update
    void Start()
    {
        //게임 시작
        this.count = 0;
        this.hallNum = 0;
        this.isLand = false;
        this.canvas.SetActive(false);
        //startPoint 이동 함수 필요
        this.disc.gameObject.transform.position = startPoints[hallNum].transform.position + new Vector3(0, 1f, 1f);
        this.Player.transform.position = startPoints[hallNum].transform.position;
        // 골대 활성화
        this.discatcheres[hallNum].SetActive(true);


        foreach(var discatcher in discatcheres)
        {
            var Goal = discatcher.GetComponentInChildren<Goal>();
            Goal.DiscGoal = () =>
            {
                this.disc.rigidBody.velocity = Vector3.zero;
                Debug.Log("<color=magenta>GameMain : 골인</color>");
                this.count += 1;
                this.textHoles[hallNum].text = this.count.ToString();
                this.totalCount += 1;
                this.textTotal.text = this.totalCount.ToString();
                // 점수판 UI 생성
                this.canvas.gameObject.SetActive(true);
                // 플레이어 앞에 생성 수정 필요
                this.canvas.SetActive(true);
                this.canvas.transform.position = this.Player.transform.position + new Vector3(0,1f,1f);
                
            };
        }
        this.btnRestart.onClick.AddListener(() =>
        {
            Debug.Log("btnRestart");
            this.TeleportHall();
            ResetCount();
            this.disc.gameObject.transform.position = startPoints[hallNum].transform.position + new Vector3(0, 1f, 1f);
            this.canvas.transform.position = this.Player.transform.position + new Vector3(0, 1f, 1f);
        });
        this.btnNextHole.onClick.AddListener(() =>
        {
            Debug.Log("btnNextHole");
            if(hallNum < startPoints.Length)
            {
                this.hallNum++;
                this.TeleportHall();
                this.discatcheres[hallNum-1].SetActive(false);
                this.discatcheres[hallNum].SetActive(true);
                this.disc.gameObject.transform.position = startPoints[hallNum].transform.position + new Vector3(0, 1f, 1f);
                this.canvas.transform.position = this.Player.transform.position + new Vector3(0, 1f, 1f);
                ResetCount();
            }

        });
        this.btnQuit.onClick.AddListener(() =>
        {
            Debug.Log("btnQuit");
            // 로비씬으로 이동
        });
        this.disc.Land = (pos) =>
        {
            this.isLand = true;
            this.discTr = disc.gameObject.transform;
            
            // 원반과 골대 거리 계산 출력            
        };
        StartCoroutine(CoPressOVRBtn());
    }
    private void IncreaseCount()
    {
        
        this.count += 1;        
        this.totalCount += 1;
        textHoles[hallNum].text = this.count.ToString();

    }

    private void TeleportHall()
    {        
        this.Player.transform.position = this.startPoints[hallNum].transform.position;
        this.Player.transform.LookAt(this.discatcheres[hallNum].transform);
    }
    private void ResetCount()
    {
        this.count = 0;
    }
    private void RespawnDisc()
    {
        GameObject discGo = Instantiate(this.discPrefab);
        discGo.transform.position = this.disc.transform.position + new Vector3(0, 0.8f, 0.5f);
        
    }
    private IEnumerator CoPressOVRBtn()
    {
        // 원반이 착지 했을 때 추가 필요
        while (true)
        {
            if (OVRInput.GetDown(controller) && this.isLand == true)
            {
                                
                // Next
                this.Player.transform.position = this.disc.transform.position;
                this.disc.gameObject.transform.position = this.disc.transform.position + new Vector3(0,0.8f,0.5f);                
                this.IncreaseCount();
                this.isLand = false;
            }
            else if (OVRInput.GetDown(controller2) && this.isLand == true)
            {
                // Retry
                
                this.disc.gameObject.transform.position = this.Player.transform.position + new Vector3(0, 0.8f, 0.5f);
                this.isLand = false;
            }
            else if (OVRInput.GetDown(controller3))
            {
                RespawnDisc();
            }
            

            yield return null;
        }
        
    }
    
}
// 스틱을 움직이면 핸드 포즈 병경
// <UI>
// 세트(홀) 시작
// 골인
// 착지
// 디스크
// PAR(파) : 한 홀에 몇 번을 샷하여 넣어야 하는지를 기준으로 한 스코어 (ex : +1)
// Birdie(퍼디) : 기준 타수 보다 한 번을 적게 넣은 것
// Eagle(이글) : 기준 타수 보다 2번 적게 넣은 것
// 홀인원(Hole in One) 한번에 넣은 것
// 보기(Bogey) : 기준 타수 보다 한 번을 더 넣은 것
// 더블 보기(Double Bogey) : 기준 타수 보다 2번을 더 넣은 것
// 트리블 보기
// 쿼드러플 보기
