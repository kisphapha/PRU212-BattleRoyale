using System;
using UnityEngine;
using V_AnimationSystem;
using CodeMonkey.Utils;

/*
 * Player movement with Arrow keys
 * Attack with Space
 * */
public class Player : MonoBehaviour {
    
    public static Player instance;

    private const float SPEED = 50f;

    private Player_Base playerBase;
    private State state;

    private enum State {
        Normal,
        Attacking,
    }

    private void Awake() {
        instance = this;
        playerBase = gameObject.GetComponent<Player_Base>();
        SetStateNormal();
    }

    private void Update() {
        switch (state) {
        case State.Normal:
            HandleMovement();
            HandleAttack();
            break;
        case State.Attacking:
            HandleAttack();
            break;
        }
    }
    
    private void SetStateNormal() {
        state = State.Normal;
    }

    private void SetStateAttacking() {
        state = State.Attacking;
    }

    private void HandleMovement() {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W)) {
            moveY = +1f;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A)) {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveX = +1f;
        }

        Vector3 moveDir = new Vector3(moveX, moveY).normalized;
        bool isIdle = moveX == 0 && moveY == 0;
        if (isIdle) {
            playerBase.PlayIdleAnim();
        } else {
            playerBase.PlayMoveAnim(moveDir);
            transform.position += moveDir * SPEED * Time.deltaTime;
        }
    }

    private void HandleAttack() {
        if (Input.GetMouseButtonDown(0)) {
            // Attack
            SetStateAttacking();
            
            Vector3 attackDir = (UtilsClass.GetMouseWorldPosition() - GetPosition()).normalized;

            EnemyHandler enemyHandler = EnemyHandler.GetClosestEnemy(GetPosition() + attackDir * 4f, 20f);
            if (enemyHandler != null) {
                //enemyHandler.Damage(this);
                attackDir = (enemyHandler.GetPosition() - GetPosition()).normalized;
                transform.position = enemyHandler.GetPosition() + attackDir * -12f;
            } else {
                transform.position = transform.position + attackDir * 4f;
            }

            Transform swordSlashTransform = Instantiate(GameAssets.i.pfSwordSlash, GetPosition() + attackDir * 13f, Quaternion.Euler(0, 0, UtilsClass.GetAngleFromVector(attackDir)));
            swordSlashTransform.GetComponent<SpriteAnimator>().onLoop = () => Destroy(swordSlashTransform.gameObject);

            UnitAnimType activeAnimType = playerBase.GetUnitAnimation().GetActiveAnimType();
            if (activeAnimType == GameAssets.UnitAnimTypeEnum.dSwordTwoHandedBack_Sword) {
                swordSlashTransform.localScale = new Vector3(swordSlashTransform.localScale.x, swordSlashTransform.localScale.y * -1, swordSlashTransform.localScale.z);
                playerBase.GetUnitAnimation().PlayAnimForced(GameAssets.UnitAnimTypeEnum.dSwordTwoHandedBack_Sword2, attackDir, 1f, (UnitAnim unitAnim) => SetStateNormal(), null, null);
            } else {
                playerBase.GetUnitAnimation().PlayAnimForced(GameAssets.UnitAnimTypeEnum.dSwordTwoHandedBack_Sword, attackDir, 1f, (UnitAnim unitAnim) => SetStateNormal(), null, null);
            }
        }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }
        
}
