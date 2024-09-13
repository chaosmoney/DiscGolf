using Oculus.Interaction;
using System;
using UnityEditor;
using UnityEngine;

public class DiscController : MonoBehaviour
{
    private float alpha = 0;
    private float RHO = 1.23f; // 공기 밀도
    private float roll;
    private float spin;
    private float pitch;

    [SerializeField] private float rotationalSpeed = 50;
    [SerializeField] private float discSpeed;

    [SerializeField] public Rigidbody rigidBody;

    [SerializeField] private float CLO = 0.1f; // 양력
    [SerializeField] private float CLA = 1.4f; // 양력
    [SerializeField] private float CDO = 0.08f;
    [SerializeField] private float CDA = 2.72f;
    [SerializeField] private float CRR = 0.014f;
    [SerializeField] private float CRP = -0.0055f;
    [SerializeField] private float CNR = -0.0000071f;
    [SerializeField] private float CM0 = -0.08f;
    [SerializeField] private float CMA = 0.43f;
    [SerializeField] private float CMQ = -0.005f;

    [SerializeField] private float ALPHA0 = -4;
    [SerializeField] private float AREA = 0.0568f; // 표준 프리스비의 면적

    [SerializeField] private float diameter = 0.21f; // in m
    [SerializeField] private float m = 0.176f; // 무게?

    [SerializeField] private float GLIDE = 3f;
    [SerializeField] private float SPEED = 3f;
    [SerializeField] private float TURN = 0;
    [SerializeField] private float FADE = 1;

    private Vector3 pos;    

    [SerializeField] private PhysicsGrabbable physicsGrabbable;
    [SerializeField] private GameObject trail;
    private GameObject goTrail;

    public bool isThrowing = false;
    private bool isDiscThrow1 = false;
    public Action<Vector3> Land;

    private Vector3 oldPosition;
    private Vector3 currentPosition;
    private double velocity;
    private float power;
    private void Start()
    {        
        Transform tr = this.transform;
        pos = tr.position;

        rigidBody = GetComponent<Rigidbody>();
        rigidBody.maxAngularVelocity = 2000;
        rigidBody.drag = 0;
        rigidBody.angularDrag = 0;//
        rigidBody.mass = m;
        //rigidBody.useGravity = false;
        rigidBody.isKinematic = false;
        alpha = Vector3.Angle(rigidBody.velocity, transform.forward);

        this.physicsGrabbable.Disc = (speed, rotationalSpeed) =>
        {
            Debug.LogFormat("<color=red>speed : {0} </color>", speed);
            Debug.LogFormat("<color=yellow>rotationalSpeed : {0} </color>", rotationalSpeed);
            float angle = CalculateAngleWithGround(speed);
            if(angle < 20)
            {
                speed.y = 0;
            }            
            Debug.LogFormat("<color=green>angle : {0} </color>", angle);
            Debug.LogFormat("<color=green>speed 제곱근 : {0} </color>", MathF.Sqrt(speed.x*speed.x + speed.z*speed.z));
            // SPEED가 3 이상이면 ThrowDisc1, 이하면 ThrowDisc2
            if(MathF.Sqrt(speed.x * speed.x + speed.z * speed.z) > 1.5f)
            {
                isDiscThrow1 = true;
                ThrowDisc(speed, rotationalSpeed);
            }
            else
            {
                ThrowDisc(speed, rotationalSpeed);
            }
            isThrowing = true;
        };
        oldPosition = transform.position;
    }
    
    private void FixedUpdate()
    {
        //
        if (Input.GetKeyDown(KeyCode.R)) // 버튼을 누르면 위치 초기화
        {
            this.rigidBody.AddForce(Vector3.zero + Vector3.forward * 0.5f);
            this.isThrowing = false;
            this.gameObject.transform.position = pos;
            this.gameObject.transform.rotation = Quaternion.identity;
        }
        //

        if (isThrowing == true && isDiscThrow1 == true)
        {
            Torque();
            //Gravity();
            Drag();
            Lift();
            //Magnus();
        }
        
        currentPosition = this.gameObject.transform.position;
        var dis = currentPosition - oldPosition;
        var distance = Math.Sqrt(Math.Pow(dis.x, 2)+Math.Pow(dis.y, 2)+Math.Pow(dis.z,2));
        velocity = distance / Time.deltaTime;
        oldPosition = currentPosition;
        
        // 속도가 0이 되고, 던지기중 false,
        if (velocity <= 0.1 && isThrowing == true)
        {
            isThrowing = false;
            isDiscThrow1 = false;
            this.Land(this.gameObject.transform.position);
            Destroy(this.goTrail);
            //Debug.LogFormat("<color=green>{0}</color>", velocity);
        }


    }
    private void Lift()
    {
        //float cl = CLO + CLA * alpha * Mathf.PI / 180;
        //float lift = (RHO * Mathf.Pow(rigidBody.velocity.magnitude, 2) * AREA * cl / 2 / m) * Time.deltaTime * 4 + GLIDE / 9;
        //rigidBody.AddForce(transform.up.normalized * (float)lift, ForceMode.Acceleration);
        
        power -= Time.deltaTime;
        if (power < 0)
        {
            power = 0;
        }
        rigidBody.AddForce(transform.up.normalized * power, ForceMode.Acceleration);
    }
    private void Drag()
    {
        float cd = CDO + CDA * Mathf.Pow((float)(alpha - (ALPHA0) * Mathf.PI / 180), 2);
        float drag = (RHO * Mathf.Pow(rigidBody.velocity.magnitude, 2) * AREA * cd) / 2 * (15 / SPEED) / 1.5f;
        //Debug.LogFormat("<color=magenta>{0}</color>", drag);
        rigidBody.AddForce(-rigidBody.velocity.normalized * (float)drag, ForceMode.Acceleration);
    }
    private void Gravity()
    {
        //rigidBody.AddForce(0, -9.82f, 0, ForceMode.Acceleration);
        rigidBody.AddForce(0, -5f, 0, ForceMode.Acceleration);
    }
    private void Torque()
    {
        roll = (CRR * rigidBody.angularVelocity.y + CRP * rigidBody.angularVelocity.x) * 1 / 2 * RHO * (float)Math.Pow(rigidBody.velocity.magnitude, 2) * AREA * diameter * 0.01f * 6 - TURN / 2;
        roll -= FADE * 3;
        spin = -(CNR * rigidBody.angularVelocity.y) * 1 / 2 * RHO * Mathf.Pow(rigidBody.velocity.magnitude, 2) * AREA * diameter * 0.01f;
        pitch = (CM0 + CMA * (Mathf.PI / 180 * (alpha)) + CMQ * rigidBody.angularVelocity.z) * 1 / 2 * RHO * Mathf.Pow(rigidBody.velocity.magnitude, 2) * AREA * diameter * 0.01f * 6;

        rigidBody.AddTorque(Vector3.Cross(transform.up, rigidBody.velocity).normalized * roll, ForceMode.Acceleration);
        rigidBody.AddTorque(transform.up * spin, ForceMode.Acceleration);
        rigidBody.AddTorque(rigidBody.velocity.normalized * pitch, ForceMode.Acceleration);
        //Debug.LogFormat("<color=magenta>{0}, {1}, {2}</color>", Vector3.Cross(transform.up, rigidBody.velocity).normalized * roll, ForceMode.Acceleration, transform.up * spin, ForceMode.Acceleration, rigidBody.velocity.normalized * pitch, ForceMode.Acceleration);
    }
    /*
    private void Torque()
    {
        roll = (Disc.CRR * rigidBody.angularVelocity.y + Disc.CRP * rigidBody.angularVelocity.x) * 1 / 2 * RHO * Math.Pow(rigidBody.velocity.magnitude, 2) * Disc.AREA * Disc.diameter * 0.01f * 6 - Disc.TURN / 2;//TODO make this more realistic!
        roll -= Disc.FADE * 3; // Gamification!
        spin = -(Disc.CNR * rigidBody.angularVelocity.y) * 1 / 2 * RHO * Math.Pow(rigidBody.velocity.magnitude, 2) * Disc.AREA * Disc.diameter * 0.01f;
        pitch = (Disc.CM0 + Disc.CMA * (Math.PI / 180 * (alpha)) + Disc.CMq * rigidBody.angularVelocity.z) * 1 / 2 * RHO * Math.Pow(rigidBody.velocity.magnitude, 2) * Disc.AREA * Disc.diameter * 0.01f * 6; //TODO make this more realistic!

        
        rigidBody.AddTorque(Vector3.Cross(transform.up, rigidBody.velocity).normalized * (float)-roll, ForceMode.Acceleration);                

        rigidBody.AddTorque(transform.up * (float)spin, ForceMode.Acceleration);
        rigidBody.AddTorque(rigidBody.velocity.normalized * (float)pitch, ForceMode.Acceleration);
    }
    */

    private void Magnus()
    {
        var direction = Vector3.Cross(rigidBody.angularVelocity, rigidBody.velocity);
        float cl = CLO + CLA * alpha * Mathf.PI / 180;
        Vector3 magnus = cl * 1 / 2 * RHO * direction * AREA;
        Debug.Log(magnus);
        this.rigidBody.AddForce(magnus, ForceMode.Acceleration);
    }

    public void ThrowDisc(Vector3 speed1, Vector3 rotationalSpeed1)
    {
        //rigidBody.AddTorque(transform.up * rotationalSpeed * 0.01f, ForceMode.Impulse);
        //rigidBody.AddForce(transform.forward * speed / 3.6f, ForceMode.Impulse);
        power = 9.82f;
        //power = 5f;
        this.goTrail = Instantiate(this.trail, this.gameObject.transform);
        
        rigidBody.AddForce(speed1 * discSpeed, ForceMode.Impulse);
        rigidBody.AddTorque(transform.up * rotationalSpeed1.y * 0.01f, ForceMode.Impulse);
        Debug.LogFormat("<color=green> {0} </color>", transform.up * rotationalSpeed * 0.01f);        
    }
    public void ThrowDisc2(Vector3 speed1, Vector3 rotationalSpeed1)
    {                
        this.goTrail = Instantiate(this.trail, this.gameObject.transform);

        rigidBody.AddForce(speed1, ForceMode.Impulse);
        //rigidBody.AddTorque(transform.up * rotationalSpeed1.y * 0.01f, ForceMode.Impulse);
        Debug.LogFormat("<color=green> {0} </color>", transform.up * rotationalSpeed * 0.01f);
    }
    public float CalculateAngleWithGround(Vector3 speed)
    {

        Vector3 groundNormal = new Vector3(speed.x, 0, speed.z);

        // 방향 벡터와 지면 법선 벡터 사이의 각도 계산
        float angle = Vector3.Angle(speed, groundNormal);

        return angle;
    }
}
