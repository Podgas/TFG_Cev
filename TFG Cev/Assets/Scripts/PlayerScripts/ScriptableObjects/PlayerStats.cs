
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
        public bool isCharging = false;
        public bool isShooting = false;
        public bool isDashing = false;
        public bool isClimbing = false;
        public bool isFox = false;
        public bool isDead = false;

        public bool canClimb = false;

        public bool jumpPressed = false;
        public bool interactPressed = false;
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

    public void InitStatus()
    {
        playerStatus.isGrounded = true;
        playerStatus.isMoving = false;
        playerStatus.isRunning = false;
        playerStatus.isCharging = false;
        playerStatus.isShooting = false;
        playerStatus.isDashing = false;
        playerStatus.isClimbing = false;
        playerStatus.isFox = false;
        playerStatus.isDead = false;

        playerStatus.canClimb = false;

        playerStatus.jumpPressed = false;
        playerStatus.interactPressed = false;
    }

}
