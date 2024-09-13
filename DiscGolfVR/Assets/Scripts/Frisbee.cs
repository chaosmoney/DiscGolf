using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Frisbee : MonoBehaviour
{
    private static float x; // ���� ��ġ
    private static float y; // ���� ��ġ
    private static float vx; // ���� �ӵ�
    private static float vy;// ���� �ӵ�
    private static readonly float g = -9.81f; // �߷� ���ӵ�
    private static readonly float m = 0.175f; // ����
    private static readonly float RHO = 1.23f; // ���� �е�
    private static readonly float AREA = 0.0568f; // ����
    private static readonly float CL0 = 0.1f; // 
    private static readonly float CLA = 1.4f;
    private static readonly float CD0 = 0.08f;
    private static readonly float CDA = 2.72f;
    private static readonly float ALPHA0 = -4f;

    private static bool simulationStarted = false;
    private GameObject frisbeeObject; // �������� ������Ʈ
    private Rigidbody rb; // ���������� Rigidbody ������Ʈ
    private float timeStep = 0.01f; // �ùķ��̼� �ܰ� ����
    private float initialYPosition = 2.0f; // ���������� �ʱ� ����
    private float initialVelocityX = 20.0f; // �ʱ� x ���� �ӵ�
    private float initialVelocityY = 10.0f; // �ʱ� y ���� �ӵ�
    private float alpha = 15.0f; // �ʱ� ���ݰ�(��)
    private float elapsedTime = 0.0f; // ��� �ð�

    private void Start()
    {
        frisbeeObject = GameObject.CreatePrimitive(PrimitiveType.Sphere); // �� ����
        frisbeeObject.transform.position = new Vector3(0, initialYPosition, 0); // �ʱ� ��ġ ����
        rb = frisbeeObject.AddComponent<Rigidbody>(); // Rigidbody �߰�
        //rb.useGravity = false; // �߷� ��Ȱ��ȭ
        rb.velocity = new Vector3(0, initialVelocityY, initialVelocityX); // �ʱ� �ӵ� ����
        frisbeeObject.SetActive(false); // ��Ȱ��ȭ
    }

    private void Update()
    {
        // �����̽��ٸ� ������ �ùķ��̼� ����
        if (Input.GetKeyDown(KeyCode.Space) && !simulationStarted)
        {
            frisbeeObject.SetActive(true); // �� Ȱ��ȭ
            simulationStarted = true; // �ùķ��̼� ���� �÷��� ����
        }

        // �ùķ��̼��� ���۵Ǹ� ��� �ð� ����
        if (simulationStarted)
        {
            elapsedTime += Time.deltaTime;

            // �ð� ���ݸ��� ��ġ ������Ʈ
            if (elapsedTime >= timeStep)
            {
                // �������� ���� �ùķ��̼� ȣ��
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
