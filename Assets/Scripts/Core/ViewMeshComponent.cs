using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ViewMeshComponent : NetworkBehaviour
{
    float meshAngle;
    float meshRadius;
    LayerMask obstrat;
    Mesh viewMesh;
    public MeshFilter viewMeshFilter;
    public float resolution;
    public int findEdgeIterations;
    public float edgeDstThresold;
    public float maskCutDistance;

    void Start() {
        meshAngle = GetComponent<FieldOfViewComponent>().viewAngle;
        meshRadius = GetComponent<FieldOfViewComponent>().viewRaidus;
        obstrat = GetComponent<FieldOfViewComponent>().obstractMask;
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    private void LateUpdate() {
        DrawFieldOfView();
    }

    /// <summary>
    /// 计算视野mesh的顶点和三角形并绘制
    /// </summary>
    void DrawFieldOfView() {
        int stepCount = Mathf.RoundToInt(meshAngle * resolution);
        float stepAngleSizes = meshAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i < stepCount; i++) {
            float angle = transform.eulerAngles.y - meshAngle / 2 + stepAngleSizes * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            if (i > 0) {
                bool edgeDstThresoldExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThresold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresoldExceeded )) {
                    EdgeInfo edgeInfo = FindEdge(oldViewCast, newViewCast);
                    if (edgeInfo.pointA != Vector3.zero)
                        viewPoints.Add(edgeInfo.pointA);
                    if (edgeInfo.pointB != Vector3.zero)
                        viewPoints.Add(edgeInfo.pointB);
                }
            }
            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];
        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++) {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]) + Vector3.forward * maskCutDistance;
            if (i < vertexCount - 2) {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    /// <summary>
    /// 获得射线的信息
    /// </summary>
    /// <param name="globalAgble"></param>
    /// <returns></returns>
    ViewCastInfo ViewCast(float globalAgble) {
        Vector3 dir = GetComponent<FieldOfViewComponent>().DirFromAngle(globalAgble, true);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, meshRadius, obstrat)) {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAgble);
        } else {
            return new ViewCastInfo(false, transform.position + dir * meshRadius, meshRadius, globalAgble);
        }
    }

    /// <summary>
    /// 找到正确的相交边
    /// </summary>
    /// <param name="minViewCast"></param>
    /// <param name="maxViewCast"></param>
    /// <returns></returns>
    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;
        for (int i = 0; i < findEdgeIterations; i++) {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);
            bool edgeDstThresoldExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThresold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresoldExceeded) {
                minAngle = angle;
                minPoint = newViewCast.point;
            } else {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }
        return new EdgeInfo(minPoint, maxPoint);
    }

    public struct ViewCastInfo {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle) {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
