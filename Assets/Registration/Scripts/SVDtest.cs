using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

public class SVDtest : MonoBehaviour {

    private Matrix<double> m = Matrix<double>.Build.Random(4, 4);
    private Matrix<double>n = Matrix<double>.Build.Random(4, 4);

    // Use this for initialization
    void Start () {
        var svd = m.Svd(true);
        n = svd.U * svd.W * svd.VT;
        Debug.Log("m" + m.ToMatrixString());
        Debug.Log("n" + n.ToMatrixString());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
