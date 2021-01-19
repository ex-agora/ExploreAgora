using System;
using UnityEngine;

namespace KDemo.D1.Scripts.Scripts
{
    public class SeekPath : Seek
    {
        [SerializeField] private PathFollowingHandler pathFollowingHandler;
        [SerializeField] private OutlineHandler outline;
        private int currentNode = 0;
        private bool isFinish =false;
        public int CurrentNode
        {
            get => currentNode;
            set => currentNode = value;
        }

        public PathFollowingHandler FollowingHandler
        {
            get => pathFollowingHandler;
            set => pathFollowingHandler = value;
        }

        private void Start()
        {
            ResetNode();
        }

        public void ResetNode()
        {
            isFinish = false;
            target = FollowingHandler.Path[currentNode];
            outline.HideOutlineWithoutFade();
        }

        private bool IsArrive() {
            return (FollowingHandler.Path[currentNode].position - this.position).magnitude 
                   <= 
                   FollowingHandler.Path[currentNode].arriveRad;
        }

        private void Update()
        {
            if (isFinish)
            {
                FollowingHandler.Finish(this);
                return;
            }

            if (!IsArrive()) return;
            
            if(FollowingHandler.IsCircle)
                currentNode = (currentNode + 1) % FollowingHandler.Path.Count;
            else
            {
                var min = Mathf.Min((currentNode + 1), FollowingHandler.Path.Count);
                if (min == (FollowingHandler.Path.Count))
                {
                    isFinish = true;
                    min--;
                }
                currentNode = min;
            }

            target = FollowingHandler.Path[currentNode];
            maxVelocity = FollowingHandler.Path[currentNode].maxVelocity;
        }
    }
}