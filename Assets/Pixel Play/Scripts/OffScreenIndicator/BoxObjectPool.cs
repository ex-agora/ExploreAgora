using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pixel_Play.Scripts.OffScreenIndicator
{
    public class BoxObjectPool : MonoBehaviour
    {
        public static BoxObjectPool current;

        [Tooltip("Assign the box prefab.")]
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
                var box = Instantiate(pooledObject, transform, false);
                box.Activate(false);
                pooledObjects.Add(box);
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
            var box = Instantiate(pooledObject, transform, false);
            box.Activate(false);
            pooledObjects.Add(box);
            return box;
        }

        /// <summary>
        /// Deactive all the objects in the pool.
        /// </summary>
        public void DeactivateAllPooledObjects()
        {
            foreach (var box in pooledObjects)
            {
                box.Activate(false);
            }
        }
    }
}
