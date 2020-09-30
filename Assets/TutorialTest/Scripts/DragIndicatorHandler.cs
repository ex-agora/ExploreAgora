using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DragIndicatorHandler : MonoBehaviour
{
   [SerializeField] private Transform target;
   [SerializeField] List<ParticleSystem> particles;
   [SerializeField] private Color enterColor;
   [SerializeField] private Color exitColor;
   private void OnTriggerEnter(Collider other)
   {
     if(other.transform != target) return;
     SetColor(enterColor);
   }

   private void SetColor(Color c)
   {
       foreach (var m in particles.Select(t => t.main))
       {
           var mainModule = m;
           mainModule.startColor = c;
       }
   }
   private void OnTriggerExit(Collider other)
   {
       if(other.transform != target) return;
       SetColor(exitColor);
   }
   
}
