﻿using UnityEngine;
using System.Collections;

public class Radar : MonoBehaviour
{
    //网格模型顶点数量
    private int VERTICES_COUNT;
    [Tooltip("边数为数组长度减1")]
    //顶点数组
    public Vector3[] vertices;
    //三角形数组
    private int[] triangles;
    public float scale;
    private MeshFilter meshFilter;
    private Mesh mesh;
    private float pi = 3.1415f;

    private void Start()
    {
        CreateMesh();
        SetVertices();
    }
    private void OnGUI()
    {
        if (meshFilter == null)
        {
            CreateMesh();
            SetVertices();
        }

        Apply();
        if (GUILayout.Button("Apply "))
        {
            Apply();
        }
    }

    private void CreateMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
    }
    private void SetVertices()
    {
        VERTICES_COUNT = vertices.Length;
        int triangles_count = VERTICES_COUNT - 1;
        triangles = new int[triangles_count * 3];

        //设定原点坐标
        vertices[0] = new Vector3(0, 0, 1);
        //首个在x轴上的坐标点
        vertices[1] = new Vector3(45, 0, 1);

        //每个三角形角度
        float everyAngle = 360 / triangles_count;

        for (int i = 2; i < vertices.Length; i++)
        {
            var angle = GetRadians(everyAngle * (i - 1));
            vertices[i] = new Vector3(45 * Mathf.Cos(angle), 45 * Mathf.Sin(angle), 1);
        }
        
        int idx = 0;
        int value = 0;
        for (int i = 0; i < triangles.Length; i++)
        {
            if (i % 3 == 0)
            {
                triangles[i] = 0;
                value = idx;
                idx++;
            }
            else
            {
                value++;
                if (value == VERTICES_COUNT)
                    value = 1;
                Debug.Log("value " + value);

                triangles[i] = value;
            }
        }

        //vertices[0] = new Vector3(0, 0, 1);
        //vertices[1] = new Vector3(45, 0, 1);
        //vertices[2] = new Vector3(45 * Mathf.Cos(GetRadians(75)), 45 * Mathf.Sin(GetRadians(75)), 1);
        //vertices[3] = new Vector3(-45 * Mathf.Cos(GetRadians(36)), 45 * Mathf.Sin(GetRadians(36)), 1);
        //vertices[4] = new Vector3(-45 * Mathf.Cos(GetRadians(36)), -45 * Mathf.Sin(GetRadians(36)), 1);
        //vertices[5] = new Vector3(45 * Mathf.Cos(GetRadians(75)), -45 * Mathf.Sin(GetRadians(75)), 1);

        //每三个顶点为一个三角面
        //triangles[0] = 0;
        //triangles[1] = 1;
        //triangles[2] = 2;

        //triangles[3] = 0;
        //triangles[4] = 2;
        //triangles[5] = 3;

        //triangles[6] = 0;
        //triangles[7] = 3;
        //triangles[8] = 4;

        //triangles[9] = 0;
        //triangles[10] = 4;
        //triangles[11] = 5;

        //triangles[12] = 0;
        //triangles[13] = 5;
        //triangles[14] = 1;
    }
    private float GetRadians(float angle)
    {
        return pi / 180 * angle;
    }
    private void Apply()
    {
        Vector3[] tmps = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            tmps[i] = vertices[i] * vertices[i].z * scale;
        }

        mesh.vertices = tmps;
        mesh.triangles = triangles;
    }
}