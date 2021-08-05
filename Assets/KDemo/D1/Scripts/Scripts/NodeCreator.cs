using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KDemo.D1.Scripts.Scripts
{
    public class NodeCreator : MonoBehaviour
    {
        [SerializeField] private SeekPath prefab;
        [SerializeField] private int nodesSwitcherPushedAmount = 1;
        [SerializeField] private float creationDelay;
        [SerializeField] private Transform place;
        [SerializeField] private PathFollowingHandler path;
        [SerializeField] private int maxNodeNum = 10;
        [SerializeField] private Transform nodeParent = null;
        [SerializeField] private NodeCreator switcher = null;
        [SerializeField] private bool isSwitcher;
        [HideInInspector] [SerializeField] private List<SeekPath> nodesInPath;
        [HideInInspector] [SerializeField] private List<SeekPath> nodesFinished;

        private bool isStarted;

        public bool IsSwitcher
        {
            get => isSwitcher;
            set => isSwitcher = value;
        }

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
            SeekPath n = null;
            if (nodeParent)
            {
                n = Instantiate(prefab, nodeParent);
                n.transform.position = place.position;
                n.transform.rotation = place.rotation;
            }
            else
            {
                n = Instantiate(prefab, place.position, place.rotation);
            }

            //n.GetComponent<MeshRenderer>().material.color = path.GizmosColor;
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

        private void PuchBackInSwitcher()
        {
            var p = nodesInPath[0];
            nodesInPath.RemoveAt(0);
            p.FollowingHandler = switcher.path;
            p.CurrentNode = 0;
            switcher.nodesInPath.Add(p);
            p.ResetNode();
            p.gameObject.SetActive(true);
        }
        public void PlaySim()
        {
            if (isStarted) return;
            isStarted = true;

            if (IsSwitcher) return;
            
            var size = nodesFinished.Count;
            for (var i = 0; i < size; i++)
            {
                Invoke(nameof(PushBack), (i + 1) * creationDelay);
            }
            InvokeRepeating(nameof(CreateNodes), creationDelay * (size + 1), creationDelay);
        }

        public IEnumerator SwitchNodes(int amount = -1, float delay = 0)
        {
            yield return new WaitForEndOfFrame();
            if (!IsSwitcher)
            {
                amount = amount == -1 ? nodesSwitcherPushedAmount : amount;
                var size = Mathf.Min(nodesInPath.Count, amount);
                for (var i = 0; i < size; i++)
                {
                    PuchBackInSwitcher();
                    yield return new WaitForSeconds(delay);
                }

                if (!IsInvoking(nameof(CreateNodes)))
                {
                    InvokeRepeating(nameof(CreateNodes), creationDelay * (size + 1), creationDelay);
                }
            }
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

            if (IsSwitcher)
            {
                while(nodesInPath.Count !=0)
                {
                    var p = nodesInPath[0];
                    nodesInPath.RemoveAt(0);
                    Destroy(p.gameObject);
                }
            }
            else
            {
                while (nodesInPath.Count != 0)
                {
                    NodeFinished(nodesInPath[0], false);
                }
            }
        }
    }
}