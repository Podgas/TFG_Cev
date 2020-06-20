using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewSystem : MonoBehaviour
{
    [Header("Base FOV Options")]
    [SerializeField]
    public float viewRadius;
    [SerializeField]
    [Range(0, 360)]
    public float viewAngle;
    [SerializeField]
    Transform pointOfView;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public Vector3 model;
    
    public List<Transform> visibleTargets = new List<Transform>();

    [Header("Mesh Debug")]
    public float meshResolution;
    public float edgeResolveIterations;
    public float edgeDistanceThreshold;
    public MeshFilter viewMeshFilter;

    Mesh viewMesh;
    Vector3 minPointDebug;
    Vector3 maxPointDebug;

    protected virtual void Start()
    {
        StartMeshDebug();
        StartCoroutine("FindTargetWithDelay",Time.deltaTime);
    }

    IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            FindVisibleTargets(viewRadius, visibleTargets);

            yield return new WaitForSeconds(delay);
            
        }
    }
    private void Update()
    {

    }
    protected virtual void LateUpdate()
    {
        DrawFieldOfView();
    }


    void FindVisibleTargets(float radius, List<Transform> viewArray)
    {

        Collider[] targetsInViewRadius = null;

        viewArray.Clear();
        targetsInViewRadius = Physics.OverlapSphere(pointOfView.position, radius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Vector3 target = targetsInViewRadius[i].transform.position;
            Vector3 dirToTarget = (target - pointOfView.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target);
                
                if (!Physics.Raycast(pointOfView.position, dirToTarget, distToTarget, obstacleMask))
                {
                    Debug.DrawRay(pointOfView.position, dirToTarget, Color.red);
                    if (!viewArray.Contains(targetsInViewRadius[0].transform))
                        viewArray.Add(targetsInViewRadius[0].transform);
                }
            }
        }
    }

    //DEBUG FUNCTIONS FOV
    void StartMeshDebug()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }
    void DrawFieldOfView()
    {
        int stepcount = Mathf.RoundToInt(viewAngle * meshResolution);

        float stepAngleSize = viewAngle / stepcount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepcount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            if (newViewCast.viewObject != null)
            {

            }


            if (i >= 0)
            {
                bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;

        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
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
    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;

            if (newViewCast.hit == minViewCast.hit && !edgeDistanceThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }
        minPointDebug = minPoint;
        maxPointDebug = maxPoint;
        return new EdgeInfo(minPoint, maxPoint);
    }
    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 direction = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle, hit.transform);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + direction * viewRadius, viewRadius, globalAngle, null);
        }
    }
    public Vector3 DirFromAngle(float angleInDegree, bool angleIsGlobal)
    {

        if (!angleIsGlobal)
        {
            angleInDegree += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegree * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegree * Mathf.Deg2Rad));
    }
    ///////////////////


    public void SetForward(Vector3 newForward)
    {
        model = newForward;
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;
        public Transform viewObject;

        public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle,Transform _viewObject)
        {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;
            viewObject = _viewObject;
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
