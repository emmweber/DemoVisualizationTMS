using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

public class NuGetTest : MonoBehaviour
{

    private Matrix<double> m = Matrix<double>.Build.Random(3, 4);
    public GameObject Cube1, Cube2, Cube3;
    // Use this for initialization
    void Start()
    {
        // 3x4 dense matrix filled with zeros
        Debug.Log(m.ToString());
        m[0, 0] = 0.10f;
        m[0, 1] = 1.0f;
        m[1, 0] = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Before Math.Net");
         //3x4 dense matrix filled with zeros
        Debug.Log(m[0, 0].ToString());
        Cube1.transform.Rotate((float)m[0, 0], (float)m[0, 0], (float)m[0, 0] * Time.deltaTime);
        Cube2.transform.Rotate((float)m[0, 1], (float)m[0, 1], (float)m[0, 1] * Time.deltaTime);
        Cube3.transform.Rotate((float)m[1, 0], (float)m[1, 0], (float)m[1, 0] * Time.deltaTime);
        Debug.Log("After Math.Net");
    }
}
