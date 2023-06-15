using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public class Parabol
    {
      
        public int pointCount = 9;
        public float height = 3;

        public Vector3[] Calculate(Vector3 startPoint,Vector3 endPoint)
        {
            Vector3[] curvePoints = new Vector3[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                float t = i / (float)(pointCount - 1);
                curvePoints[i] = CalculateParabolicCurvePoint(t, startPoint, endPoint);
            }
            return curvePoints;
        }
        public Vector3 CalculateParabolicCurvePoint(float t, Vector3 start, Vector3 end)
        {
            float x = (end.x - start.x) * t + start.x;
            float y = height * t * (1 - t) + start.y; ;
            float z = (end.z - start.z) * t + start.z;
            return new Vector3(x, y, z);
        }
    }
}
