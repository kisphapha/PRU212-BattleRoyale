using UnityEngine;

namespace Assets.Script
{
    public class GrenadeThrowController : ThrowingBehavior
    {
        public override GameObject Throw()
        {
            int randomAngle = Random.Range(-10, 10);  // Adjust angle spread as needed
            float throwOffset = 2f;  // Adjust throw offset as needed
            Quaternion rotationRandom = Quaternion.Euler(0f, 0f, randomAngle);
            Quaternion rotationOfThrow = Quaternion.Euler(0f, 0f, grenadeEntity.holder.angle - 90);
            Quaternion rotationOfMaster = Quaternion.Euler(0f, 0f, grenadeEntity.holder.angle);
            Vector3 targetPosition = grenadeEntity.holder.transform.position + (rotationOfThrow * Vector3.right * throwOffset);
            var moverTransform = grenadeEntity.holder.mover.gameObject.transform;
            Vector3 direction = rotationRandom * moverTransform.up * -1;
            GameObject grenade = Instantiate(grenadeEntity.grenadePrefab, targetPosition, rotationOfMaster);
            Rigidbody2D grenadeRigidbody = grenade.GetComponent<Rigidbody2D>();
            grenadeRigidbody.velocity = direction * grenadeEntity.grenadeSpeed;

            return grenade;
        }
    }
}