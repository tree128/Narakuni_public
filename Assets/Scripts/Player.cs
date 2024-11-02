using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("<color=red>=====‘S‘Ì===============</color>")]
    //Gravity
    [Header("y—‰º‚ÌX‰Á‘¬“x‚ÍZz"), SerializeField, Range(-120f, 0f)] private float gravity;
    [Header("y—‰º‚ÌXÅ‚‘¬“x‚ÍZz"), SerializeField, Range(0f, 120f)] private float fallingMaxSpeed;

    // HP
    [Header("yƒvƒŒƒCƒ„[‚ÌXÅ‘å‘Ì—Í‚ÍZz"), SerializeField, Range(1, 300)]private int maxHP;
    private int currentHP;
    [SerializeField] private TMP_Text hpText;

    // Damage
    [Header("yƒ_ƒ[ƒW‚ÌXd’¼ŠÔ‚ÍZ•bz"), SerializeField, Range(0f, 5f)] private float secondsForDamageFreeze;
    [Header("yƒ_ƒ[ƒW‚ÌX–³“GŠÔ‚ÍZ•bz"), SerializeField, Range(0f, 5f)] private float secondsForDamageInvincible;
    private float elapsedSeconds_Damage;
    /// <summary>
    /// ƒ_ƒ[ƒWŒãA–³“Gó‘Ô‚ÌŠÔtrue‚ğ•Ô‚·
    /// </summary>
    private bool isInvincible_Damage = false;
    /// <summary>
    /// ƒvƒŒƒCƒ„[‚ª‰½‚ç‚©‚ÌŒ`‚Å–³“Gó‘Ô‚ÌŠÔtrue‚ğ•Ô‚·
    /// </summary>
    private bool isPlayerInvincible = false;
    public bool IsPlayerInvincible => isPlayerInvincible;
    /// <summary>
    /// ƒ_ƒ[ƒWŒãAƒ_ƒbƒVƒ…’†Aö…/•‚ã’†Ad’¼ó‘Ô‚ÌŠÔtrue‚ğ•Ô‚·B
    /// true‚ÌŠÔ‚Í“ü—Í‚ğó‚¯•t‚¯‚È‚¢
    /// </summary>
    private bool isFreeze;
    [Header("yƒQ[ƒ€ƒI[ƒo[‚Ìˆê’â~‚ÍZ•bz"), SerializeField, Range(0.01f, 5f)]private float pauseTime;

    // Dash
    private float elapsedSeconds_Dash;
    /// <summary>
    /// elapedSeconds_Dash‚ğ‰ÁZ‚·‚éŠÔtrue‚ğ•Ô‚·
    /// </summary>
    private bool isElapsed_Dash = false;
    private Vector3 currentPos_Dash;
    private Vector3 targetPos_Dash;
    private Vector2 dashVector = Vector2.zero;
    /// <summary>
    /// ƒ_ƒbƒVƒ…’†‚Étrue‚ğ•Ô‚·
    /// </summary>
    private bool isDash = false;
    /// <summary>
    /// ƒ_ƒbƒVƒ…ŠJn‰Â”\‚ÈŠÔtrue‚ğ•Ô‚·
    /// </summary>
    private bool canDash = true;
    /// <summary>
    /// ƒ_ƒbƒVƒ…‚Å–³“G‚ª”­¶‚µ‚Ä‚¢‚éŠÔtrue‚ğ•Ô‚·
    /// </summary>
    private bool isInvincible_Dash = false;
    /// <summary>
    /// ƒ_ƒbƒVƒ…‚ÅÕ“Ë’†‚Étrue‚ğ•Ô‚·
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
    /// ŒÅ’è’†‚Étrue‚ğ•Ô‚·
    /// </summary>
    private bool isFixed;
    /// <summary>
    /// ŒÅ’èƒ{ƒ^ƒ“‚ğ‰Ÿ‚µ‚Ä‚¢‚éŠÔtrue‚ğ•Ô‚·
    /// </summary>
    private bool isFixButtonPushing = false;

    // Transfer
    [SerializeField] private Vector2 transferTargetPos;
    [SerializeField] private Environment transferTargetPosEnvironment;



    [Header("<color=red>=====—¤ã===============</color>")]
    // Walk
    [Header("yˆÚ“®‚ÌX‘¬“x‚ÍZz"), SerializeField, Range(0.1f, 30f)] private float walkSpeed;
    private InputAction walk;
    private Vector2 inputWalkAxis;
    [Header("yˆÚ“®‚Ì“ü—Í–³‹‚ÌX”’l‚ÍZz"), SerializeField, Range(0f, 0.5f)] private float ignoreInput_Walk;

    // Jump
    [Header("yƒWƒƒƒ“ƒv‚ÌX‰‘¬“x‚ÍZz"), SerializeField, Range(0.5f, 120f)] private float jumpSpeed;
    private float appliedJumpSpeed;
    [Header("ƒWƒƒƒ“ƒv‚ÌXÅ‚‚“x‚ÍZ"), SerializeField, Range(0.1f, 30f)] private float jumpMaxHeight;
    private float limitYPos;
    /// <summary>
    /// Ground, Surface, OwBlockƒ^ƒO‚ÌƒIƒuƒWƒFƒNƒg‚Éæ‚Á‚Ä‚¢‚éŠÔtrue‚ğ•Ô‚·
    /// </summary>
    [SerializeField]private bool onGround = true;
    /// <summary>
    /// ƒWƒƒƒ“ƒv‰Â”\‚ÈŠÔtrue‚ğ•Ô‚·
    /// </summary>
    private bool canJump = true;
    /// <summary>
    /// ƒWƒƒƒ“ƒv‚Åã¸’†‚Étrue‚ğ•Ô‚·
    /// </summary>
    private bool isJump = false;
    [Header("y•ÇƒWƒƒƒ“ƒv‚ÌX‰ñ”‚ÍZz"), SerializeField, Range(0, 12)] private int wallJumpMaxNum;
    private int wallJumpCount;
    [SerializeField]private TMP_Text wallJumpCountText;
    [Header("•ÇƒWƒƒƒ“ƒv‚ÌX‰‘¬“x‚ÍZ"), SerializeField, Range(0.5f, 120f)] private float wallJumpSpeed;
    private float appliedWallJumpSpeed;
    [Header("y•ÇƒWƒƒƒ“ƒv‚ÌX‘±‚ÍZ•bz"), SerializeField, Range(0f, 5f)] private float secondsForWallJump;
    private float elapsedSeconds_wallJump;
    /// <summary>
    /// Ground, Surfaceƒ^ƒO‚ÌƒIƒuƒWƒFƒNƒg‚Ì‚’¼–Ê‚ÆÚ‚µ‚Ä‚¢‚éŠÔ‚Étrue‚ğ•Ô‚·
    /// </summary>
    [SerializeField]private bool atWall;
    /// <summary>
    /// •ÇƒWƒƒƒ“ƒv‚ğ‚µ‚Ä‚¢‚éŠÔtrue‚ğ•Ô‚·
    /// </summary>
    private bool isWallJump;
    /// <summary>
    /// ƒWƒƒƒ“ƒvƒ{ƒ^ƒ“‚ğ‰Ÿ‚µ‚Ä‚¢‚éŠÔtrue‚ğ•Ô‚·
    /// </summary>
    private bool isJumpButtonPushing = false;
    [Header("y‹ó’†ƒWƒƒƒ“ƒv‚ÌX—P—\‚ÍZ•bz"), SerializeField, Range(0f, 3f)] private float airJumpBonusSeconds;
    private float elapsedSeconds_canJump;
    /// <summary>
    /// elapsedSeconds_canJump‚ğ‰ÁZ‚·‚éŠÔtrue‚ğ•Ô‚·
    /// </summary>
    private bool isElapsed_Jump = false;

    // Easing
    [Header("Œ¸‘¬‚ğn‚ß‚éXƒWƒƒƒ“ƒv‚Ìis‹ï‡‚ÍZ"), SerializeField, Range(0.1f, 1f)] private float easingStartRate;
    [Header("ƒWƒƒƒ“ƒv‚ÌXŒ¸‘¬‚Ì‹­‚³‚ÍZ"), SerializeField, Range(0.1f, 10f)] private float easingForce;

    // Dash
    [Header("yƒ_ƒbƒVƒ…‚ÌX‘S‘Ì‚ÍZ•bz"), SerializeField, Range(0f, 5f)] private float secondsForDash_Ground;
    [Header("yƒ_ƒbƒVƒ…‚ÌX‹——£‚ÍZz"), SerializeField, Range(0.1f, 100f)] private float dashDistance_Ground;
    [Header("yƒ_ƒbƒVƒ…/ƒXƒ‰ƒCƒfƒBƒ“ƒO‚ÌXƒN[ƒ‹ƒ^ƒCƒ€‚ÍZ•bz"), SerializeField, Range(0f, 5f)] private float secondsForDashCT_Ground;
    [Header("yƒ_ƒbƒVƒ…–³“G‚ÌX‘±‚ÍZ•bz"), SerializeField, Range(0f, 5f)] private float secondsForDashInvincible_Ground;
    [Header("yƒ_ƒbƒVƒ…‚ÌXŒã—‰º‚ÍZz"), SerializeField, Range(0.1f, 10f)] private float fallPowerAfterDash;
    [Header("yƒ_ƒbƒVƒ…‚ÌX‹ó’†‰ñ”‚ÍZz"), SerializeField, Range(1, 5)] private int airDashMaxNum;
    private int airDashCount;
    [SerializeField] private TMP_Text airDashCountText;

    // Squat
    [Header("y‚µ‚á‚ª‚İ‚ÌX“ü—Í[“x‚ÍZz"), SerializeField, Range(0.1f, 1f)] private float ActiveSquat_Y;
    [Header("y‚µ‚á‚ª‚İ‚ÌX“ü—ÍŠp“x‚ÍZz"), SerializeField, Range(0.1f, 1f)] private float ActiveSquat_X;
    [Header("y‚µ‚á‚ª‚İ”»’è‚ÌX‚‚³‚ÍZz"), SerializeField] private Vector2 colliderSize_Squat;
    [Header("‚µ‚á‚ª‚İ”»’è‚ÌX’†SˆÊ’u‚ÍZ"), SerializeField] private Vector2 colliderOffset_Squat;
    /// <summary>
    /// ‚µ‚á‚ª‚İ’†‚Étrue‚ğ•Ô‚·
    /// </summary>
    private bool isSquat;

    // Sliding
    [Header("yƒXƒ‰ƒCƒfƒBƒ“ƒO‚ÌX‘S‘Ì‚ÍZ•bz"), SerializeField, Range(0f, 5f)] private float secondsForSliding;
    [Header("yƒXƒ‰ƒCƒfƒBƒ“ƒO‚ÌX‹——£‚ÍZz"), SerializeField, Range(0.1f, 100f)] private float slidingDistance;
    [Header("yƒXƒ‰ƒCƒfƒBƒ“ƒO–³“G‚ÌX‘±‚ÍZ•bz"), SerializeField, Range(0f, 5f)] private float secondsForSlidingInvincible;

    // Attack
    [Header("yUŒ‚‚ÌX‘OƒXƒL‚ÍZ•bz"), SerializeField, Range(0f, 1f)] private float secondsForGroundAttack_before;
    [Header("yUŒ‚‚ÌX‘±‚ÍZ•bz"), SerializeField, Range(0f, 1f)] private float secondsForGroundAttacking;
    [Header("yUŒ‚‚ÌX‘S‘Ì‚ÍZ•bz"), SerializeField, Range(0f, 3f)] private float secondsForGroundAttack;
    [Header("yUŒ‚‚ÌXƒN[ƒ‹ƒ^ƒCƒ€‚ÍZ•bz"), SerializeField, Range(0f, 5f)] private float secondsForGroundAttackCT;
    /// <summary>
    /// UŒ‚“ü—Í‚©‚çyUŒ‚‚ÌXƒN[ƒ‹ƒ^ƒCƒ€‚ÍZ•bzŒo‰ß‚·‚éŠÔ‚Éfalse‚ğ•Ô‚·
    /// </summary>
    private bool canAttack = true;
    /// <summary>
    /// UŒ‚’†(UŒ‚“ü—Í‚©‚çyUŒ‚‚ÌX‘S‘Ì‚ÍZ•bzŒo‰ß‚·‚éŠÔ)‚Étrue‚ğ•Ô‚·
    /// </summary>
    private bool isAttack = false;
    /// <summary>
    /// UŒ‚’†‚©‚ÂUŒ‚“ü—Í‚©‚çyUŒ‚‚ÌX‘OƒXƒL‚ÍZ•bzŒo‰ß‚µ‚½‚çtrue‚ğ•Ô‚·
    /// </summary>
    private bool isElapsed_AttackBefore = false;
    /// <summary>
    /// UŒ‚’†‚©‚ÂUŒ‚“ü—Í‚©‚çyUŒ‚‚ÌX‘OƒXƒL‚ÍZ•bz+yUŒ‚‚ÌX‘±‚ÍZ•bzŒo‰ß‚µ‚½‚çtrue‚ğ•Ô‚·
    /// </summary>
    private bool isElapsed_Attacking = false;
    private Vector2 attackDirection;

    // KnockBack
    [Header("yƒmƒbƒNƒoƒbƒN‚ÌX‰‘¬“x‚ÍZz"), SerializeField, Range(0f, 1000f)] private float knockBackPower;
    [Header("yƒmƒbƒNƒoƒbƒN‚ÌXŠp“x‚ÍZz"), SerializeField, Range(0f, 90f)] private float knockBackAngle;
    //[Header("ƒmƒbƒNƒoƒbƒN‚ÌXŒ¸‘¬ŠJn‚ÍZ•bŒã"), SerializeField, Range(0f, 1f)] private float knockBackWeakeningStartSecond_Ground;
    [Header("yƒmƒbƒNƒoƒbƒN‚ÌXŒ¸‘¬‚ğn‚ß‚éis‹ï‡‚ÍZz"), SerializeField, Range(0f, 1f)] private float knockBackWeakeningStartRate;
    [Header("yƒmƒbƒNƒoƒbƒN‚ÌXŒ¸‘¬‚Ì‹­‚³‚ÍZz"), SerializeField, Range(0f, 1f)] private float knockBackWeakeningPower_Ground;
    [Header("ƒmƒbƒNƒoƒbƒN‚ÌXŒ¸‘¬‘±‚ÍZ•b"), SerializeField, Range(0f, 3f)] private float secondsForKnockBackWeakening_Ground;
    /// <summary>
    /// ’nãƒmƒbƒNƒoƒbƒN’†‚Étrue‚ğ•Ô‚·
    /// </summary>
    private bool isKnockBack_Ground;
    /// <summary>
    /// ’nãƒmƒbƒNƒoƒbƒN‚ÅŒ¸‘¬’†‚Étrue‚ğ•Ô‚·
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
    /// ö…/•‚ã‚ÅˆÚ“®’†‚Étrue‚ğ•Ô‚·
    /// </summary>
    private bool isChanging = false;
    // EnterTheWater
    [ Header("<color=red>=====ö…/•‚ã===============</color>")]
    [Header("yö…‚ÌXˆÚ“®ŠÔ‘O‚ÍZ•bz"), SerializeField, Range(0f, 3f)] private float secondsForEnter_before;
    [Header("yö…‚ÌXˆÚ“®ŠÔ‚ÍZ•bz"), SerializeField, Range(0f, 5f)] private float secondsForEnter;
    [Header("yö…‚ÌXˆÚ“®ŠÔŒã‚ÍZ•bz"), SerializeField, Range(0f, 3f)] private float secondsForEnter_after;
    /// <summary>
    /// ö…‰Â”\‚ÈŠÔtrue‚ğ•Ô‚·
    /// </summary>
    [Tooltip("Surface/buffer‚Å’²®‰Â”\")] public bool CanEnterTheWater = false;
    /// <summary>
    /// ˆÚ“®‘OŒã‚Ìd’¼‚ğŠÜ‚Şö…’†‚Étrue‚ğ•Ô‚·
    /// </summary>
    private bool isEntering = false;
    [HideInInspector] public bool IsEntering => isEntering;
    [Header("yö…‚ÌX–³“GŠJn‚ÍZ•bŒãz"), SerializeField, Range(0.01f, 1f)] private float secondsForBeforeEnterInvincible;
    [Header("yö…‚ÌX–³“G‘±‚ÍZ•bz"), SerializeField, Range(0.01f, 5f)] private float secondsForEnterInvincible;
    /// <summary>
    /// ö…‚É‚æ‚é–³“G’†‚Étrue‚ğ•Ô‚·
    /// </summary>
    private bool isInvincible_Enter;

    // GoUpToGround
    [Header("y•‚ã‚ÌXˆÚ“®ŠÔ‘O‚ÍZ•bz"), SerializeField, Range(0f, 3f)] private float secondsForGoUp_before;
    [Header("y•‚ã‚ÌXˆÚ“®ŠÔ‚ÍZ•bz"), SerializeField, Range(0f, 5f)] private float secondsForGoUp;
    [Header("y•‚ã‚ÌXˆÚ“®ŠÔŒã‚ÍZ•bz"), SerializeField, Range(0f, 3f)] private float secondsForGoUp_after;
    /// <summary>
    /// •‚ã‰Â”\‚ÈŠÔtrue‚ğ•Ô‚·
    /// </summary>
    [Tooltip("Surface/buffer‚Å’²®‰Â”\")]public bool CanGoUpToGround = false;
    /// <summary>
    /// ˆÚ“®‘OŒã‚Ìd’¼‚ğŠÜ‚Ş•‚ã’†‚Étrue‚ğ•Ô‚·
    /// </summary>
    private bool isGoingUp = false;
    [HideInInspector] public bool IsGoingUp => isGoingUp;
    [Header("y•‚ã‚ÌX–³“GŠJn‚ÍZ•bŒãz"), SerializeField, Range(0.01f, 1f)] private float secondsForBeforeGoUpInvincible;
    [Header("y•‚ã‚ÌX–³“G‘±‚ÍZ•bz"), SerializeField, Range(0.01f, 5f)] private float secondsForGoUpInvincible;
    /// <summary>
    /// •‚ã‚É‚æ‚é–³“G’†‚Étrue‚ğ•Ô‚·
    /// </summary>
    private bool isInvincible_GoUp;




    [Header("<color=red>=====…’†===============</color>")]
    // Fall
    [Header("y…’†—‰º‚ÌX‰Á‘¬“x‚ÍZz"), SerializeField, Range(0.001f, 0.01f)] private float fallAcceleration;
    [Header("y…’†—‰º‚ÌXÅ‚‘¬“x‚ÍZz"), SerializeField, Range(0.1f, 10f)] private float fallingMaxSpeed_Water;
    private float fallSpeed_Water;
    
    // Swim
    [Header("y—V‰j‚ÌX‘¬“x‚ÍZz"), SerializeField, Range(0.1f, 30f)] private float swimSpeed;
    private InputAction swim;
    private Vector2 inputSwimAxis;
    [Header("y—V‰j‚Ì“ü—Í–³‹‚ÌX”’l‚ÍZz"), SerializeField, Range(0f, 0.5f)] private float ignoreInput_Swim;
    
    // Dash
    [Header("y…’†ƒ_ƒbƒVƒ…‚ÌX‘S‘Ì‚ÍZ•bz"), SerializeField, Range(0f, 5f)] private float secondsForDash_Water;
    [Header("y…’†ƒ_ƒbƒVƒ…‚ÌX‹——£‚ÍZz"), SerializeField, Range(0.1f, 100f)] private float dashDistance_Water;
    [Header("y…’†ƒ_ƒbƒVƒ…‚ÌXƒN[ƒ‹ƒ^ƒCƒ€‚ÍZ•bz"), SerializeField, Range(0f, 5f)] private float secondsForDashCT_Water;
    [Header("y…’†ƒ_ƒbƒVƒ…–³“G‚ÌX‘±‚ÍZ•bz"), SerializeField, Range(0f, 5f)] private float secondsForDashInvincible_Water;

    // Attack
    [Header("y…’†UŒ‚‚ÌX‘OƒXƒL‚ÍZ•bz"), SerializeField, Range(0f, 1f)] private float secondsForWaterAttack_before;
    [Header("y…’†UŒ‚‚ÌX‘±‚ÍZ•bz"), SerializeField, Range(0f, 1f)] private float secondsForWaterAttacking;
    [Header("y…’†UŒ‚‚ÌX‘S‘Ì‚ÍZ•bz"), SerializeField, Range(0f, 3f)] private float secondsForWaterAttack;
    [Header("y…’†UŒ‚‚ÌXƒN[ƒ‹ƒ^ƒCƒ€‚ÍZ•bz"), SerializeField, Range(0f, 5f)] private float secondsForWaterAttackCT;

    // KnockBack
    [Header("y…’†ƒmƒbƒNƒoƒbƒN‚ÌX‘¬“x‚ÍZz"), SerializeField, Range(0f, 50f)] private float waterKnockBackSpeed_First;
    [Header("y…’†ƒmƒbƒNƒoƒbƒN‚ÌXŒ¸‘¬ŠJn‚ÍZ•bz"), SerializeField, Range(0f, 3f)] private float knockBackWeakeningStartSecond_Water;
    [Header("y…’†ƒmƒbƒNƒoƒbƒN‚ÌXŒ¸‘¬‚Ì‹­‚³‚ÍZz"), SerializeField, Range(0f, 10f)] private float knockBackWeakeningPower_Water;
    [Header("y…’†ƒmƒbƒNƒoƒbƒN‚ÌXŒ¸‘¬‘±‚ÍZ•bz"), SerializeField, Range(0f, 3f)] private float secondsForKnockBackWeakening_Water;
    private float elapsedSeconds_WaterKnockBack;
    private Vector2 waterKnockBackAngle;
    private float waterKnockBackSpeed;
    /// <summary>
    /// …’†ƒmƒbƒNƒoƒbƒN’†‚Étrue‚ğ•Ô‚·
    /// </summary>
    private bool isKnockBack_Water;
    /// <summary>
    /// …’†ƒmƒbƒNƒoƒbƒN‚ÅŒ¸‘¬’†‚Étrue‚ğ•Ô‚·
    /// </summary>
    private bool isWeakening_Water = false;



    [Header("<color=red>=====‚»‚Ì‘¼===============</color>")]
    [SerializeField] private Environment currentEnvironment;
    public string CurrentEnvironment => currentEnvironment.ToString();
    //private static Player instance;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private Transform myTransform;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D myCollider;
    [Header("ƒfƒtƒHƒ‹ƒg‚ÌXƒRƒ‰ƒCƒ_[ƒTƒCƒY‚ÍZ"), SerializeField] private Vector2 colliderSize_default = new Vector2(0.55f, 1.75f);
    [Header("ƒfƒtƒHƒ‹ƒg‚ÌXƒRƒ‰ƒCƒ_[‚Ì’†SˆÊ’u‚ÍZ"), SerializeField] private Vector2 colliderOffset_default = new Vector2(0.03f, -0.34f);
    [SerializeField] private SpriteRenderer myRenderer;
    [SerializeField] private Sprite TestPlayerSprite_default;
    [SerializeField] private Sprite TestPlayerSprite_squat;
    [SerializeField] private Sprite TestPlayerSprite_invincible;
    [SerializeField] private Sprite TestPlayerSprite_squatInvincible;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private LayerMask environmentLayer;
    private ActionButtonTarget actionButtonTarget;
    [SerializeField] private SpriteRenderer actionIcon;
    [Header("IƒtƒLƒ_ƒV‚Ì•\¦ˆÊ’u‚ÍZ"), SerializeField] private Vector3 iconPosOffset;
    private GameObject goal;

    /// <summary>
    /// ålŒö‚ª—¤ã‚É‚¢‚é‚©…’†‚É‚¢‚é‚©
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
        // ƒGƒ‰[ƒƒO
        if (jumpSpeed > fallingMaxSpeed)
        {
            jumpSpeed = fallingMaxSpeed;
            Debug.LogError("yƒWƒƒƒ“ƒv‚ÌX‰‘¬“x‚ÍZz‚Íy—‰º‚ÌXÅ‚‘¬“x‚ÍZz‚ğ‰z‚¦‚È‚¢‚æ‚¤‚Éİ’è‚µ‚Ä‚­‚¾‚³‚¢");
        }

        if (secondsForDash_Ground > secondsForDashCT_Ground)
        {
            Debug.LogError("yƒ_ƒbƒVƒ…‚ÌX‘S‘Ì‚ÍZ•bz‚æ‚èyƒ_ƒbƒVƒ…/ƒXƒ‰ƒCƒfƒBƒ“ƒO‚ÌXƒN[ƒ‹ƒ^ƒCƒ€‚ÍZ•bz‚ª‘å‚«‚­‚È‚é‚æ‚¤İ’è‚µ‚Ä‚­‚¾‚³‚¢");
        }
        else if (secondsForDash_Water > secondsForDashCT_Water)
        {
            Debug.LogError("y…’†ƒ_ƒbƒVƒ…‚ÌX‘S‘Ì‚ÍZ•bz‚æ‚èy…’†ƒ_ƒbƒVƒ…‚ÌXƒN[ƒ‹ƒ^ƒCƒ€‚ÍZ•bz‚ª‘å‚«‚­‚È‚é‚æ‚¤İ’è‚µ‚Ä‚­‚¾‚³‚¢");
        }
        else if (secondsForSliding > secondsForDashCT_Ground)
        {
            Debug.LogError("yƒXƒ‰ƒCƒfƒBƒ“ƒO‚ÌX‘S‘Ì‚ÍZ•bz‚æ‚èyƒ_ƒbƒVƒ…/ƒXƒ‰ƒCƒfƒBƒ“ƒO‚ÌXƒN[ƒ‹ƒ^ƒCƒ€‚ÍZ•bz‚ª‘å‚«‚­‚È‚é‚æ‚¤İ’è‚µ‚Ä‚­‚¾‚³‚¢");
        }

        if(secondsForGroundAttack_before + secondsForGroundAttacking > secondsForGroundAttack)
        {
            secondsForGroundAttack = secondsForGroundAttack_before + secondsForGroundAttacking;
            Debug.LogError("yUŒ‚‚ÌX‘S‘Ì‚ÍZ•bz‚ÍyUŒ‚‚ÌX‘OƒXƒL‚ÍZ•bz‚ÆyUŒ‚‚ÌX‘±‚ÍZ•bz‚Ì‡ŒvˆÈã‚Å‚ ‚é•K—v‚ª‚ ‚è‚Ü‚·B");
        }

        if (secondsForWaterAttack_before + secondsForWaterAttacking > secondsForWaterAttack)
        {
            secondsForWaterAttack = secondsForWaterAttack_before + secondsForWaterAttacking;
            Debug.LogError("y…’†UŒ‚‚ÌX‘S‘Ì‚ÍZ•bz‚Íy…’†UŒ‚‚ÌX‘OƒXƒL‚ÍZ•bz‚Æy…’†UŒ‚‚ÌX‘±‚ÍZ•bz‚Ì‡ŒvˆÈã‚Å‚ ‚é•K—v‚ª‚ ‚è‚Ü‚·B");
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

        // ‰æ‘œØ‚è‘Ö‚¦
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
                    // ŒÅ’è’†‚Ì‹““®
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
    /// ˆÚ“®“ü—Í‚ğó‚¯æ‚èA¬‚³‚¢“ü—Í‚ğ–³‹‚·‚é
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
    /// ˆÚ“®•ûŒü‚ğŒü‚­
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
            // Œ¸‘¬
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
            // Œ¸‘¬
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

        if (!isElapsed_Dash) // Õ“Ë‚µ‚½‚¯‚ÇisHit_Dash‚ªtrue‚É‚È‚ç‚È‚¢ê‡‚ ‚èA‘€ì•s”\‘Îô
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
                else if (collision.GetContact(0).normal == Vector2.down) // “Vˆä‚ÆÚG
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
                else if(collision.GetContact(0).normal == Vector2.down) // “Vˆä‚ÆÚG
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

                if (contact.normal.x == 0)// °‚ÆÚG
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

                if (contact.normal == Vector2.up)// °‚ÆÚG
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
                else if (contact.normal.x != 0)// •Ç‚ÆÚG
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
        StartCoroutine(DamageFreezeCoroutine(position - (myTransform.position + (Vector3)myCollider.offset))); // ålŒö¨“G
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
            //Debug.Log("Å‰");
            rb.velocity = Vector2.zero;

            if (enemyRelativeXPos > 0) // ¶‚©‚ç
            {
                rb.AddForce(Quaternion.Euler(0, 0, knockBackAngle) * Vector2.up * knockBackPower, ForceMode2D.Impulse);
            }
            else if (enemyRelativeXPos < 0) // ‰E‚©‚ç
            {
                rb.AddForce(Quaternion.Euler(0, 0, -knockBackAngle) * Vector2.up * knockBackPower, ForceMode2D.Impulse);
            }
            else // ^ã/^‰º‚©‚ç
            {
                rb.AddForce(Quaternion.Euler(0, 0, knockBackAngle * myTransform.localScale.x) * Vector2.up * knockBackPower, ForceMode2D.Impulse);
            }
            isFinishedFirstAction = true;
        }
        else
        {
            if (!isWeakening_Ground)
            {
                //Debug.Log("‘Ò‚¿");
            }
            else
            {
                //Debug.Log("Œ¸‘¬’†");
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
                Debug.Log("‘OƒXƒL");
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
                    Debug.Log("UŒ‚’†");
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
                        Debug.Log("ƒ‚[ƒVƒ‡ƒ“Š®—¹‘Ò‹@");
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
    /// ƒAƒNƒVƒ‡ƒ“ƒ}ƒbƒv‚ğ“K‹XØ‚è‘Ö‚¦‚é
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
                //ChangeInput()‚Å‚ÍUI‚ÉØ‚è‘Ö‚¦‚ç‚ê‚Ä‚µ‚Ü‚¤‚½‚ß
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
                    // ‘ÎÛ‚Ì•û‚ğŒü‚­
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
