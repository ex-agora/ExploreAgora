using System.Collections;
using System.Collections.Generic;
using KDemo.D1.Scripts.Scripts;
using UnityEngine;

public class PathFollowingHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] [SerializeField] List<SteerBase> path;
    [SerializeField] Seek obj;
    [SerializeField] float arriveRad;
    [SerializeField] int curruntNode;
    [SerializeField] private bool isCircle = true;
    [Range(0.000001f, 100f)] [SerializeField] private float gizmosRadius;
    [SerializeField] private Color gizmosColor = Color.red;
    [SerializeField] private NodeCreator creator;
    public List<SteerBase> Path
    {
        get => path;
        private set => path = value;
    }

    public bool IsCircle
    {
        get => isCircle;
        private set => isCircle = value;
    }
    public Color GizmosColor { get => gizmosColor; private set => gizmosColor = value; }

    private void Start()
    {
        Path.Clear();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            var s = this.transform.GetChild(i).GetComponent<SteerBase>();
            if (s != null)
                Path.Add(s);
        }
        if (Path.Count == 0)
            Destroy(this);
        curruntNode = 0;
        if (obj)
            obj.target = Path[curruntNode];
    }
    bool isArrive() {
       return (Path[curruntNode].position - obj.position).magnitude <= arriveRad;
    }

    public void Finish(SeekPath p)
    {
        creator.NodeFinished(p);
    }

    private void Update()
    {
        if(obj is null)
            return;
        
        if (isArrive())
        {
            curruntNode = (curruntNode + 1) % Path.Count;
            obj.target = Path[curruntNode];
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = GizmosColor;
        for(int i = 0; i < this.transform.childCount; i++) {
            Gizmos.DrawSphere(this.transform.GetChild(i).position, gizmosRadius);
            if(isCircle)
                Gizmos.DrawLine(this.transform.GetChild(i).position, this.transform.GetChild((i+1)% this.transform.childCount).position);
            else if( i!=0)
            {
                Gizmos.DrawLine(this.transform.GetChild(i).position, this.transform.GetChild((i-1)% this.transform.childCount).position);
            }
        }
    }
}
