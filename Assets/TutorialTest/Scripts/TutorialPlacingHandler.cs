using System;
using System.Collections;
using System.Collections.Generic;
using Pixel_Play.Scripts.OffScreenIndicator;
using UnityEngine;

public class TutorialPlacingHandler : MonoBehaviour
{
   [SerializeField] private List<PointToGoHandler> points;
   [SerializeField] private Transform dragGroundl;
   [SerializeField] private DragIndicatorHandler indicator;
   [SerializeField] private Target tableTareget;
   [SerializeField] private GameObject portal;
   private int pointIndex = -1;
   public static TutorialPlacingHandler Instance { get; private set; }

   private void Awake()
   {
      if (Instance is null)
         Instance = this;
   }

   private void Start()
   {
      TutorialFlowManager.Instance.AppleFeedback();
   }

   public void HandlePoint()
   {
      if (pointIndex > -1)
         points[pointIndex].DisactivePoint();      
      pointIndex++;
      if (pointIndex < points.Count)
         points[pointIndex].ActivePoint();
      else
         TargetReachPoint();
   }
   

   public void TargetReachPoint()
   { 
      TutorialFlowManager.Instance.AppleFeedback();
   }

   public void EnableDrag()
   {
      dragGroundl.gameObject.SetActive(true);
      indicator.gameObject.SetActive(true);
      tableTareget.enabled = true;
   }

   public void StartPortal()
   {
      portal.SetActive(true);
   }

   public void DragDone()
   {
      dragGroundl.gameObject.SetActive(false);     
      tableTareget.enabled = false;

   }
}
