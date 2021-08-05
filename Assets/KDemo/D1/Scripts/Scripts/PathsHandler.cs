using System;
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
        IEnumerator NodeSwitchPush(int amount, float delay) {
            yield return new WaitForEndOfFrame();
            foreach (var nodeCreator in nodeCreators)
            {
                StartCoroutine(nodeCreator.SwitchNodes(amount, 1f));
                yield return new WaitForSeconds(delay);
            }
        }
        public void SwitchNode(int amount = -1)
        {
            StartCoroutine(NodeSwitchPush(amount, 1.5f));
        }
        
        public void SwitchNode(int index, int amount = -1)
        {
            if (index < 0 || index >= nodeCreators.Count) return;
            StartCoroutine(nodeCreators[index].SwitchNodes(amount));
        }
    }
}