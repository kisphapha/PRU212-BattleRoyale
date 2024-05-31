using UnityEngine;

namespace Assets.Script
{
    public abstract class ThrowingBehavior : MonoBehaviour
    {
        public GrenadeEntity grenadeEntity;

        private void Start()
        {
            grenadeEntity = GetComponent<GrenadeEntity>();
        }
        public abstract void Throw();
    }
}