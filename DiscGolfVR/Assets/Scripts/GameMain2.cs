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
    private OVRInput.Button controller; // ��Ʈ�ѷ�
    [SerializeField]
    private OVRInput.Button controller2; // ��Ʈ�ѷ�
    [SerializeField]
    private OVRInput.Button controller3; // ��Ʈ�ѷ�
    [SerializeField]
    private GameObject[] discatcheres; // ���
    [SerializeField]
    private GameObject[] startPoints; // ���� ����(Ȧ)
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

    private int count; // ���� Ƚ��
    private int totalCount;
    private bool isLand; // ���� ����
    private Transform discTr;    
    public int hallNum;

    // ���� ���� �� UI
    // ���� ������ �������� ��

    // Start is called before the first frame update
    void Start()
    {
        //���� ����
        this.count = 0;
        this.hallNum = 0;
        this.isLand = false;
        this.canvas.SetActive(false);
        //startPoint �̵� �Լ� �ʿ�
        this.disc.gameObject.transform.position = startPoints[hallNum].transform.position + new Vector3(0, 1f, 1f);
        this.Player.transform.position = startPoints[hallNum].transform.position;
        // ��� Ȱ��ȭ
        this.discatcheres[hallNum].SetActive(true);


        foreach(var discatcher in discatcheres)
        {
            var Goal = discatcher.GetComponentInChildren<Goal>();
            Goal.DiscGoal = () =>
            {
                this.disc.rigidBody.velocity = Vector3.zero;
                Debug.Log("<color=magenta>GameMain : ����</color>");
                this.count += 1;
                this.textHoles[hallNum].text = this.count.ToString();
                this.totalCount += 1;
                this.textTotal.text = this.totalCount.ToString();
                // ������ UI ����
                this.canvas.gameObject.SetActive(true);
                // �÷��̾� �տ� ���� ���� �ʿ�
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
            // �κ������ �̵�
        });
        this.disc.Land = (pos) =>
        {
            this.isLand = true;
            this.discTr = disc.gameObject.transform;
            
            // ���ݰ� ��� �Ÿ� ��� ���            
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
        // ������ ���� ���� �� �߰� �ʿ�
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
// ��ƽ�� �����̸� �ڵ� ���� ����
// <UI>
// ��Ʈ(Ȧ) ����
// ����
// ����
// ��ũ
// PAR(��) : �� Ȧ�� �� ���� ���Ͽ� �־�� �ϴ����� �������� �� ���ھ� (ex : +1)
// Birdie(�۵�) : ���� Ÿ�� ���� �� ���� ���� ���� ��
// Eagle(�̱�) : ���� Ÿ�� ���� 2�� ���� ���� ��
// Ȧ�ο�(Hole in One) �ѹ��� ���� ��
// ����(Bogey) : ���� Ÿ�� ���� �� ���� �� ���� ��
// ���� ����(Double Bogey) : ���� Ÿ�� ���� 2���� �� ���� ��
// Ʈ���� ����
// ���巯�� ����
