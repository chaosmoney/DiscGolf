using Oculus.Interaction;
using Oculus.Interaction.OVR.Input;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;

// ������
// �÷��̾� �̵�
// �� �̵�

public class GameMain : MonoBehaviour
{
    // ��ũ
    [SerializeField]
    private DiscController disc;
    [SerializeField]
    private GameObject player;
    [SerializeField] 
    private OVRInput.Controller controller;
    [SerializeField]
    private Discatcher[] discatcheres;
    [SerializeField]
    private float moveSpeed = 5f;

    private int score = 0;

    // ��ũ ��ġ
    private Vector3 discTr;

    private bool isLand = false;

    private bool isCatch = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // ��ũ ���� �ϸ� ��ġ ����
        // ��ư ������ ������ �ִ� ��ũ ���� ��
        // ������ ��ġ�� ��ũ ���� ����
        // 
        this.disc.Land = (pos) =>
        {
            this.isLand = true;
            this.discTr = pos;
        };
        foreach(var discatcher in discatcheres)
        {
            discatcher.Goal = () =>
            {
                // ���� ����Ʈ ����
                // ���� UI ����
                // ������ UI ����
            };
        }
        
        
    }
    private void Update()
    {        
        // 
        if (OVRInput.Get(OVRInput.Button.Four))
        {
            this.isLand = false;
            this.disc.transform.position = new Vector3(this.discTr.x, 0.85f, this.discTr.z);
            this.player.transform.position = new Vector3(this.discTr.x, 0, this.discTr.z - 0.2f);
        }
        else if (OVRInput.Get(OVRInput.Button.Three))
        {
            this.disc.gameObject.transform.position = new Vector3(this.player.transform.position.x, 0.85f, this.player.transform.position.z + 0.3f);
            this.disc.rigidBody.AddForce(Vector3.zero);
            this.disc.isThrowing = false;            
            this.disc.gameObject.transform.rotation = Quaternion.identity;
        }
        Vector2 thumbstickValue = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller);
        var h = thumbstickValue.y;
        var v = thumbstickValue.x;
        var dir = new Vector3(h, 0, v);
        if (dir != Vector3.zero)
        {
            var angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            var q = Quaternion.AngleAxis(angle, Vector3.up);
            //var to = Quaternion.Slerp(this.playerTrans.rotation, q, Time.deltaTime * rotDamping);
            this.player.transform.rotation = q;

            this.player.transform.Translate(Vector3.forward * this.moveSpeed * Time.deltaTime);
        }
    }
    private void TeleportPlayer()
    {
        // ����� ��ũ ��ġ�� �÷��̾� �̵�

    }
    private void RespwanDisc()
    {
        Destroy(this.disc.gameObject);
        Instantiate(disc.gameObject);
        DiscController newDisc = GetComponent<DiscController>();
        this.disc = newDisc;
    }
}
