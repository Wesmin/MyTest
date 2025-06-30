using UnityEngine;

public class MappingCalculate : MonoBehaviour
{
    public Transform pointA, pointB, pointC, pointD;
    void Update()
    {
        pointD.position = PointMapping(pointA.position, pointB.position, pointC.position);
    }
    Vector3 PointMapping(Vector3 A, Vector3 B, Vector3 C)
    {
        float x1 = A.x;float y1 = A.y;float z1 = A.z;
        float x2 = B.x;float y2 = B.y;float z2 = B.z;
        float x3 = C.x;float y3 = C.y;float z3 = C.z;
        Vector3 vectorAB = new Vector3(x2 - x1, y2 - y1, z2 - z1);
        float dotProduct = vectorAB.x * (x3 - x1) + vectorAB.y * (y3 - y1) + vectorAB.z * (z3 - z1);
        float lengthSquared = vectorAB.x * vectorAB.x + vectorAB.y * vectorAB.y + vectorAB.z * vectorAB.z;
        if (dotProduct > lengthSquared) dotProduct = lengthSquared; if (dotProduct < 0) dotProduct = 0; //限制
        float Px = x1 + vectorAB.x * dotProduct / lengthSquared;
        float Py = y1 + vectorAB.y * dotProduct / lengthSquared;
        float Pz = z1 + vectorAB.z * dotProduct / lengthSquared;
        float Cx = x3 + (Px - x3);
        float Cy = y3 + (Py - y3);
        float Cz = z3 + (Pz - z3);
        return new Vector3(Cx, Cy, Cz);
    }
}
