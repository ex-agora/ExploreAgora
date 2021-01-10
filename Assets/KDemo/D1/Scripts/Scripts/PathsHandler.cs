using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KDemo.D1.Scripts.Scripts
{
    public class PathsHandler : MonoBehaviour
    {
        [SerializeField] List<NodeCreator> nodeCreators;
        public void StartSimulation()
        {
            foreach (var nodeCreator in nodeCreators)
            {
                nodeCreator.PlaySim();
            }
        }
        public void StopSimulation()
        {
            foreach (var nodeCreator in nodeCreators)
            {
                nodeCreator.StopSim();
            }
        }
    }
}