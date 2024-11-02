using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("<color=red>=====�S��===============</color>")]
    //Gravity
    [Header("�y������X�����x�́Z�z"), SerializeField, Range(-120f, 0f)] private float gravity;
    [Header("�y������X�ō����x�́Z�z"), SerializeField, Range(0f, 120f)] private float fallingMaxSpeed;

    // HP
    [Header("�y�v���C���[��X�ő�̗͂́Z�z"), SerializeField, Range(1, 300)]private int maxHP;
    private int currentHP;
    [SerializeField] private TMP_Text hpText;

    // Damage
    [Header("�y�_���[�W��X�d�����Ԃ́Z�b�z"), SerializeField, Range(0f, 5f)] private float secondsForDamageFreeze;
    [Header("�y�_���[�W��X���G���Ԃ́Z�b�z"), SerializeField, Range(0f, 5f)] private float secondsForDamageInvincible;
    private float elapsedSeconds_Damage;
    /// <summary>
    /// �_���[�W��A���G��Ԃ̊�true��Ԃ�
    /// </summary>
    private bool isInvincible_Damage = false;
    /// <summary>
    /// �v���C���[�����炩�̌`�Ŗ��G��Ԃ̊�true��Ԃ�
    /// </summary>
    private bool isPlayerInvincible = false;
    public bool IsPlayerInvincible => isPlayerInvincible;
    /// <summary>
    /// �_���[�W��A�_�b�V�����A����/���㒆�A�d����Ԃ̊�true��Ԃ��B
    /// true�̊Ԃ͓��͂��󂯕t���Ȃ�
    /// </summary>
    private bool isFreeze;
    [Header("�y�Q�[���I�[�o�[�̈ꎞ��~�́Z�b�z"), SerializeField, Range(0.01f, 5f)]private float pauseTime;

    // Dash
    private float elapsedSeconds_Dash;
    /// <summary>
    /// elapedSeconds_Dash�����Z�����true��Ԃ�
    /// </summary>
    private bool isElapsed_Dash = false;
    private Vector3 currentPos_Dash;
    private Vector3 targetPos_Dash;
    private Vector2 dashVector = Vector2.zero;
    /// <summary>
    /// �_�b�V������true��Ԃ�
    /// </summary>
    private bool isDash = false;
    /// <summary>
    /// �_�b�V���J�n�\�Ȋ�true��Ԃ�
    /// </summary>
    private bool canDash = true;
    /// <summary>
    /// �_�b�V���Ŗ��G���������Ă����true��Ԃ�
    /// </summary>
    private bool isInvincible_Dash = false;
    /// <summary>
    /// �_�b�V���ŏՓ˒���true��Ԃ�
    /// </summary>
    private bool isHit_Dash = false;
    private float secondsForDash;
    private float dashDistance;
    private float secondsForDashCT;
    private float secondsForDashInvincible;

    // Attack
    private float secondsForAttack_before;
    private float secondsForAttacking;
    private float secondsForAttack;
    private float secondsForAttackCT;
    private float elapsedSeconds_Attack;

    // Fix
    /// <summary>
    /// �Œ蒆��true��Ԃ�
    /// </summary>
    private bool isFixed;
    /// <summary>
    /// �Œ�{�^���������Ă����true��Ԃ�
    /// </summary>
    private bool isFixButtonPushing = false;

    // Transfer
    [SerializeField] private Vector2 transferTargetPos;
    [SerializeField] private Environment transferTargetPosEnvironment;



    [Header("<color=red>=====����===============</color>")]
    // Walk
    [Header("�y�ړ���X���x�́Z�z"), SerializeField, Range(0.1f, 30f)] private float walkSpeed;
    private InputAction walk;
    private Vector2 inputWalkAxis;
    [Header("�y�ړ��̓��͖�����X���l�́Z�z"), SerializeField, Range(0f, 0.5f)] private float ignoreInput_Walk;

    // Jump
    [Header("�y�W�����v��X�����x�́Z�z"), SerializeField, Range(0.5f, 120f)] private float jumpSpeed;
    private float appliedJumpSpeed;
    [Header("�W�����v��X�ō����x�́Z"), SerializeField, Range(0.1f, 30f)] private float jumpMaxHeight;
    private float limitYPos;
    /// <summary>
    /// Ground, Surface, OwBlock�^�O�̃I�u�W�F�N�g�ɏ���Ă����true��Ԃ�
    /// </summary>
    [SerializeField]private bool onGround = true;
    /// <summary>
    /// �W�����v�\�Ȋ�true��Ԃ�
    /// </summary>
    private bool canJump = true;
    /// <summary>
    /// �W�����v�ŏ㏸����true��Ԃ�
    /// </summary>
    private bool isJump = false;
    [Header("�y�ǃW�����v��X�񐔂́Z�z"), SerializeField, Range(0, 12)] private int wallJumpMaxNum;
    private int wallJumpCount;
    [SerializeField]private TMP_Text wallJumpCountText;
    [Header("�ǃW�����v��X�����x�́Z"), SerializeField, Range(0.5f, 120f)] private float wallJumpSpeed;
    private float appliedWallJumpSpeed;
    [Header("�y�ǃW�����v��X�����́Z�b�z"), SerializeField, Range(0f, 5f)] private float secondsForWallJump;
    private float elapsedSeconds_wallJump;
    /// <summary>
    /// Ground, Surface�^�O�̃I�u�W�F�N�g�̐����ʂƐڂ��Ă���Ԃ�true��Ԃ�
    /// </summary>
    [SerializeField]private bool atWall;
    /// <summary>
    /// �ǃW�����v�����Ă����true��Ԃ�
    /// </summary>
    private bool isWallJump;
    /// <summary>
    /// �W�����v�{�^���������Ă����true��Ԃ�
    /// </summary>
    private bool isJumpButtonPushing = false;
    [Header("�y�󒆃W�����v��X�P�\�́Z�b�z"), SerializeField, Range(0f, 3f)] private float airJumpBonusSeconds;
    private float elapsedSeconds_canJump;
    /// <summary>
    /// elapsedSeconds_canJump�����Z�����true��Ԃ�
    /// </summary>
    private bool isElapsed_Jump = false;

    // Easing
    [Header("�������n�߂�X�W�����v�̐i�s��́Z"), SerializeField, Range(0.1f, 1f)] private float easingStartRate;
    [Header("�W�����v��X�����̋����́Z"), SerializeField, Range(0.1f, 10f)] private float easingForce;

    // Dash
    [Header("�y�_�b�V����X�S�̂́Z�b�z"), SerializeField, Range(0f, 5f)] private float secondsForDash_Ground;
    [Header("�y�_�b�V����X�����́Z�z"), SerializeField, Range(0.1f, 100f)] private float dashDistance_Ground;
    [Header("�y�_�b�V��/�X���C�f�B���O��X�N�[���^�C���́Z�b�z"), SerializeField, Range(0f, 5f)] private float secondsForDashCT_Ground;
    [Header("�y�_�b�V�����G��X�����́Z�b�z"), SerializeField, Range(0f, 5f)] private float secondsForDashInvincible_Ground;
    [Header("�y�_�b�V����X�㗎���́Z�z"), SerializeField, Range(0.1f, 10f)] private float fallPowerAfterDash;
    [Header("�y�_�b�V����X�󒆉񐔂́Z�z"), SerializeField, Range(1, 5)] private int airDashMaxNum;
    private int airDashCount;
    [SerializeField] private TMP_Text airDashCountText;

    // Squat
    [Header("�y���Ⴊ�݂�X���͐[�x�́Z�z"), SerializeField, Range(0.1f, 1f)] private float ActiveSquat_Y;
    [Header("�y���Ⴊ�݂�X���͊p�x�́Z�z"), SerializeField, Range(0.1f, 1f)] private float ActiveSquat_X;
    [Header("�y���Ⴊ�ݔ����X�����́Z�z"), SerializeField] private Vector2 colliderSize_Squat;
    [Header("���Ⴊ�ݔ����X���S�ʒu�́Z"), SerializeField] private Vector2 colliderOffset_Squat;
    /// <summary>
    /// ���Ⴊ�ݒ���true��Ԃ�
    /// </summary>
    private bool isSquat;

    // Sliding
    [Header("�y�X���C�f�B���O��X�S�̂́Z�b�z"), SerializeField, Range(0f, 5f)] private float secondsForSliding;
    [Header("�y�X���C�f�B���O��X�����́Z�z"), SerializeField, Range(0.1f, 100f)] private float slidingDistance;
    [Header("�y�X���C�f�B���O���G��X�����́Z�b�z"), SerializeField, Range(0f, 5f)] private float secondsForSlidingInvincible;

    // Attack
    [Header("�y�U����X�O�X�L�́Z�b�z"), SerializeField, Range(0f, 1f)] private float secondsForGroundAttack_before;
    [Header("�y�U����X�����́Z�b�z"), SerializeField, Range(0f, 1f)] private float secondsForGroundAttacking;
    [Header("�y�U����X�S�̂́Z�b�z"), SerializeField, Range(0f, 3f)] private float secondsForGroundAttack;
    [Header("�y�U����X�N�[���^�C���́Z�b�z"), SerializeField, Range(0f, 5f)] private float secondsForGroundAttackCT;
    /// <summary>
    /// �U�����͂���y�U����X�N�[���^�C���́Z�b�z�o�߂���Ԃ�false��Ԃ�
    /// </summary>
    private bool canAttack = true;
    /// <summary>
    /// �U����(�U�����͂���y�U����X�S�̂́Z�b�z�o�߂����)��true��Ԃ�
    /// </summary>
    private bool isAttack = false;
    /// <summary>
    /// �U�������U�����͂���y�U����X�O�X�L�́Z�b�z�o�߂�����true��Ԃ�
    /// </summary>
    private bool isElapsed_AttackBefore = false;
    /// <summary>
    /// �U�������U�����͂���y�U����X�O�X�L�́Z�b�z+�y�U����X�����́Z�b�z�o�߂�����true��Ԃ�
    /// </summary>
    private bool isElapsed_Attacking = false;
    private Vector2 attackDirection;

    // KnockBack
    [Header("�y�m�b�N�o�b�N��X�����x�́Z�z"), SerializeField, Range(0f, 1000f)] private float knockBackPower;
    [Header("�y�m�b�N�o�b�N��X�p�x�́Z�z"), SerializeField, Range(0f, 90f)] private float knockBackAngle;
    //[Header("�m�b�N�o�b�N��X�����J�n�́Z�b��"), SerializeField, Range(0f, 1f)] private float knockBackWeakeningStartSecond_Ground;
    [Header("�y�m�b�N�o�b�N��X�������n�߂�i�s��́Z�z"), SerializeField, Range(0f, 1f)] private float knockBackWeakeningStartRate;
    [Header("�y�m�b�N�o�b�N��X�����̋����́Z�z"), SerializeField, Range(0f, 1f)] private float knockBackWeakeningPower_Ground;
    [Header("�m�b�N�o�b�N��X���������́Z�b"), SerializeField, Range(0f, 3f)] private float secondsForKnockBackWeakening_Ground;
    /// <summary>
    /// �n��m�b�N�o�b�N����true��Ԃ�
    /// </summary>
    private bool isKnockBack_Ground;
    /// <summary>
    /// �n��m�b�N�o�b�N�Ō�������true��Ԃ�
    /// </summary>
    private bool isWeakening_Ground;
    private float knockBackSpeed_Ground;
    private float elapsedSeconds_GroundKnockBack;
    private float enemyRelativeXPos;
    private bool isFinishedFirstAction;


    // ChangeEnvironment
    private Vector2 currentPos_ChangeEnvironment;
    private Vector3 targetPos_ChangeEnvironment;
    [HideInInspector]public Vector2 TargetPos { get { return targetPos_ChangeEnvironment; }set { targetPos_ChangeEnvironment = value; } }
    private float elapsedSeconds_ChangeEnvironment;
    private float usedSeconds_before = 0;
    private float usedSeconds_move = 0;
    private float usedSeconds_after = 0;
    private float rate;
    private float elapsedSeconds_InvincibleByChangeEnironment;
    private float usedSeconds_beforeInvincible;
    private float usedSeconds_Invincible;
    private bool isSetInvincible = false;
    /// <summary>
    /// ����/����ňړ�����true��Ԃ�
    /// </summary>
    private bool isChanging = false;
    // EnterTheWater
    [ Header("<color=red>=====����/����===============</color>")]
    [Header("�y������X�ړ����ԑO�́Z�b�z"), SerializeField, Range(0f, 3f)] private float secondsForEnter_before;
    [Header("�y������X�ړ����Ԃ́Z�b�z"), SerializeField, Range(0f, 5f)] private float secondsForEnter;
    [Header("�y������X�ړ����Ԍ�́Z�b�z"), SerializeField, Range(0f, 3f)] private float secondsForEnter_after;
    /// <summary>
    /// �����\�Ȋ�true��Ԃ�
    /// </summary>
    [Tooltip("Surface/buffer�Œ����\")] public bool CanEnterTheWater = false;
    /// <summary>
    /// �ړ��O��̍d�����܂ސ�������true��Ԃ�
    /// </summary>
    private bool isEntering = false;
    [HideInInspector] public bool IsEntering => isEntering;
    [Header("�y������X���G�J�n�́Z�b��z"), SerializeField, Range(0.01f, 1f)] private float secondsForBeforeEnterInvincible;
    [Header("�y������X���G�����́Z�b�z"), SerializeField, Range(0.01f, 5f)] private float secondsForEnterInvincible;
    /// <summary>
    /// �����ɂ�閳�G����true��Ԃ�
    /// </summary>
    private bool isInvincible_Enter;

    // GoUpToGround
    [Header("�y�����X�ړ����ԑO�́Z�b�z"), SerializeField, Range(0f, 3f)] private float secondsForGoUp_before;
    [Header("�y�����X�ړ����Ԃ́Z�b�z"), SerializeField, Range(0f, 5f)] private float secondsForGoUp;
    [Header("�y�����X�ړ����Ԍ�́Z�b�z"), SerializeField, Range(0f, 3f)] private float secondsForGoUp_after;
    /// <summary>
    /// ����\�Ȋ�true��Ԃ�
    /// </summary>
    [Tooltip("Surface/buffer�Œ����\")]public bool CanGoUpToGround = false;
    /// <summary>
    /// �ړ��O��̍d�����܂ޕ��㒆��true��Ԃ�
    /// </summary>
    private bool isGoingUp = false;
    [HideInInspector] public bool IsGoingUp => isGoingUp;
    [Header("�y�����X���G�J�n�́Z�b��z"), SerializeField, Range(0.01f, 1f)] private float secondsForBeforeGoUpInvincible;
    [Header("�y�����X���G�����́Z�b�z"), SerializeField, Range(0.01f, 5f)] private float secondsForGoUpInvincible;
    /// <summary>
    /// ����ɂ�閳�G����true��Ԃ�
    /// </summary>
    private bool isInvincible_GoUp;




    [Header("<color=red>=====����===============</color>")]
    // Fall
    [Header("�y����������X�����x�́Z�z"), SerializeField, Range(0.001f, 0.01f)] private float fallAcceleration;
    [Header("�y����������X�ō����x�́Z�z"), SerializeField, Range(0.1f, 10f)] private float fallingMaxSpeed_Water;
    private float fallSpeed_Water;
    
    // Swim
    [Header("�y�V�j��X���x�́Z�z"), SerializeField, Range(0.1f, 30f)] private float swimSpeed;
    private InputAction swim;
    private Vector2 inputSwimAxis;
    [Header("�y�V�j�̓��͖�����X���l�́Z�z"), SerializeField, Range(0f, 0.5f)] private float ignoreInput_Swim;
    
    // Dash
    [Header("�y�����_�b�V����X�S�̂́Z�b�z"), SerializeField, Range(0f, 5f)] private float secondsForDash_Water;
    [Header("�y�����_�b�V����X�����́Z�z"), SerializeField, Range(0.1f, 100f)] private float dashDistance_Water;
    [Header("�y�����_�b�V����X�N�[���^�C���́Z�b�z"), SerializeField, Range(0f, 5f)] private float secondsForDashCT_Water;
    [Header("�y�����_�b�V�����G��X�����́Z�b�z"), SerializeField, Range(0f, 5f)] private float secondsForDashInvincible_Water;

    // Attack
    [Header("�y�����U����X�O�X�L�́Z�b�z"), SerializeField, Range(0f, 1f)] private float secondsForWaterAttack_before;
    [Header("�y�����U����X�����́Z�b�z"), SerializeField, Range(0f, 1f)] private float secondsForWaterAttacking;
    [Header("�y�����U����X�S�̂́Z�b�z"), SerializeField, Range(0f, 3f)] private float secondsForWaterAttack;
    [Header("�y�����U����X�N�[���^�C���́Z�b�z"), SerializeField, Range(0f, 5f)] private float secondsForWaterAttackCT;

    // KnockBack
    [Header("�y�����m�b�N�o�b�N��X���x�́Z�z"), SerializeField, Range(0f, 50f)] private float waterKnockBackSpeed_First;
    [Header("�y�����m�b�N�o�b�N��X�����J�n�́Z�b�z"), SerializeField, Range(0f, 3f)] private float knockBackWeakeningStartSecond_Water;
    [Header("�y�����m�b�N�o�b�N��X�����̋����́Z�z"), SerializeField, Range(0f, 10f)] private float knockBackWeakeningPower_Water;
    [Header("�y�����m�b�N�o�b�N��X���������́Z�b�z"), SerializeField, Range(0f, 3f)] private float secondsForKnockBackWeakening_Water;
    private float elapsedSeconds_WaterKnockBack;
    private Vector2 waterKnockBackAngle;
    private float waterKnockBackSpeed;
    /// <summary>
    /// �����m�b�N�o�b�N����true��Ԃ�
    /// </summary>
    private bool isKnockBack_Water;
    /// <summary>
    /// �����m�b�N�o�b�N�Ō�������true��Ԃ�
    /// </summary>
    private bool isWeakening_Water = false;



    [Header("<color=red>=====���̑�===============</color>")]
    [SerializeField] private Environment currentEnvironment;
    public string CurrentEnvironment => currentEnvironment.ToString();
    //private static Player instance;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private Transform myTransform;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D myCollider;
    [Header("�f�t�H���g��X�R���C�_�[�T�C�Y�́Z"), SerializeField] private Vector2 colliderSize_default = new Vector2(0.55f, 1.75f);
    [Header("�f�t�H���g��X�R���C�_�[�̒��S�ʒu�́Z"), SerializeField] private Vector2 colliderOffset_default = new Vector2(0.03f, -0.34f);
    [SerializeField] private SpriteRenderer myRenderer;
    [SerializeField] private Sprite TestPlayerSprite_default;
    [SerializeField] private Sprite TestPlayerSprite_squat;
    [SerializeField] private Sprite TestPlayerSprite_invincible;
    [SerializeField] private Sprite TestPlayerSprite_squatInvincible;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private LayerMask environmentLayer;
    private ActionButtonTarget actionButtonTarget;
    [SerializeField] private SpriteRenderer actionIcon;
    [Header("�I�t�L�_�V�̕\���ʒu�́Z"), SerializeField] private Vector3 iconPosOffset;
    private GameObject goal;

    /// <summary>
    /// ��l��������ɂ��邩�����ɂ��邩
    /// </summary>
    private enum Environment
    {
        Ground,
        Water
    }

    private void Awake()
    {
        playerData.Init(this, myCollider);
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.gravity = new Vector2(0, gravity);
        myCollider.size = colliderSize_default;
        myCollider.offset = colliderOffset_default;
        GameObject start = GameObject.FindGameObjectWithTag("Start");
        if(start != null)
        {
            myTransform.position = start.transform.position + new Vector3(0f, start.transform.localScale.y * 0.5f + myCollider.size.y * 0.5f + myCollider.offset.y * 0.5f, 0f);
        }
        currentHP = maxHP;
        hpText.text = "HP:" + currentHP;
        currentEnvironment = Environment.Ground;
        walk = playerInput.actions["Walk"];
        swim = playerInput.actions["Swim"];
    }

    private void OnValidate()
    {
        if(myCollider.size != colliderSize_default)
        {
            myCollider.size = colliderSize_default;
        }
        if (myCollider.offset != colliderOffset_default)
        {
            myCollider.offset = colliderOffset_default;
        }
        // �G���[���O
        if (jumpSpeed > fallingMaxSpeed)
        {
            jumpSpeed = fallingMaxSpeed;
            Debug.LogError("�y�W�����v��X�����x�́Z�z�́y������X�ō����x�́Z�z���z���Ȃ��悤�ɐݒ肵�Ă�������");
        }

        if (secondsForDash_Ground > secondsForDashCT_Ground)
        {
            Debug.LogError("�y�_�b�V����X�S�̂́Z�b�z���y�_�b�V��/�X���C�f�B���O��X�N�[���^�C���́Z�b�z���傫���Ȃ�悤�ݒ肵�Ă�������");
        }
        else if (secondsForDash_Water > secondsForDashCT_Water)
        {
            Debug.LogError("�y�����_�b�V����X�S�̂́Z�b�z���y�����_�b�V����X�N�[���^�C���́Z�b�z���傫���Ȃ�悤�ݒ肵�Ă�������");
        }
        else if (secondsForSliding > secondsForDashCT_Ground)
        {
            Debug.LogError("�y�X���C�f�B���O��X�S�̂́Z�b�z���y�_�b�V��/�X���C�f�B���O��X�N�[���^�C���́Z�b�z���傫���Ȃ�悤�ݒ肵�Ă�������");
        }

        if(secondsForGroundAttack_before + secondsForGroundAttacking > secondsForGroundAttack)
        {
            secondsForGroundAttack = secondsForGroundAttack_before + secondsForGroundAttacking;
            Debug.LogError("�y�U����X�S�̂́Z�b�z�́y�U����X�O�X�L�́Z�b�z�Ɓy�U����X�����́Z�b�z�̍��v�ȏ�ł���K�v������܂��B");
        }

        if (secondsForWaterAttack_before + secondsForWaterAttacking > secondsForWaterAttack)
        {
            secondsForWaterAttack = secondsForWaterAttack_before + secondsForWaterAttacking;
            Debug.LogError("�y�����U����X�S�̂́Z�b�z�́y�����U����X�O�X�L�́Z�b�z�Ɓy�����U����X�����́Z�b�z�̍��v�ȏ�ł���K�v������܂��B");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((currentEnvironment == Environment.Water && inputSwimAxis != Vector2.zero) || isEntering || isGoingUp)
        {
            if(Physics2D.gravity.y != 0)
            {
                Physics2D.gravity = Vector2.zero;
            }
        }
        else
        {
            if (Physics2D.gravity.y != gravity)
            {
                Physics2D.gravity = new Vector2(0, gravity);
            }
        }

        if(isInvincible_Damage || isInvincible_Dash || isInvincible_Enter || isInvincible_GoUp)
        {
            if (!isPlayerInvincible)
            {
                isPlayerInvincible = true;
            }
        }
        else if (isPlayerInvincible)
        {
            isPlayerInvincible = false;
        }

        // �摜�؂�ւ�
        if (isPlayerInvincible)
        {
            if (isSquat)
            {
                if(myRenderer.sprite != TestPlayerSprite_squatInvincible)
                {
                    myRenderer.sprite = TestPlayerSprite_squatInvincible;
                }
            }
            else
            {
                if (myRenderer.sprite != TestPlayerSprite_invincible)
                {
                    myRenderer.sprite = TestPlayerSprite_invincible;
                }
            }
        }
        else
        {
            if (isSquat)
            {
                if (myRenderer.sprite != TestPlayerSprite_squat)
                {
                    myRenderer.sprite = TestPlayerSprite_squat;
                }
            }
            else
            {
                if (myRenderer.sprite != TestPlayerSprite_default)
                {
                    myRenderer.sprite = TestPlayerSprite_default;
                }
            }
        }

        if (isEntering || isGoingUp)
        {
            ChangeEnvironment();
        }
        if (isSetInvincible)
        {
            SetInvincibleChangeEnvironment();
        }

        if (isElapsed_Dash)
        {
            elapsedSeconds_Dash += Time.deltaTime;

            if(secondsForDashCT <= elapsedSeconds_Dash && !canDash)
            {
                canDash = true;
            }

            if(secondsForDashInvincible <= elapsedSeconds_Dash && isInvincible_Dash)
            {
                isInvincible_Dash = false;
            }

            if (elapsedSeconds_Dash >= Mathf.Max(secondsForDash, secondsForDashCT, secondsForDashInvincible))
            {
                isElapsed_Dash = false;
            }
        }

        if (secondsForAttack > secondsForAttackCT)
        {
            if (isAttack)
            {
                Attack();
            }
        }
        else
        {
            if (!canAttack)
            {
                Attack();
            }
        }

        if (isKnockBack_Ground && isFinishedFirstAction)
        {
            if (elapsedSeconds_GroundKnockBack / secondsForKnockBackWeakening_Ground >= knockBackWeakeningStartRate)
            {
                knockBackSpeed_Ground = rb.velocity.x;
                isWeakening_Ground = true;
            }
            if (elapsedSeconds_GroundKnockBack >= secondsForKnockBackWeakening_Ground)
            {
                isKnockBack_Ground = false;
                isWeakening_Ground = false;
            }
            elapsedSeconds_GroundKnockBack += Time.deltaTime;
        }

        if (isKnockBack_Water)
        {
            if (!isWeakening_Water)
            {
                if (elapsedSeconds_WaterKnockBack >= knockBackWeakeningStartSecond_Water)
                {
                    elapsedSeconds_WaterKnockBack = 0;
                    isWeakening_Water = true;
                }
            }
            else
            {
                if (elapsedSeconds_WaterKnockBack >= secondsForKnockBackWeakening_Water)
                {
                    rb.gravityScale = 0;
                    isWeakening_Water = false;
                    isKnockBack_Water = false;
                }
            }
            elapsedSeconds_WaterKnockBack += Time.deltaTime;
        }

        if (isInvincible_Damage)
        {
            elapsedSeconds_Damage += Time.deltaTime;
        }

        if (isElapsed_Jump)
        {
            elapsedSeconds_canJump += Time.deltaTime;
            if(elapsedSeconds_canJump >= airJumpBonusSeconds)
            {
                canJump = false;
                isElapsed_Jump = false;
                elapsedSeconds_canJump = 0;
            }
        }

        if (isWallJump)
        {
            elapsedSeconds_wallJump += Time.deltaTime;
            if(elapsedSeconds_wallJump >= secondsForWallJump)
            {
                isWallJump = false;
            }
        }

        if(wallJumpCountText.text != "WallJump:" + wallJumpCount.ToString())
        {
            wallJumpCountText.text = "WallJump:" + wallJumpCount.ToString();
        }

        if(airDashCountText.text != "AirDash:" + airDashCount.ToString())
        {
            airDashCountText.text = "AirDash:" + airDashCount.ToString();
        }
    }

    private void FixedUpdate()
    {
        if (currentEnvironment == Environment.Ground)
        {
            LimitFallingSpeed();
            if (isKnockBack_Ground)
            {
                KnockBack_Ground();
            }
        }
        else if(currentEnvironment == Environment.Water)
        {
            if (isKnockBack_Water)
            {
                KnockBack_Water();
            }
        }

        if (isDash)
        {
            Dash();
        }

        if (isFreeze)
        {
            Freeze();
        }
        else
        {
            ReceiveMoveInput();
            if(!isDash)
            {
                if (currentEnvironment == Environment.Ground)
                {
                    if (!canJump)
                    {
                        Jump();
                    }
                }
                //ReceiveMoveInput();
                if (!isFixed)
                {
                    if (rb.gravityScale == 0)
                    {
                        rb.gravityScale = 1;
                    }

                    if (currentEnvironment == Environment.Water && inputSwimAxis == Vector2.zero)
                    {
                        FallInWater();
                    }
                    else
                    {
                        Move();
                        //LookAtMoveDirection();
                    }
                }
                else
                {
                    // �Œ蒆�̋���
                    if (currentEnvironment == Environment.Ground)
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                    else if (currentEnvironment == Environment.Water)
                    {
                        if (rb.gravityScale != 0)
                        {
                            rb.gravityScale = 0;
                        }
                        rb.velocity = Vector2.zero;
                    }
                }
            }
        }
    }

    /// <summary>
    /// �ړ����͂��󂯎��A���������͂𖳎�����
    /// </summary>
    private void ReceiveMoveInput()
    {
        if(currentEnvironment == Environment.Ground)
        {
            inputWalkAxis = walk.ReadValue<Vector2>();

            if (inputWalkAxis.x != 0 && Mathf.Abs(inputWalkAxis.x) <= ignoreInput_Walk)
            {
                inputWalkAxis.x = 0;
            }

            Squat();
        }
        else if(currentEnvironment == Environment.Water)
        {
            inputSwimAxis = swim.ReadValue<Vector2>();

            if (inputSwimAxis.x != 0 && Mathf.Abs(inputSwimAxis.x) <= ignoreInput_Swim)
            {
                inputSwimAxis.x = 0;
            }
            if (inputSwimAxis.y != 0 && Mathf.Abs(inputSwimAxis.y) <= ignoreInput_Swim)
            {
                inputSwimAxis.y = 0;
            }
        }
        LookAtMoveDirection();
    }

    /// <summary>
    /// �ړ�����������
    /// </summary>
    private void LookAtMoveDirection()
    {
        if (currentEnvironment == Environment.Ground)
        {
            if (inputWalkAxis.x < 0)
            {
                myTransform.localScale = new Vector2(-1, myTransform.localScale.y);
            }
            else if (inputWalkAxis.x > 0)
            {
                myTransform.localScale = new Vector2(1, myTransform.localScale.y);
            }
        }
        else
        {
            if(inputSwimAxis == Vector2.zero)
            {
                if(myTransform.eulerAngles != Vector3.zero)
                {
                    myTransform.eulerAngles = Vector3.zero;
                }
            }
            else
            {
                if (myTransform.localScale.x != Mathf.Sign(inputSwimAxis.x))
                {
                    myTransform.localScale = new Vector3(Mathf.Sign(inputSwimAxis.x), 1f, 1f);
                }

                Quaternion target = Quaternion.FromToRotation(Vector3.right, inputSwimAxis);
                if (5f < Mathf.Abs(Quaternion.Angle(target, myTransform.rotation)))
                {

                    if (inputSwimAxis.x < 0 && target.eulerAngles.z != 0)
                    {
                        target = Quaternion.Euler(0f, 0f, target.eulerAngles.z - 180);
                    }

                    myTransform.rotation = target;

                    if (myTransform.eulerAngles.y == 180)
                    {
                        myTransform.rotation = Quaternion.Euler(Vector3.zero);
                    }
                }
            }
        }
    }

    private void Move()
    {
        if (currentEnvironment == Environment.Ground)
        {
            rb.velocity = new Vector2(inputWalkAxis.x * walkSpeed, rb.velocity.y);
        }
        else if (currentEnvironment == Environment.Water)
        {
            rb.velocity = inputSwimAxis * swimSpeed;
            if(fallSpeed_Water != 0)
            {
                fallSpeed_Water = 0;
            }
        }
    }

    private void FallInWater()
    {
        if (fallSpeed_Water < fallingMaxSpeed_Water)
        {
            fallSpeed_Water += fallAcceleration;
        }
        rb.velocity = new Vector2(0, -fallSpeed_Water);
    }

    private void Squat()
    {
        if (onGround && Mathf.Abs(inputWalkAxis.x) < ActiveSquat_X && inputWalkAxis.y < -ActiveSquat_Y)
        {
            if (!isSquat)
            {
                Squat_ChangeCollider(true);
            }
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else if (isSquat)
        {
            Squat_ChangeCollider(false);
        }
    }

    private void Squat_ChangeCollider(bool squat)
    {
        if (squat)
        {
            isSquat = true;
            isFixed = true;
            myCollider.size = colliderSize_Squat;
            myCollider.offset = colliderOffset_Squat;
        }
        else
        {
            isSquat = false;
            if (!isFixButtonPushing)
            {
                isFixed = false;
            }
            myCollider.size = colliderSize_default;
            myCollider.offset = colliderOffset_default;
        }
    }

    private void Jump()
    {
        if (isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, appliedJumpSpeed);
            if(myTransform.position.y >= limitYPos)
            {
                isJump = false;
            }
            // ����
            if ((limitYPos - myTransform.position.y) / jumpMaxHeight <= 1 - easingStartRate && rb.velocity.y > 0)
            {
                appliedJumpSpeed -= easingForce;
                if (appliedJumpSpeed <= 0)
                {
                    isJump = false;
                }
            }
        }
        else if (isWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, appliedWallJumpSpeed);
            if (myTransform.position.y >= limitYPos)
            {
                isWallJump = false;
            }
            // ����
            if ((limitYPos - myTransform.position.y) / jumpMaxHeight <= 1 - easingStartRate && rb.velocity.y > 0)
            {
                appliedWallJumpSpeed -= easingForce;
                if (appliedWallJumpSpeed <= 0)
                {
                    isWallJump = false;
                }
            }
        }
        else if(rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    public void onJump(InputAction.CallbackContext context)
    {
        if (isFreeze) return;

        if (context.performed)
        {
            if (onGround)
            {
                appliedJumpSpeed = jumpSpeed;
                limitYPos = myTransform.position.y + jumpMaxHeight;
                isJump = true;
            }
            else if (atWall)
            {
                if (wallJumpCount <= 0) return;
                wallJumpCount--;
                elapsedSeconds_wallJump = 0;
                appliedWallJumpSpeed = wallJumpSpeed;
                limitYPos = myTransform.position.y + jumpMaxHeight;
                if (airDashCount != airDashMaxNum)
                {
                    airDashCount = airDashMaxNum;
                }
                isWallJump = true;
            }
            else if (canJump)
            {
                appliedJumpSpeed = jumpSpeed;
                limitYPos = myTransform.position.y + jumpMaxHeight;
                isJump = true;
            }

            isJumpButtonPushing = true;
            canJump = false;
        }

        if (context.canceled)
        {
            isJump = false;
            isJumpButtonPushing = false;
        }
    }

    private void LimitFallingSpeed()
    {
        if(rb.velocity.y < 0 && rb.velocity.magnitude > fallingMaxSpeed)
        {
            rb.velocity = rb.velocity.normalized * fallingMaxSpeed;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (isFreeze || isDash || !canDash || Time.timeScale == 0f) return;

        if (context.performed)
        {
            if(currentEnvironment == Environment.Ground)
            {
                if (isJump)
                {
                    isJump = false;
                }
                if (isWallJump)
                {
                    isWallJump = false;
                }

                if (!onGround)
                {
                    if (airDashCount == 0)
                    {
                        return;
                    }
                    else
                    {
                        airDashCount--;
                    }
                }

                if (isSquat)
                {
                    secondsForDash = secondsForSliding;
                    dashDistance = slidingDistance;
                    secondsForDashInvincible = secondsForSlidingInvincible;
                }
                else
                {
                    secondsForDash = secondsForDash_Ground;
                    dashDistance = dashDistance_Ground;
                    secondsForDashInvincible = secondsForDashInvincible_Ground;
                }

                dashVector.x = dashDistance / secondsForDash * myTransform.localScale.x;
                dashVector.y = 0f;
                secondsForDashCT = secondsForDashCT_Ground;
            }
            else if(currentEnvironment == Environment.Water)
            {
                if (inputSwimAxis == Vector2.zero)
                {
                    dashVector.x = myTransform.localScale.x * dashDistance_Water / secondsForDash_Water;
                    dashVector.y = 0f;
                }
                else
                {
                    dashVector = inputSwimAxis.normalized * dashDistance_Water / secondsForDash_Water;
                }
                secondsForDash = secondsForDash_Water;
                dashDistance = dashDistance_Water;
                secondsForDashInvincible = secondsForDashInvincible_Water;
                secondsForDashCT = secondsForDashCT_Water;
            }

            if (atWall)
            {
                RaycastHit2D hit = Physics2D.BoxCast(myCollider.bounds.center, myCollider.size, myTransform.eulerAngles.z, dashVector, dashDistance, environmentLayer);
                if(hit.collider != null)
                {
                    isHit_Dash = true;
                }
            }

            currentPos_Dash = myTransform.position;
            targetPos_Dash = myTransform.position + (Vector3)dashVector.normalized * dashDistance;
            elapsedSeconds_Dash = 0;
            isFreeze = true;
            rb.gravityScale = 0f;
            isDash = true;
            canDash = false;
            isInvincible_Dash = true;
            isElapsed_Dash = true;
        }
    }

    private void Dash()
    {
        rb.velocity = dashVector;

        if(dashDistance * dashDistance <= (myTransform.position - currentPos_Dash).sqrMagnitude)
        {
            FinishDash();
        }

        if (isHit_Dash && secondsForDash < elapsedSeconds_Dash)
        {
            FinishDash();
            isHit_Dash = false;
        }

        if (!isElapsed_Dash) // �Փ˂�������isHit_Dash��true�ɂȂ�Ȃ��ꍇ����A����s�\�΍�
        {
            FinishDash();
        }
    }

    private void FinishDash()
    {
        isDash = false;
        isFreeze = false;
        rb.gravityScale = 1f;
        rb.velocity = new Vector2(rb.velocity.x, -fallPowerAfterDash);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Surface")))
        {
            if (isJump)
            {
                if (!isJumpButtonPushing)
                {
                    isJump = false;
                }
                else if (collision.GetContact(0).normal == Vector2.down) // �V��ƐڐG
                {
                    isJump = false;
                }
            }

            if (isWallJump)
            {
                if (!isJumpButtonPushing)
                {
                    isWallJump = false;
                }
                else if(collision.GetContact(0).normal == Vector2.down) // �V��ƐڐG
                {
                    isWallJump = false;
                }
            }
        }

        if (isKnockBack_Ground)
        {
            isKnockBack_Ground = false;
            isWeakening_Ground = false;
        }

        if (isDash &&  LayerMask.LayerToName(collision.gameObject.layer) == "Environment" &&  !isHit_Dash)
        {
            isHit_Dash = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("ActionButtonTarget") && actionButtonTarget == null)
        {
            actionButtonTarget = collision.GetComponent<ActionButtonTarget>();
            actionIcon.transform.position = actionButtonTarget.ObjectTopPos + iconPosOffset;
            if(actionIcon != null)
            {
                actionIcon.enabled = true;
            }
        }

        if(collision.CompareTag("Goal") && goal == null)
        {
            goal = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!isEntering && !isGoingUp)
        {
            if (LayerMask.LayerToName(collision.gameObject.layer) == "Environment")
            {
                if (onGround && rb.velocity.y != 0)
                {
                    onGround = false;
                    if (!isJump && !atWall)
                    {
                        isElapsed_Jump = true;
                    }
                }

                if (atWall)
                {
                    atWall = false;
                    if (!onGround && !isJump && !isWallJump)
                    {
                        canJump = false;
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("ActionButtonTarget") && actionButtonTarget != null)
        {
            actionButtonTarget = null;
            if(actionIcon != null)
            {
                actionIcon.enabled = false;
            }
        }
        if (collision.CompareTag("Goal") && goal != null)
        {
            goal = null;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Environment")
        {
            ContactPoint2D contact = collision.GetContact(0);
            if (collision.gameObject.CompareTag("OwBlock"))
            {
                if (!canJump && !isJump && !isWallJump)
                {
                    canJump = true;
                }

                if (contact.normal.x == 0)// ���ƐڐG
                {
                    if (atWall)
                    {
                        atWall = false;
                    }

                    if (!isJumpButtonPushing)
                    {
                        onGround = true;
                        wallJumpCount = wallJumpMaxNum;
                        airDashCount = airDashMaxNum;
                    }
                }
            }
            else
            {
                if (!canJump && !isJump && !isWallJump && contact.normal != Vector2.down)
                {
                    canJump = true;
                }

                if (contact.normal == Vector2.up)// ���ƐڐG
                {
                    if (!onGround)
                    {
                        onGround = true;
                    }

                    if (wallJumpCount != wallJumpMaxNum)
                    {
                        wallJumpCount = wallJumpMaxNum;
                    }
                    if (airDashCount != airDashMaxNum)
                    {
                        airDashCount = airDashMaxNum;
                    }
                }
                else if (contact.normal.x != 0)// �ǂƐڐG
                {
                    if (!isJump && !atWall)
                    {
                        atWall = true;
                    }

                    if (isJump && !isJumpButtonPushing)
                    {
                        isJump = false;
                    }
                }
            }
        }
    }

    public void GetDamage(Vector3 position, int damage)
    {
        if (isPlayerInvincible) return;

        currentHP = Mathf.Clamp(currentHP - damage, 0, currentHP);
        if(currentHP == 0)
        {
            GameManager.Instance.GameOver();
        }
        hpText.text = "HP:" + currentHP;
        Squat_ChangeCollider(false);
        StartCoroutine(DamageInvincibleCoroutine());
        StartCoroutine(DamageFreezeCoroutine(position - (myTransform.position + (Vector3)myCollider.offset))); // ��l�����G
    }

    private IEnumerator DamageInvincibleCoroutine()
    {
        isInvincible_Damage = true;
        yield return new WaitUntil(() => elapsedSeconds_Damage >= secondsForDamageInvincible);
        isInvincible_Damage = false;
        elapsedSeconds_Damage = 0;
    }

    private IEnumerator DamageFreezeCoroutine(Vector2 enemyRelativePos)
    {
        isFreeze = true;
        if(currentEnvironment == Environment.Ground)
        {
            enemyRelativeXPos = enemyRelativePos.x;
            elapsedSeconds_GroundKnockBack = 0;
            isFinishedFirstAction = false;
            isWeakening_Ground = false;
            isKnockBack_Ground = true;
        }
        else if (currentEnvironment == Environment.Water)
        {
            if(enemyRelativePos == Vector2.zero)
            {
                waterKnockBackAngle = new Vector2(-myTransform.localScale.x, 0);
            }
            else
            {
                waterKnockBackAngle = -enemyRelativePos;
            }
            elapsedSeconds_WaterKnockBack = 0;
            waterKnockBackSpeed = waterKnockBackSpeed_First;
            rb.gravityScale = 1;
            isWeakening_Water = false;
            isKnockBack_Water = true;
        }
        yield return new WaitUntil(() => elapsedSeconds_Damage >= secondsForDamageFreeze);
        if (isJumpButtonPushing)
        {
            isJumpButtonPushing = false;
        }
        isFreeze = false;
    }

    private void KnockBack_Ground()
    {
        if (!isFinishedFirstAction)
        {
            //Debug.Log("�ŏ�");
            rb.velocity = Vector2.zero;

            if (enemyRelativeXPos > 0) // ������
            {
                rb.AddForce(Quaternion.Euler(0, 0, knockBackAngle) * Vector2.up * knockBackPower, ForceMode2D.Impulse);
            }
            else if (enemyRelativeXPos < 0) // �E����
            {
                rb.AddForce(Quaternion.Euler(0, 0, -knockBackAngle) * Vector2.up * knockBackPower, ForceMode2D.Impulse);
            }
            else // �^��/�^������
            {
                rb.AddForce(Quaternion.Euler(0, 0, knockBackAngle * myTransform.localScale.x) * Vector2.up * knockBackPower, ForceMode2D.Impulse);
            }
            isFinishedFirstAction = true;
        }
        else
        {
            if (!isWeakening_Ground)
            {
                //Debug.Log("�҂�");
            }
            else
            {
                //Debug.Log("������");
                knockBackSpeed_Ground = knockBackSpeed_Ground * (1f - knockBackWeakeningPower_Ground);
                rb.velocity = new Vector2(knockBackSpeed_Ground, rb.velocity.y);
                if (Mathf.Abs(rb.velocity.x) < 0.1f || Mathf.Abs(rb.velocity.y) < 0.1f)
                {
                    isKnockBack_Ground = false;
                    isWeakening_Ground = false;
                }
            }
        }
    }

    private void KnockBack_Water()
    {
        waterKnockBackSpeed -= knockBackWeakeningPower_Water;
        rb.velocity = waterKnockBackAngle.normalized * waterKnockBackSpeed;
    }

    private void Freeze()
    {
        if (currentEnvironment == Environment.Ground)
        {
            if (isJump)
            {
                isJump = false;
            }
            if (isWallJump)
            {
                isWallJump = false;
            }
            if (isSquat && !isDash)
            {
                Squat_ChangeCollider(false);
            }
        }
    }

    public void onAttack(InputAction.CallbackContext context)
    {
        if (isFreeze || !canAttack) return;

        if (context.performed)
        {
            if (currentEnvironment == Environment.Ground)
            {
                attackDirection = walk.ReadValue<Vector2>();
                secondsForAttack_before = secondsForGroundAttack_before;
                secondsForAttacking = secondsForGroundAttacking;
                secondsForAttack = secondsForGroundAttack;
                secondsForAttackCT = secondsForGroundAttackCT;

            }
            else
            {
                attackDirection = swim.ReadValue<Vector2>();
                secondsForAttack_before = secondsForWaterAttack_before;
                secondsForAttacking = secondsForWaterAttacking;
                secondsForAttack = secondsForWaterAttack;
                secondsForAttackCT = secondsForWaterAttackCT;
            }

            elapsedSeconds_Attack = 0;
            isElapsed_AttackBefore = false;
            isElapsed_Attacking = false;

            isAttack = true;
            canAttack = false;
        }
    }

    private void Attack()
    {
        if (!isElapsed_AttackBefore)
        {
            if (elapsedSeconds_Attack >= secondsForAttack_before)
            {
                if(currentEnvironment == Environment.Ground)
                {
                    if (!onGround && attackDirection.y < -0.5f)
                    {
                        playerAttack.DownAttack();
                    }
                    else if (isSquat)
                    {
                        playerAttack.SquatAttack();
                    }
                    else if (attackDirection.y > 0.5f)
                    {
                        playerAttack.UpAttack();
                    }
                    else
                    {
                        playerAttack.FrontAttack();
                    }
                }
                else
                {
                    playerAttack.WaterAttack();
                }

                isElapsed_AttackBefore = true;
            }
            /*else
            {
                Debug.Log("�O�X�L");
            }*/
        }
        else
        {
            if (!isElapsed_Attacking)
            {
                if (elapsedSeconds_Attack >= secondsForAttack_before + secondsForAttacking)
                {
                    playerAttack.FinishAttacking();
                    isElapsed_Attacking = true;
                }
                /*else
                {
                    Debug.Log("�U����");
                }*/
            }
            else
            {
                if (isAttack)
                {
                    if (elapsedSeconds_Attack >= secondsForAttack)
                    {
                        isAttack = false;
                    }
                    /*else
                    {
                        Debug.Log("���[�V���������ҋ@");
                    }*/
                }
            }
        }

        if (elapsedSeconds_Attack >= secondsForAttackCT)
        {
            canAttack = true;
        }

        elapsedSeconds_Attack += Time.deltaTime;
    }

    public void onFix(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isFixButtonPushing = true;
            if (!isFixed)
            {
                isFixed = true;
            }
        }

        if (context.canceled)
        {
            isFixButtonPushing = false;
            isFixed = false;
        }
    }

    public void onEnterTheWater(InputAction.CallbackContext context)
    {
        if (isFreeze || !CanEnterTheWater || isDash) return;

        if (context.performed)
        {
            if (isSquat)
            {
                Squat_ChangeCollider(false);
            }
            isFreeze = true;
            rb.velocity = Vector2.zero;
            usedSeconds_before = secondsForEnter_before;
            usedSeconds_move = secondsForEnter;
            usedSeconds_after = secondsForEnter_after;
            StartCoroutine(ChangeEnvironmentCoroutine());
            usedSeconds_beforeInvincible = secondsForBeforeEnterInvincible;
            usedSeconds_Invincible = secondsForEnterInvincible;
            elapsedSeconds_InvincibleByChangeEnironment = 0f;
            isSetInvincible = true;
            CanEnterTheWater = false;
        }
    }

    public void onGoUpToGround(InputAction.CallbackContext context)
    {
        if (isFreeze || !CanGoUpToGround || isDash) return;

        if (context.performed)
        {
            isFreeze = true;
            rb.velocity = Vector2.zero;
            usedSeconds_before = secondsForGoUp_before;
            usedSeconds_move = secondsForGoUp;
            usedSeconds_after = secondsForGoUp_after;
            StartCoroutine(ChangeEnvironmentCoroutine());
            usedSeconds_beforeInvincible = secondsForBeforeGoUpInvincible;
            usedSeconds_Invincible = secondsForGoUpInvincible;
            elapsedSeconds_InvincibleByChangeEnironment = 0f;
            isSetInvincible = true;
            CanGoUpToGround = false;
        }
    }

    private void SetInvincibleChangeEnvironment()
    {
        elapsedSeconds_InvincibleByChangeEnironment += Time.deltaTime;
        if (elapsedSeconds_InvincibleByChangeEnironment >= usedSeconds_beforeInvincible)
        {
            if (currentEnvironment == Environment.Ground && !isInvincible_Enter)
            {
                isInvincible_Enter = true;
            }
            if (currentEnvironment == Environment.Water && !isInvincible_GoUp)
            {
                isInvincible_GoUp = true;
            }
        }
        if(elapsedSeconds_InvincibleByChangeEnironment >= usedSeconds_beforeInvincible + usedSeconds_Invincible)
        {
            if (isInvincible_Enter)
            {
                isInvincible_Enter = false;
            }
            if (isInvincible_GoUp)
            {
                isInvincible_GoUp = false;
            }
            isSetInvincible = false;
        }
    }

    private IEnumerator ChangeEnvironmentCoroutine()
    {
        elapsedSeconds_ChangeEnvironment = 0;
        currentPos_ChangeEnvironment = myTransform.position;
        myRenderer.enabled = false;
        if (currentEnvironment == Environment.Ground)
        {
            isEntering = true;
        }
        else
        {
            isGoingUp = true;
        }
        ChangeInput();
        yield return new WaitUntil(() => elapsedSeconds_ChangeEnvironment >= usedSeconds_before);
        elapsedSeconds_ChangeEnvironment = 0;
        targetPos_ChangeEnvironment = playerData.TargetPos_ChangeEnvironment;
        isChanging = true;
        yield return new WaitUntil(() => elapsedSeconds_ChangeEnvironment >= usedSeconds_move);
        isChanging = false;
        CameraManager.Instance.ChangeCameraVersion();
        elapsedSeconds_ChangeEnvironment = 0;
        yield return new WaitUntil(() => elapsedSeconds_ChangeEnvironment >= usedSeconds_after);
        if (currentEnvironment == Environment.Ground)
        {
            isEntering = false;
        }
        else
        {
            isGoingUp = false;
        }
        isFreeze = false;
        myRenderer.enabled = true;
        if (currentEnvironment == Environment.Ground)
        {
            currentEnvironment = Environment.Water;
            //playerInput.SwitchCurrentActionMap("Player_Water");
        }
        else
        {
            currentEnvironment = Environment.Ground;
            myTransform.rotation = Quaternion.identity;
            //playerInput.SwitchCurrentActionMap("Player_Ground");
            limitYPos = myTransform.position.y + jumpMaxHeight;
        }
    }

    private void ChangeEnvironment()
    {
        elapsedSeconds_ChangeEnvironment += Time.deltaTime;
        if (isChanging)
        {
            rate = elapsedSeconds_ChangeEnvironment / usedSeconds_move;
            myTransform.position = Vector2.Lerp(currentPos_ChangeEnvironment, targetPos_ChangeEnvironment, rate);
        }
    }

    /// <summary>
    /// �A�N�V�����}�b�v��K�X�؂�ւ���
    /// </summary>
    public void ChangeInput()
    {
        if (playerInput.currentActionMap.name == "UI")
        {
            if(currentEnvironment == Environment.Ground)
            {
                playerInput.SwitchCurrentActionMap("Player_Ground");
            }
            else
            {
                playerInput.SwitchCurrentActionMap("Player_Water");
            }
        }
        else
        {
            if (currentEnvironment == Environment.Ground)
            {
                if (isEntering)
                {
                    playerInput.SwitchCurrentActionMap("Player_Water");
                }
                else
                {
                    playerInput.SwitchCurrentActionMap("UI");
                }
            }
            else
            {
                if (isGoingUp)
                {
                    playerInput.SwitchCurrentActionMap("Player_Ground");
                }
                else
                {
                    playerInput.SwitchCurrentActionMap("UI");
                }
            }
        }
        //Debug.Log(playerInput.currentActionMap.name);
    }

    public void onTransfer(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            myTransform.position = transferTargetPos;

            if(transferTargetPosEnvironment != currentEnvironment)
            {
                //ChangeInput()�ł�UI�ɐ؂�ւ����Ă��܂�����
                if (currentEnvironment == Environment.Ground)
                {
                    currentEnvironment = Environment.Water;
                    playerInput.SwitchCurrentActionMap("Player_Water");
                }
                else if (currentEnvironment == Environment.Water)
                {
                    currentEnvironment = Environment.Ground;
                    playerInput.SwitchCurrentActionMap("Player_Ground");
                }

                CameraManager.Instance.ChangeCameraVersion();
            }
        }
    }

    public void onAction(InputAction.CallbackContext context)
    {
        if ((goal == null && actionButtonTarget == null) || isEntering || isGoingUp || isFreeze || (currentEnvironment == Environment.Ground && !onGround) || (currentEnvironment == Environment.Water && CanGoUpToGround)) return;
        if (context.performed)
        {
            if(goal != null)
            {
                goal.GetComponent<Goal>().FlagUpdate();
                GameManager.Instance.GameClear();
            }
            else
            {
                actionButtonTarget.Execute();
                if (actionButtonTarget.ObjectType == ActionButtonTarget.ObjectTypeData.ShowMessage)
                {
                    // �Ώۂ̕�������
                    if (myTransform.position.x <= actionButtonTarget.transform.position.x && myTransform.localScale.x != 1)
                    {
                        myTransform.localScale = Vector3.one;
                    }
                    if (actionButtonTarget.transform.position.x < myTransform.position.x && myTransform.localScale.x != -1)
                    {
                        myTransform.localScale = new Vector3(-1, 1, 1);
                    }
                }
            }
        }
    }
}
