using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;

namespace Assets.Script
{
    public class GrenadeThrowController : ThrowingBehavior
    {
        public override GameObject Throw()
        {
            Vector3 direction, targetPosition;
            Quaternion rotationOfMaster;
            float throwOffset = 2f;
            int randomAngle = Random.Range(-10, 10); 
            //AI behavior
            if (grenadeEntity.holderAI != null)
            {
                // Adjust throw offset as needed
                var AIBehavior = grenadeEntity.holderAI.GetComponent<AIBehavior>();
                Quaternion rotationRandom = Quaternion.Euler(0f, 0f, randomAngle);
                Quaternion rotationOfThrow = Quaternion.Euler(0f, 0f, AIBehavior.angle);
                rotationOfMaster = Quaternion.Euler(0f, 0f, AIBehavior.angle);
                targetPosition = grenadeEntity.holderAI.transform.position + (rotationOfThrow * Vector3.right * throwOffset);
                var moverTransform = grenadeEntity.holderAI.transform;
                direction = rotationRandom * moverTransform.right * -1;            
            } 
            //Player Behavior
            else
            {
                Quaternion rotationRandom = Quaternion.Euler(0f, 0f, randomAngle);
                Quaternion rotationOfThrow = Quaternion.Euler(0f, 0f, grenadeEntity.holder.angle - 90);
                rotationOfMaster = Quaternion.Euler(0f, 0f, grenadeEntity.holder.angle);
                targetPosition = grenadeEntity.holder.transform.position + (rotationOfThrow * Vector3.right * throwOffset);
                var moverTransform = grenadeEntity.holder.mover.gameObject.transform;
                direction = rotationRandom * moverTransform.up * -1;
            }
            GameObject grenade = PhotonNetwork.Instantiate(grenadeEntity.grenadePrefab.name, targetPosition, rotationOfMaster);
            Rigidbody2D grenadeRigidbody = grenade.GetComponent<Rigidbody2D>();
            grenadeRigidbody.velocity = direction * grenadeEntity.grenadeSpeed;
            return grenade;
        }
    }
}