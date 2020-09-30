using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToGoHandler : MonoBehaviour
{
   private bool isStarted;
   private bool isDone = false;
   [SerializeField] private ParticleSystem ps;
   [SerializeField] private float minDis =0.9f;
   private void OnEnable() => isStarted = true;

   private void OnDisable() => isStarted = false;

   private void Update()
   {
      if (!isStarted || isDone) return;
      var taregtPosition = interactions.Instance.SessionOrigin.camera.transform.position;
      var x = new Vector2(taregtPosition.x,taregtPosition.z);
      var position = transform.position;
      var y = new Vector2(position.x,position.z);
      var dis = Vector2.Distance(x , y);
      if (dis > minDis) return;
      TutorialPlacingHandler.Instance.TargetReachPoint();
      isDone = true;
   }

   public void ActivePoint()
   {
      gameObject.SetActive(true); 
      ps.Play(true);
   }

   public void DisactivePoint()
   {
      ps.Stop(true);
      gameObject.SetActive(false);
   }
}
