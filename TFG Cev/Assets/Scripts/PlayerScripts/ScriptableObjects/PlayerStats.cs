
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player/Stats")]
public class PlayerStats : ScriptableObject
{

    //Hp//
    [System.Serializable]
    public class PlayerHP{

        public float maxValue;
        public float minValue = 0;
        public float value;
    }

    [System.Serializable]
    public class PlayerStatus
    {
        public bool isGrounded = true;
        public bool isMoving = false;
        public bool isRunning = false;
        public bool isJumping = false;
        public bool isCharging = false;
        public bool isShooting = false;
        public bool isDashing = false;
        public bool isCrouching = false;
        public bool isClimbing = false;
        public bool isCarring = false;
        public bool isFox = false;
        public bool isDead = false;
    }

    
    public int ammo;
    public float baseDamage;

    public bool win;


    [Header("Statics")]
    [SerializeField]
    public PlayerHP hp;
    [SerializeField]
    public int maxAmmo;

    [Header("Status")]
    [SerializeField]
    public PlayerStatus playerStatus;

    void InitStatus()
    {
        playerStatus.isGrounded = true;
        playerStatus.isMoving = false;
        playerStatus.isRunning = false;
        playerStatus.isJumping = false;
        playerStatus.isCharging = false;
        playerStatus.isShooting = false;
        playerStatus.isDashing = false;
        playerStatus.isCrouching = false;
        playerStatus.isClimbing = false;
        playerStatus.isCarring = false;
        playerStatus.isFox = false;
        playerStatus.isDead = false;
    }

}
