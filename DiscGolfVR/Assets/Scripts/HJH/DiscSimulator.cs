using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DiscSimulator : MonoBehaviour
{
    [SerializeField] private PhysicsGrabbable physicsGrabbable;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private OVRInput.Button controller; // 컨트롤러    
    [SerializeField] private float discSpeed;
    [SerializeField] private GameObject trail;

    private GameObject goTrail;
    private bool isThrowing = false;
    public Action<Vector3> Land;
    private Vector3 pos;
    private GameObject rightHandAnchor;

    private Vector3 oldPosition;
    private Vector3 currentPosition;
    private double velocity;
    private float power;
    // Start is called before the first frame update
    void Start()
    {
        this.rightHandAnchor = GameObject.Find("RightHandAnchor");
        this.rigidbody = GetComponent<Rigidbody>();
        this.pos = this.transform.position;
        this.physicsGrabbable.Disc = (speed, rotationalSpeed) =>
        {
            //Debug.LogFormat("<color=red>speed : {0} </color>", speed);
            //Debug.LogFormat("<color=yellow>rotationalSpeed : {0} </color>", rotationalSpeed);
            // 각도 구하기
            float angle = CalculateAngleWithGround(speed);
            //Debug.LogFormat("<color=yellow>angle : {0} </color>", angle);
            //Debug.LogFormat("<color=yellow>speed : {0} </color>", speed.magnitude);
            ThrowDisc(speed, rotationalSpeed);
            isThrowing = true;
        };

    }

    private void FixedUpdate()
    {
        if (OVRInput.GetDown(controller)) // 버튼을 누르면 위치 초기화
        {
            this.rigidbody.AddForce(Vector3.zero);
            this.isThrowing = false;
            this.gameObject.transform.position = pos;
            this.gameObject.transform.rotation = Quaternion.identity;
        }
        if (isThrowing)
        {
            Gravity();
            Lift();
            //Drag();
        }
        currentPosition = this.gameObject.transform.position;
        var dis = currentPosition - oldPosition;
        var distance = Math.Sqrt(Math.Pow(dis.x, 2) + Math.Pow(dis.y, 2) + Math.Pow(dis.z, 2));
        velocity = distance / Time.deltaTime;
        oldPosition = currentPosition;

        // 속도가 0이 되고, 던지기중 false,
        if (velocity <= 0.1 && isThrowing == true)
        {
            isThrowing = false;
            this.Land(this.gameObject.transform.position);            
            Destroy(this.goTrail);
            //Debug.LogFormat("<color=green>{0}</color>", velocity);
        }
    }
    private void Gravity()
    {
        rigidbody.AddForce(0, -9.82f, 0, ForceMode.Acceleration);
        //rigidbody.AddForce(0, -5f, 0, ForceMode.Acceleration);
    }
    private void Lift()
    {
        //rigidbody.AddForce(transform.up.normalized * (float)3f / 9, ForceMode.Acceleration);
        //rigidbody.AddForce(transform.up.normalized * 5f, ForceMode.Acceleration);
        
        power -= Time.deltaTime;
        if (power < 0)
        {
            power = 0;
        }
        //Debug.LogFormat("<color=magenta>Power : {0}</color>", power);
        rigidbody.AddForce(transform.up.normalized * power, ForceMode.Acceleration);
        
    }
    private void Drag()
    {
        //float cd = CDO + CDA * Mathf.Pow((float)(alpha - (ALPHA0) * Mathf.PI / 180), 2);
        float drag = 0; //(RHO * Mathf.Pow(rigidBody.velocity.magnitude, 2) * AREA * cd) / 2 * (15 / SPEED) / 1.5f;
        rigidbody.AddForce(-rigidbody.velocity.normalized * (float)drag, ForceMode.Acceleration);
    }
    private void ThrowDisc(Vector3 speed, Vector3 rotationalSpeed)
    {
        //power = 5f;
        power = 9.82f;
        this.goTrail = Instantiate(this.trail, this.gameObject.transform);
        rigidbody.maxAngularVelocity = 2000;
        rigidbody.drag = 0.1f;
        rigidbody.mass = 0.176f;

        rigidbody.angularDrag = 0;        
        rigidbody.AddForce(speed * (1.0f + 0.1f * discSpeed), ForceMode.Impulse); // 날아가는 힘과 방향 
        //rigidbody.AddForce(speed, ForceMode.Impulse); // 날아가는 힘과 방향 
        rigidbody.AddTorque(transform.up * rotationalSpeed.y * 0.01f, ForceMode.Impulse); // 회전

        this.rightHandAnchor.GetComponent<AudioSource>().Play();
    }
    public float CalculateAngleWithGround(Vector3 speed)
    {

        Vector3 groundNormal = new Vector3(speed.x, 0, speed.z);

        // 방향 벡터와 지면 법선 벡터 사이의 각도 계산
        float angle = Vector3.Angle(speed, groundNormal);

        return angle;
    }
}
