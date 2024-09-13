using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Frisbee : MonoBehaviour
{
    private static float x; // 현재 위치
    private static float y; // 현재 위치
    private static float vx; // 방향 속도
    private static float vy;// 방향 속도
    private static readonly float g = -9.81f; // 중력 가속도
    private static readonly float m = 0.175f; // 질량
    private static readonly float RHO = 1.23f; // 공기 밀도
    private static readonly float AREA = 0.0568f; // 면적
    private static readonly float CL0 = 0.1f; // 
    private static readonly float CLA = 1.4f;
    private static readonly float CD0 = 0.08f;
    private static readonly float CDA = 2.72f;
    private static readonly float ALPHA0 = -4f;

    private static bool simulationStarted = false;
    private GameObject frisbeeObject; // 프리스비 오브젝트
    private Rigidbody rb; // 프리스비의 Rigidbody 컴포넌트
    private float timeStep = 0.01f; // 시뮬레이션 단계 간격
    private float initialYPosition = 2.0f; // 프리스비의 초기 높이
    private float initialVelocityX = 20.0f; // 초기 x 방향 속도
    private float initialVelocityY = 10.0f; // 초기 y 방향 속도
    private float alpha = 15.0f; // 초기 공격각(α)
    private float elapsedTime = 0.0f; // 경과 시간

    private void Start()
    {
        frisbeeObject = GameObject.CreatePrimitive(PrimitiveType.Sphere); // 공 생성
        frisbeeObject.transform.position = new Vector3(0, initialYPosition, 0); // 초기 위치 설정
        rb = frisbeeObject.AddComponent<Rigidbody>(); // Rigidbody 추가
        //rb.useGravity = false; // 중력 비활성화
        rb.velocity = new Vector3(0, initialVelocityY, initialVelocityX); // 초기 속도 설정
        frisbeeObject.SetActive(false); // 비활성화
    }

    private void Update()
    {
        // 스페이스바를 누르면 시뮬레이션 시작
        if (Input.GetKeyDown(KeyCode.Space) && !simulationStarted)
        {
            frisbeeObject.SetActive(true); // 공 활성화
            simulationStarted = true; // 시뮬레이션 시작 플래그 설정
        }

        // 시뮬레이션이 시작되면 경과 시간 증가
        if (simulationStarted)
        {
            elapsedTime += Time.deltaTime;

            // 시간 간격마다 위치 업데이트
            if (elapsedTime >= timeStep)
            {
                // 프리스비 비행 시뮬레이션 호출
                Simulate(frisbeeObject.transform.position.y, rb.velocity.x, rb.velocity.y, alpha, timeStep);
                elapsedTime = 0.0f;
            }
        }
    }
    public static void Simulate(float y0, float vx0, float vy0, float alpha, float deltaT)
    {
        float cl = CL0 + CLA * alpha * Mathf.PI / 180;
        float cd = CD0 + CDA * Mathf.Pow((alpha - ALPHA0) * Mathf.PI / 180, 2);
        x = 0;
        y = y0;
        vx = vx0;
        vy = vy0;

        try
        {
            StreamWriter writer = new StreamWriter("frisbee.csv");
            int k = 0;
            while (y > 0)
            {
                float deltavy = (RHO * Mathf.Pow((float)vx, 2) * AREA * cl / (2 * m) + g) * deltaT;
                float deltavx = -RHO * Mathf.Pow((float)vx, 2) * AREA * cd * deltaT;
                vx = vx + deltavx;
                vy = vy + deltavy;
                x = x + vx * deltaT;
                y = y + vy * deltaT;

                if (k % 10 == 0)
                {
                    writer.WriteLine(x + "," + y + "," + vx);
                }
                k++;
            }

            writer.Close();
        }
        catch (Exception e)
        {
            Debug.Log("Error, file frisbee.csv is in use.");
        }
    }
}
