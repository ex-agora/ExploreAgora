﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
#if UNITY_2017_2_OR_NEWER
using UnityEngine.XR;
#else
using UnityEngine.VR;
#endif

namespace SIS
{
    [RequireComponent(typeof(Canvas))]
    public class VRGraphicRaycaster : BaseRaycaster
    {
        protected const int kNoEventMaskSet = -1;
        public enum BlockingObjects
        {
            None = 0,
            TwoD = 1,
            ThreeD = 2,
            All = 3,
        }

        public override int sortOrderPriority
        {
            get
            {
                // We need to return the sorting order here as distance will all be 0 for overlay.
                if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                    return canvas.sortingOrder;

                return base.sortOrderPriority;
            }
        }

        public override int renderOrderPriority
        {
            get
            {
                // We need to return the sorting order here as distance will all be 0 for overlay.
                if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                    return canvas.rootCanvas.renderOrder;

                return base.renderOrderPriority;
            }
        }

        [FormerlySerializedAs("ignoreReversedGraphics")]
        [SerializeField]
        private bool m_IgnoreReversedGraphics = true;
        [FormerlySerializedAs("blockingObjects")]
        [SerializeField]
        private BlockingObjects m_BlockingObjects = BlockingObjects.None;

        public bool ignoreReversedGraphics { get { return m_IgnoreReversedGraphics; } set { m_IgnoreReversedGraphics = value; } }
        public BlockingObjects blockingObjects { get { return m_BlockingObjects; } set { m_BlockingObjects = value; } }

        [SerializeField]
        protected LayerMask m_BlockingMask = kNoEventMaskSet;

        private Canvas m_Canvas;

        protected VRGraphicRaycaster()
        { }

        private Canvas canvas
        {
            get
            {
                if (m_Canvas != null)
                    return m_Canvas;

                m_Canvas = GetComponent<Canvas>();
                return m_Canvas;
            }
        }

        [NonSerialized] private List<Graphic> m_RaycastResults = new List<Graphic>();
        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            if (canvas == null)
                return;

            int displayIndex;
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || !eventCamera)
                displayIndex = canvas.targetDisplay;
            else
                displayIndex = eventCamera.targetDisplay;

            #if !UNITY_EDITOR
                #if UNITY_2017_2_OR_NEWER
                Vector2 vrPos = new Vector2(XRSettings.eyeTextureWidth / 2, XRSettings.eyeTextureHeight / 2);
                #else
                Vector2 vrPos = new Vector2(VRSettings.eyeTextureWidth / 2, VRSettings.eyeTextureHeight / 2);
                #endif
            eventData.position = vrPos;
            #endif

            var eventPosition = Display.RelativeMouseAt(eventData.position);
            if (eventPosition != Vector3.zero)
            {
                // We support multiple display and display identification based on event position.

                int eventDisplayIndex = (int)eventPosition.z;

                // Discard events that are not part of this display so the user does not interact with multiple displays at once.
                if (eventDisplayIndex != displayIndex)
                    return;
            }
            else
            {
                // The multiple display system is not supported on all platforms, when it is not supported the returned position
                // will be all zeros so when the returned index is 0 we will default to the event data to be safe.
                eventPosition = eventData.position;

                // We dont really know in which display the event occured. We will process the event assuming it occured in our display.
            }

            // Convert to view space
            Vector2 pos = Vector2.zero;
            if (eventCamera == null)
            {
                // Multiple display support only when not the main display. For display 0 the reported
                // resolution is always the desktops resolution since its part of the display API,
                // so we use the standard none multiple display method. (case 741751)
                float w = Screen.width;
                float h = Screen.height;
                #if !UNITY_EDITOR
                    #if UNITY_2017_2_OR_NEWER
                    w = XRSettings.eyeTextureWidth;
                    h = XRSettings.eyeTextureHeight;
                    #else
                    w = VRSettings.eyeTextureWidth;
                    h = VRSettings.eyeTextureHeight;
                    #endif
                #endif

                if (displayIndex > 0 && displayIndex < Display.displays.Length)
                {
                    w = Display.displays[displayIndex].systemWidth;
                    h = Display.displays[displayIndex].systemHeight;
                }
                pos = new Vector2(eventPosition.x / w, eventPosition.y / h);
            }
            else
                pos = eventCamera.ScreenToViewportPoint(eventPosition);

            // If it's outside the camera's viewport, do nothing
            if (pos.x < 0f || pos.x > 1f || pos.y < 0f || pos.y > 1f)
                return;

            float hitDistance = float.MaxValue;

            Ray ray = new Ray();

            if (eventCamera != null)
            {
                #if UNITY_EDITOR
                ray = eventCamera.ScreenPointToRay(eventPosition);
                #else
                ray = new Ray(eventCamera.transform.position, eventCamera.transform.forward);
                #endif
            }
			
			if (canvas.renderMode != RenderMode.ScreenSpaceOverlay && blockingObjects != BlockingObjects.None)
			{
				float dist = 100.0f;

				if (eventCamera != null)
					dist = eventCamera.farClipPlane - eventCamera.nearClipPlane;

				if (blockingObjects == BlockingObjects.ThreeD || blockingObjects == BlockingObjects.All)
				{
					if (ReflectionMethodsCache.Singleton.raycast3D != null)
					{
						RaycastHit hit;
						if (ReflectionMethodsCache.Singleton.raycast3D(ray, out hit, dist, m_BlockingMask))
							hitDistance = hit.distance;
					}
				}

				if (blockingObjects == BlockingObjects.TwoD || blockingObjects == BlockingObjects.All)
				{
					if (ReflectionMethodsCache.Singleton.raycast2D != null)
					{
						var hit = ReflectionMethodsCache.Singleton.raycast2D((Vector2)ray.origin, (Vector2)ray.direction, dist, (int)m_BlockingMask);
                        if (hit.collider)
                            hitDistance = hit.fraction * dist;
					}
				}
			}

			m_RaycastResults.Clear();
			Raycast(canvas, eventCamera, eventPosition, m_RaycastResults);

			for (var index = 0; index < m_RaycastResults.Count; index++)
			{
				var go = m_RaycastResults[index].gameObject;
				bool appendGraphic = true;

				if (ignoreReversedGraphics)
				{
					if (eventCamera == null)
					{
						// If we dont have a camera we know that we should always be facing forward
						var dir = go.transform.rotation * Vector3.forward;
						appendGraphic = Vector3.Dot(Vector3.forward, dir) > 0;
					}
					else
					{
						// If we have a camera compare the direction against the cameras forward.
						var cameraFoward = eventCamera.transform.rotation * Vector3.forward;
						var dir = go.transform.rotation * Vector3.forward;
						appendGraphic = Vector3.Dot(cameraFoward, dir) > 0;
					}
				}

				if (appendGraphic)
				{
					float distance = 0;

					if (eventCamera == null || canvas.renderMode == RenderMode.ScreenSpaceOverlay)
						distance = 0;
					else
					{
						Transform trans = go.transform;
						Vector3 transForward = trans.forward;
						// http://geomalgorithms.com/a06-_intersect-2.html
						distance = (Vector3.Dot(transForward, trans.position - ray.origin) / Vector3.Dot(transForward, ray.direction));

						// Check to see if the go is behind the camera.
						if (distance < 0)
							continue;
					}

					if (distance >= hitDistance)
						continue;

					var castResult = new RaycastResult
					{
						gameObject = go,
						module = this,
						distance = distance,
						screenPosition = eventPosition,
						index = resultAppendList.Count,
						depth = m_RaycastResults[index].depth,
						sortingLayer = canvas.sortingLayerID,
						sortingOrder = canvas.sortingOrder
					};
					resultAppendList.Add(castResult);
				}
			}
		}

		public override Camera eventCamera
		{
			get
			{
				if (canvas.renderMode == RenderMode.ScreenSpaceOverlay
					|| (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null))
					return null;

				return canvas.worldCamera != null ? canvas.worldCamera : Camera.main;
			}
		}

		/// <summary>
		/// Perform a raycast into the screen and collect all graphics underneath it.
		/// </summary>
		[NonSerialized] static readonly List<Graphic> s_SortedGraphics = new List<Graphic>();
		private static void Raycast(Canvas canvas, Camera eventCamera, Vector2 pointerPosition, List<Graphic> results)
		{
			// Debug.Log("ttt" + pointerPoision + ":::" + camera);
			// Necessary for the event system
			var foundGraphics = GraphicRegistry.GetGraphicsForCanvas(canvas);
			for (int i = 0; i < foundGraphics.Count; ++i)
			{
				Graphic graphic = foundGraphics[i];

				if (graphic.canvasRenderer.cull)
					continue;

				// -1 means it hasn't been processed by the canvas, which means it isn't actually drawn
				if (graphic.depth == -1 || !graphic.raycastTarget)
					continue;

				if (!RectTransformUtility.RectangleContainsScreenPoint(graphic.rectTransform, pointerPosition, eventCamera))
					continue;

				if (graphic.Raycast(pointerPosition, eventCamera))
				{
					s_SortedGraphics.Add(graphic);
				}
			}

			s_SortedGraphics.Sort((g1, g2) => g2.depth.CompareTo(g1.depth));
			//      StringBuilder cast = new StringBuilder();
			for (int i = 0; i < s_SortedGraphics.Count; ++i)
				results.Add(s_SortedGraphics[i]);
			//      Debug.Log (cast.ToString());

			s_SortedGraphics.Clear();
		}
	}


	internal class ReflectionMethodsCache
	{
		public delegate bool Raycast3DCallback(Ray r, out RaycastHit hit, float f, int i);
		public delegate RaycastHit2D Raycast2DCallback(Vector2 p1, Vector2 p2, float f, int i);
		public delegate RaycastHit[] RaycastAllCallback(Ray r, float f, int i);
		public delegate RaycastHit2D[] GetRayIntersectionAllCallback(Ray r, float f, int i);

		// We call Physics.Raycast and Physics2D.Raycast through reflection to avoid creating a hard dependency from
		// this class to the Physics/Physics2D modules, which would otherwise make it impossible to make content with UI
		// without force-including both modules.
		public ReflectionMethodsCache()
		{
			var raycast3DMethodInfo = typeof(Physics).GetMethod("Raycast", new[] { typeof(Ray), typeof(RaycastHit).MakeByRefType(), typeof(float), typeof(int) });
            if (raycast3DMethodInfo != null)
            {
                #if UNITY_2019_1_OR_NEWER
                    raycast3D = (Raycast3DCallback)Delegate.CreateDelegate(typeof(Raycast3DCallback), raycast3DMethodInfo);
                #else
                    raycast3D = (Raycast3DCallback)UnityEngineInternal.ScriptingUtils.CreateDelegate(typeof(Raycast3DCallback), raycast3DMethodInfo);
                #endif
            }

			var raycast2DMethodInfo = typeof(Physics2D).GetMethod("Raycast", new[] { typeof(Vector2), typeof(Vector2), typeof(float), typeof(int) });
            if (raycast2DMethodInfo != null)
            {
                #if UNITY_2019_1_OR_NEWER
                    raycast2D = (Raycast2DCallback)Delegate.CreateDelegate(typeof(Raycast2DCallback), raycast2DMethodInfo);
                #else
                    raycast2D = (Raycast2DCallback)UnityEngineInternal.ScriptingUtils.CreateDelegate(typeof(Raycast2DCallback), raycast2DMethodInfo);
                #endif
            }

			var raycastAllMethodInfo = typeof(Physics).GetMethod("RaycastAll", new[] { typeof(Ray), typeof(float), typeof(int) });
            if (raycastAllMethodInfo != null)
            {
                #if UNITY_2019_1_OR_NEWER
                    raycast3DAll = (RaycastAllCallback)Delegate.CreateDelegate(typeof(RaycastAllCallback), raycastAllMethodInfo);
                #else
                    raycast3DAll = (RaycastAllCallback)UnityEngineInternal.ScriptingUtils.CreateDelegate(typeof(RaycastAllCallback), raycastAllMethodInfo);
                #endif
                
            }

			var getRayIntersectionAllMethodInfo = typeof(Physics2D).GetMethod("GetRayIntersectionAll", new[] { typeof(Ray), typeof(float), typeof(int) });
            if (getRayIntersectionAllMethodInfo != null)
            {
                #if UNITY_2019_1_OR_NEWER
                    getRayIntersectionAll = (GetRayIntersectionAllCallback)Delegate.CreateDelegate(typeof(GetRayIntersectionAllCallback), getRayIntersectionAllMethodInfo);
                #else
                    getRayIntersectionAll = (GetRayIntersectionAllCallback)UnityEngineInternal.ScriptingUtils.CreateDelegate(typeof(GetRayIntersectionAllCallback), getRayIntersectionAllMethodInfo);
                #endif
            }
		}

		public Raycast3DCallback raycast3D = null;
		public RaycastAllCallback raycast3DAll = null;
		public Raycast2DCallback raycast2D = null;
		public GetRayIntersectionAllCallback getRayIntersectionAll = null;

		private static ReflectionMethodsCache s_ReflectionMethodsCache = null;

		public static ReflectionMethodsCache Singleton
		{
			get
			{
				if (s_ReflectionMethodsCache == null)
					s_ReflectionMethodsCache = new ReflectionMethodsCache();
				return s_ReflectionMethodsCache;
			}
		}
	}
}