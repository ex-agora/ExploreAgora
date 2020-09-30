using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pixel_Play.Scripts.OffScreenIndicator
{
    class ArrowObjectPool : MonoBehaviour
    {
        public static ArrowObjectPool current;

        [Tooltip("Assign the arrow prefab.")]
        public Indicator pooledObject;
        [Tooltip("Initial pooled amount.")]
        public int pooledAmount = 1;
        [Tooltip("Should the pooled amount increase.")]
        public bool willGrow = true;

        List<Indicator> pooledObjects;

        private void Awake()
        {
            current = this;
        }

        private void Start()
        {
            pooledObjects = new List<Indicator>();

            for (var i = 0; i < pooledAmount; i++)
            {
                var arrow = Instantiate(pooledObject, transform, false);
                arrow.Activate(false);
                pooledObjects.Add(arrow);
            }
        }

        /// <summary>
        /// Gets pooled objects from the pool.
        /// </summary>
        /// <returns></returns>
        public Indicator GetPooledObject()
        {
            foreach (var t in pooledObjects.Where(t => !t.Active))
            {
                return t;
            }

            if (!willGrow) return null;
            var arrow = Instantiate(pooledObject, transform, false);
            arrow.Activate(false);
            pooledObjects.Add(arrow);
            return arrow;
        }

        /// <summary>
        /// Deactive all the objects in the pool.
        /// </summary>
        public void DeactivateAllPooledObjects()
        {
            foreach (var arrow in pooledObjects)
            {
                arrow.Activate(false);
            }
        }
    }
}
