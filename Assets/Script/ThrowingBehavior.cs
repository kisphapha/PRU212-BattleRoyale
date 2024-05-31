using UnityEngine;

namespace Assets.Script
{
    public abstract class ThrowingBehavior : MonoBehaviour
    {
        public GrenadeEntity grenadeEntity;

        public abstract void Throw();
    }
}