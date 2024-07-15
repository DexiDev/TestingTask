using UnityEngine;

namespace Game.Extensions
{
    public struct Vector3Save
    {
        public float x;
        public float y;
        public float z;

        public Vector3 GetVector3()
        {
            return new Vector3(x, y, z);
        }
    }
    public static class Vector3Extensions
    {
        public static Vector3Save ToSave(this Vector3 vector3)
        {
            return new Vector3Save()
            {
                x = vector3.x,
                y = vector3.y,
                z = vector3.z
            };
        }
    }
}