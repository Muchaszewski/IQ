using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Extensions
{
    public static class TransformUtils
    {
        #region Position
        /// <summary>
        /// Set X value for given vector3
        /// </summary>
        public static Vector3 SetX(this Vector3 vector3, float x)
        {
            var newTransform = new Vector3(x, vector3.y, vector3.z);
            vector3 = newTransform;
            return vector3;
        }

        /// <summary>
        /// Set Y value for given vector3
        /// </summary>
        public static Vector3 SetY(this Vector3 vector3, float y)
        {
            var newTransform = new Vector3(vector3.x, y, vector3.z);
            vector3 = newTransform;
            return vector3;
        }

        /// <summary>
        /// Set Z value for given vector3
        /// </summary>
        public static Vector3 SetZ(this Vector3 vector3, float z)
        {
            var newTransform = new Vector3(vector3.x, vector3.y, z);
            vector3 = newTransform;
            return vector3;
        }

        /// <summary>
        /// Set XY value for given vector3
        /// </summary>
        public static Vector3 SetXY(this Vector3 vector3, float x, float y)
        {
            var newTransform = new Vector3(x, y, vector3.z);
            vector3 = newTransform;
            return vector3;
        }

        /// <summary>
        /// Set XYZ value for given vector3
        /// </summary>
        public static Vector3 SetXY(this Vector3 vector3, Vector2 vector2)
        {
            var newTransform = new Vector3(vector2.x, vector2.y, vector3.z);
            vector3 = newTransform;
            return vector3;
        }

        /// <summary>
        /// Override given vector
        /// <para>Extenstion method for new</para>
        /// </summary>
        public static Vector3 Override(this Vector3 vector3, float x, float y, float z)
        {
            var newTransform = new Vector3(x, y, z);
            vector3 = newTransform;
            return vector3;
        }

        /// <summary>
        /// Move given transform
        /// </summary>
        public static Transform Move(this Transform transform, float x, float y = 0, float z = 0)
        {
            var newTransform = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z);
            transform.position = newTransform;
            return transform;
        }

        /// <summary>
        /// Move by Vector3(0, value, 0)
        /// </summary>
        public static Transform MoveUp(this Transform transform, float val)
        {
            return transform.Move(0, val, 0);
        }

        /// <summary>
        /// Move by Vector3(0, -value, 0)
        /// </summary>
        public static Transform MoveDown(this Transform transform, float val)
        {
            return transform.Move(0, -val, 0);
        }


        /// <summary>
        /// Move by Vector3(val, 0, 0)
        /// </summary>
        public static Transform MoveRight(this Transform transform, float val)
        {
            return transform.Move(val, 0, 0);
        }

        /// <summary>
        /// Move by Vector3(-val, 0, 0)
        /// </summary>
        public static Transform MoveLeft(this Transform transform, float val)
        {
            return transform.Move(-val, 0, 0);
        }

        /// <summary>
        /// Move by Vector3(0, 0, val)
        /// </summary>
        public static Transform MoveForward(this Transform transform, float val)
        {
            return transform.Move(0, 0, val);
        }

        /// <summary>
        /// Move by Vector3(0, 0, val)
        /// </summary>
        public static Transform MoveBack(this Transform transform, float val)
        {
            return transform.Move(0, 0, -val);
        }
        #endregion

        #region Rotation
        /// <summary>
        /// Look at target Vector in world space
        /// </summary>
        /// <param name="worldPosition">Vector to look at</param>
        /// <returns></returns>
        public static Transform LookAt2D(this Transform transform, Vector2 worldPosition)
        {
            //Get this object point absolutive
            Vector3 object_pos = transform.position;
            //Calculate diffrence
            worldPosition.x = worldPosition.x - object_pos.x;
            worldPosition.y = worldPosition.y - object_pos.y;
            //Get angle
            float angle = Mathf.Atan2(worldPosition.y, worldPosition.x) * Mathf.Rad2Deg;
            //Set the rotation
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            return transform;
        }

        /// <summary>
        /// Look at target Vector in world space
        /// </summary>
        /// <param name="worldPosition">Vector to look at</param>
        /// <returns></returns>
        public static Transform LookAt2D(this Transform transform, Transform target)
        {
            return transform.LookAt2D(target.position);
        }
        #endregion
    }
}
