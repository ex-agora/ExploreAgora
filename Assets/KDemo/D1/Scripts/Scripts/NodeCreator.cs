using System;
using System.Collections.Generic;
using UnityEngine;

namespace KDemo.D1.Scripts.Scripts
{
    public class NodeCreator : MonoBehaviour
    {
        [SerializeField] private SeekPath prefab;
        [SerializeField] private float creationDelay;
        [SerializeField] private Transform place;
        [SerializeField] private PathFollowingHandler path;
        [SerializeField] private int maxNodeNum = 10;
        [HideInInspector] [SerializeField] private List<SeekPath> nodesInPath;
        [HideInInspector] [SerializeField] private List<SeekPath> nodesFinished;

        private bool isStarted;
        //private void Start()
        //{
        //    PlaySim();
        //}
        private void Start()
        {
            isStarted = false;
        }

        private void CreateNodes()
        {
            if((nodesFinished.Count + nodesInPath.Count)<maxNodeNum)
                CreateNode();
            else if(IsInvoking(nameof(CreateNodes)))
                CancelInvoke(nameof(CreateNodes));
            
        }

        private void CreateNode()
        {
            var n = Instantiate(prefab, place.position, place.rotation);
            n.FollowingHandler = path;
            nodesInPath.Add(n);
        }

        public void NodeFinished(SeekPath p, bool isBack = true)
        {
            nodesInPath.Remove(p);
            p.gameObject.SetActive(false);
            nodesFinished.Add(p);
            if(isBack)
                PushBack();
        }

        private void PushBack()
        {
            var p = nodesFinished[0];
            nodesFinished.RemoveAt(0);
            p.CurrentNode = 0;
            p.ResetNode();
            p.transform.position = place.position;
            p.gameObject.SetActive(true);
            nodesInPath.Add(p);
        }
        
        public void PlaySim()
        {
            if (isStarted)
                return;
            isStarted = true;
            var size = nodesFinished.Count;
            for (var i = 0; i < size; i++)
            {
                Invoke(nameof(PushBack), (i + 1) * creationDelay);
            }
            InvokeRepeating(nameof(CreateNodes), creationDelay * (size + 1), creationDelay);
        }

        public void StopSim()
        {
            if (!isStarted)
                return;
            isStarted = false;
            var size = nodesInPath.Count;
            if(IsInvoking(nameof(CreateNodes)))
            {
                CancelInvoke(nameof(CreateNodes));
            }
            while(nodesInPath.Count !=0)
            {
                NodeFinished(nodesInPath[0], false);
            }   
        }
    }
}