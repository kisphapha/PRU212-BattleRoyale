using UnityEngine;

namespace Assets.Script
{
    public class GrenadeThrowController : ThrowingBehavior
    {
        public override void Throw()
        {
            int randomAngle = Random.Range(-10, 10);  // Adjust angle spread as needed
            float throwOffset = 1.5f;  // Adjust throw offset as needed
            Quaternion rotationRandom = Quaternion.Euler(0f, 0f, randomAngle);
            Quaternion rotationOfThrow = Quaternion.Euler(0f, 0f, grenadeEntity.holder.angle - 90);
            Quaternion rotationOfMaster = Quaternion.Euler(0f, 0f, grenadeEntity.holder.angle);
            Vector3 targetPosition = grenadeEntity.holder.transform.position + (rotationOfThrow * Vector3.right * throwOffset);
            Vector3 direction = rotationRandom * grenadeEntity.holder.transform.up;
            GameObject grenade = Instantiate(grenadeEntity.grenadePrefab, targetPosition, rotationOfMaster);
            Rigidbody2D grenadeRigidbody = grenade.GetComponent<Rigidbody2D>();
            grenadeRigidbody.velocity = direction * grenadeEntity.grenadeSpeed;
        }
    }
}