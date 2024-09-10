using UnityEngine;

namespace LoadSave
{
    [System.Serializable]
    public class SerializablePlayerPosition
    {
        public float x;
        public float y;
        public float z;

        public SerializablePlayerPosition(Vector3 position)
        {
            x = position.x;
            y = position.y;
            z = position.z;
        }
        public Vector3 GetVector3()
        {
            return new Vector3(x, y, z);
        }
    
    }
}
