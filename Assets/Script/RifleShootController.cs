using UnityEngine;
using UnityEngine.Timeline;

namespace Assets.Script

{
    public class RifleShootController : ShootingBehavior
    {     
        public override void Shoot()
        {
            int randomAngle = Random.Range(-10, 10);
            float gunOffset = 4f;
            Quaternion rotationRandom = Quaternion.Euler(0f, 0f, randomAngle);
            Quaternion rotationOfGun = Quaternion.Euler(0f, 0f, gunEntity.holder.angle - 90);
            Quaternion rotationOfMaster = Quaternion.Euler(0f, 0f, gunEntity.holder.angle);
            Vector3 targetPosition = gunEntity.holder.transform.position + (rotationOfGun * Vector3.right * gunOffset);
            Vector3 direction = rotationRandom * gunEntity.holder.transform.up * -1;
            GameObject bullet = Instantiate(gunEntity.bullet, targetPosition, rotationOfMaster);
            Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            bulletRigidbody.velocity = direction * gunEntity.bulletSpeed;
        }
    }
}