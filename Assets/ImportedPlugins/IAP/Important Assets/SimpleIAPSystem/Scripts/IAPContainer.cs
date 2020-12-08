/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SIS
{
    /// <summary>
    /// Stretches a container rect transform to include all children.
    /// Also repositions children using the GridLayoutGroup, if found.
    /// </summary>
    public class IAPContainer : MonoBehaviour
    {
        /// <summary>
        /// Maximum value for clamping the product vertically.
        /// </summary>
        public int maxCellSizeX;

        /// <summary>
        /// Maximum value for clamping the product horizontally.
        /// </summary>
        public int maxCellSizeY;


        /// <summary>
        /// Executes the repositioning of UI elements within the grid layout.
        /// </summary>
        public IEnumerator Start()
        {
            RectTransform rectTrans = GetComponent<RectTransform>();
            GridLayoutGroup grid = GetComponent<GridLayoutGroup>();

            if (rectTrans != null && grid != null)
            {
                if (transform.childCount <= 0)
                    yield return new WaitForEndOfFrame();
                if (transform.childCount == 0)
                    yield break;

                RectTransform child = transform.GetChild(0).GetComponent<RectTransform>();

                switch (grid.startAxis)
                {
                    case GridLayoutGroup.Axis.Vertical:
                        grid.cellSize = new Vector2(rectTrans.rect.width, child.rect.height);
                        float newHeight = child.rect.height * transform.childCount;
                        newHeight += (transform.childCount - 1) * grid.spacing.y + grid.padding.top + grid.padding.bottom;
                        rectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x, newHeight);
                        break;
                    case GridLayoutGroup.Axis.Horizontal:
                        grid.cellSize = new Vector2(child.rect.width, rectTrans.rect.height);
                        float newWidth = child.rect.width * transform.childCount;
                        newWidth += (transform.childCount - 1) * grid.spacing.x + grid.padding.left + grid.padding.right;
                        rectTrans.sizeDelta = new Vector2(newWidth, rectTrans.sizeDelta.y);
                        break;
                }

                if (maxCellSizeX > 0 && grid.cellSize.x > maxCellSizeX)
                    grid.cellSize = new Vector2(maxCellSizeX, grid.cellSize.y);
                if (maxCellSizeY > 0 && grid.cellSize.y > maxCellSizeY)
                    grid.cellSize = new Vector2(grid.cellSize.x, maxCellSizeY);

                grid.enabled = true;
            }
        }
    }
}
