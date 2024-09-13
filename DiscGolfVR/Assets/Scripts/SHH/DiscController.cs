using OVR;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SHH
{
    public class DiscController : MonoBehaviour
    {
        private float alpha = 0;
        private float RHO = 1.23f; // 공기 밀도
        private float roll;
        private float spin;
        private float pitch;

        [SerializeField] bool isLift;
        [SerializeField] bool isDrag;
        [SerializeField] bool isGravity;
        [SerializeField] bool isTorque;

        [SerializeField] private float rotationalSpeed = 50;
        [SerializeField] private float speed = 100;

        [SerializeField] private Rigidbody rigidBody;

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

        private void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
            alpha = Vector3.Angle(rigidBody.velocity, transform.forward);
            Debug.Log(alpha);

            rigidBody.maxAngularVelocity = 2000;
            rigidBody.drag = 0;
            rigidBody.mass = m;
            rigidBody.useGravity = false;
            rigidBody.isKinematic = false;

            rigidBody.AddTorque(transform.up * rotationalSpeed * 0.01f, ForceMode.Impulse);
            rigidBody.AddForce(transform.forward * speed / 3.6f, ForceMode.Impulse);
        }
        private void FixedUpdate()
        {
            if (isLift)
            {
                Lift();
            }
            if (isDrag)
            {
                Drag();
            }
            if (isGravity)
            {
                Gravity();
            }
            if (isTorque)
            {
                Torque();
            }


        }
        private void Lift()
        {
            float cl = CLO + CLA * alpha * Mathf.PI / 180;
            float lift = (RHO * Mathf.Pow(rigidBody.velocity.magnitude, 2) * AREA * cl / 2 / m) * Time.deltaTime * 4 + GLIDE / 9;
            rigidBody.AddForce(transform.up.normalized * (float)lift, ForceMode.Acceleration);
            Debug.LogFormat("Lift: {0}", transform.up.normalized * (float)lift);
        }
        private void Drag()
        {
            float cd = CDO + CDA * Mathf.Pow((float)(alpha - (ALPHA0) * Mathf.PI / 180), 2);
            float drag = (RHO * Mathf.Pow(rigidBody.velocity.magnitude, 2) * AREA * cd) / 2 * (15 / SPEED) / 1.5f;
            rigidBody.AddForce(-rigidBody.velocity.normalized * (float)drag, ForceMode.Acceleration);
            Debug.LogFormat("Drag: {0}", -rigidBody.velocity.normalized * (float)drag);
        }
        private void Gravity()
        {
            rigidBody.AddForce(0, -9.82f, 0, ForceMode.Acceleration);
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
        }
    }
}