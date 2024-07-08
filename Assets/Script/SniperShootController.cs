using UnityEngine;

namespace Assets.Script

{
    public class SniperShootController : ShootingBehavior
    {
        private CameraController cameraController;
        public override void Start()
        {
            base.Start();
            cameraController = gameObject.AddComponent<CameraController>();
        }
        private void Update()
        {
            if (cameraController != null && gunEntity.canShoot)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    cameraController.setIsZoomingIn(true);
                }

                if (Input.GetMouseButtonUp(1))
                {
                    cameraController.setIsZoomingIn(false);
                }
            }
        }
        public override void OnDrop()
        {
            if (cameraController != null)
            {
                cameraController.setIsZoomingIn(false);
            }
        }
        public override void Shoot()
        {
            int randomAngle = Random.Range(0, 0);
            float gunOffset = 4f;
            if (gunEntity.holderAI != null)
            {
                var AIBehavior = gunEntity.holderAI.GetComponent<AIBehavior>();
                Quaternion rotationRandom = Quaternion.Euler(0f, 0f, randomAngle);
                Quaternion rotationOfGun = Quaternion.Euler(0f, 0f, AIBehavior.angle);
                Quaternion rotationOfMaster = Quaternion.Euler(0f, 0f, AIBehavior.angle - 90);
                Vector3 targetPosition = gunEntity.holderAI.transform.position + (rotationOfGun * Vector3.right * gunOffset);
                var moverTransform = gunEntity.holderAI.gameObject.transform;
                Vector3 direction = rotationRandom * moverTransform.right * -1;
                GameObject bullet = Instantiate(gunEntity.bullet, targetPosition, rotationOfMaster);
                Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                bulletRigidbody.velocity = direction * gunEntity.bulletSpeed;
            }
            else
            {
                Quaternion rotationRandom = Quaternion.Euler(0f, 0f, randomAngle);
                Quaternion rotationOfGun = Quaternion.Euler(0f, 0f, gunEntity.holder.angle - 90);
                Quaternion rotationOfMaster = Quaternion.Euler(0f, 0f, gunEntity.holder.angle);
                Vector3 targetPosition = gunEntity.holder.transform.position + (rotationOfGun * Vector3.right * gunOffset);
                var moverTransform = gunEntity.holder.mover.gameObject.transform;
                Vector3 direction = rotationRandom * moverTransform.up * -1;
                GameObject bullet = Instantiate(gunEntity.bullet, targetPosition, rotationOfMaster);
                Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                bulletRigidbody.velocity = direction * gunEntity.bulletSpeed * 2;
            }




        }
    }
}