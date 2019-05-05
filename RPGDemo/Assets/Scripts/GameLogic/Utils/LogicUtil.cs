using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class LogicUtil
    {
        public static List<T> ArrayToList<T>(T[] arr)
        {
            if (arr == null)
            {
                return null;
            }
            int arrayLength = arr.Length;
            List<T> targetList = new List<T>(arrayLength);
            T item = default(T);
            for (int index = 0; index < arrayLength; ++index)
            {
                item = arr[index];
                targetList.Add(item);
            }
            return targetList;
        }

        public static void CopyListContent<T>(IList<T> srcList, IList<T> dstList)
        {
            if (srcList == null || dstList == null)
            {
                return;
            }
            if (srcList.Count != dstList.Count)
            {
                return;
            }
            dstList.Clear();
            T srcObj = default(T);
            for (int index = 0; index < srcList.Count; ++index)
            {
                srcObj = srcList[index];
                if (srcObj == null)
                {
                    continue;
                }
                dstList.Add(srcObj);
            }
        }

        public static void Rotate2DVector(ref Vector2 direction, float angle)
        {
            float radians = (float)Math.PI * angle / 180.0f;
            float sinVector = (float)Math.Sin(radians);
            float cosVector = (float)Math.Cos(radians);

            float direcX = direction.x;
            float direcY = direction.y;
            direction.x = direcX * cosVector - direcY * sinVector;
            direction.y = direcX * sinVector + direcY * cosVector;
        }

        public static void Rotate3DVector(Vector3 direction, float angle)
        {

        }

    }
}
