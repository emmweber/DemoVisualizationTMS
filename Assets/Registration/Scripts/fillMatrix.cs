using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

public class fillMatrix : MonoBehaviour {

    // create a dense zero-vector of length 10
    Matrix<double> fidMat = Matrix<double>.Build.Dense(3,4);
    List<Vector<double>> vecList = new List<Vector<double>>();

    // put all children Transforms in a Math.Net Matrix (not used anymore)
    public Matrix<double> Trans2Matrix()
    {
        int i = 0;
        foreach (Transform fidsTrans in this.GetComponentInChildren<Transform>())
        {
            fidMat[0, i] = fidsTrans.transform.position.x;
            fidMat[2, i] = fidsTrans.transform.position.y;
            fidMat[3, i] = fidsTrans.transform.position.z;
            i += 1;
        }
        return fidMat;
    }

    // put all children Transforms in a Math.Net Vector list
    public List<Vector<double>> TransVecList()
    {
        foreach (Transform fidsTrans in this.GetComponentInChildren<Transform>())
        {
            Vector<double> tmpVec = Vector<double>.Build.Dense(3);
            tmpVec[0] = fidsTrans.position.x;
            tmpVec[1] = fidsTrans.position.y;
            tmpVec[2] = fidsTrans.position.z;
            vecList.Add(tmpVec);
        }
        return vecList;
    }
}
