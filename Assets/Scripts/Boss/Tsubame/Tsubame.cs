using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tsubame : MonoBehaviour
{
    [Header("yƒcƒoƒ‚ÌX‘Ì—Í‚ÍZz"), SerializeField, Range(1, 1000)] private int hp;
    private bool isGetDamage = false;
    private float elapsedTime_Damage = 0f;
    [Header("yƒ{ƒXŒ‚”j‚Ìˆê’â~‚ÍZ•bz"), SerializeField, Range(0.01f, 10f)] private float pauseTime;

    [Header("<color=red>ƒ_ƒ[ƒW</color>"), Space(5f)]
    [Header("y‚Â‚Â‚«‚ÌXƒ_ƒ[ƒW‚ÍZz"), SerializeField, Range(1, 50)] private int pickAttackDamage;
    [Header("y—ƒ’@‚«‚Â‚¯‚ÌXƒ_ƒ[ƒW‚ÍZz"), SerializeField, Range(1, 50)] private int wingAttackDamage;
    [Header("y”š—ô—‘‚ÌXƒ_ƒ[ƒW‚ÍZz"), SerializeField, Range(1, 50)] private int bombEggAttackDamage;

    private float moveDirection;
    private Vector3 moveFirstVector = new Vector3(0.01f, 0f, 0f);
    private Vector3 moveSpeed;
    [Header("<color=red>ˆÚ“®</color>"), Space(5f)]
    [Header("yUŒ‚‚P•‚Q‚ÌX‰¡ˆÚ“®‰Á‘¬“x‚ÍZz"), SerializeField, Range(0.01f, 10f)] private float acceleration;
    [Header("yUŒ‚‚P•‚Q‚ÌX‰¡ˆÚ“®Å‚‘¬“x‚ÍZz"), SerializeField, Range(0.1f, 10f)] private float moveMaxSpeed;
    private float origin;
    [Header("UŒ‚‚P•‚Q‚ÌX‰¡ˆÚ“®Œ¸‘¬“x‚ÍZ"), SerializeField, Range(0.01f, 10f)] private float deceleration;
    [Header("UŒ‚‚P•‚Q‚ÌX‰¡ˆÚ“®—h‚ê•‚ÍZ"), SerializeField, Range(0.1f, 10f)] private float offset;
    [SerializeField] private Transform leftBlocker;
    [SerializeField] private Transform rightBlocker;
    private bool isMove;

    /// <summary>
    /// –‘OŠÔ‚Étrue‚ğ•Ô‚·
    /// </summary>
    private bool isWaiting_Before = false;
    /// <summary>
    /// UŒ‚‚P•‚Q‚ÌUŒ‚ŠÔŠu’†‚Étrue‚ğ•Ô‚·
    /// </summary>
    private bool isWaiting_BetweenAttack = false;
    /// <summary>
    /// UŒ‚ƒpƒ^[ƒ“‚ª•Ï‚í‚é‘Ò‚¿ŠÔ‚Étrue‚ğ•Ô‚·
    /// </summary>
    private bool isWaiting_BetweenSet = false;

    [Header("<color=red>UŒ‚‚P•‚Q</color>"), Space(5f)]
    [Header("yUŒ‚‚P•‚Q‚ÌX–‘OŠÔ‚ÍZ•bz"), SerializeField, Range(0.01f, 5f)] private float secondsForBefore;
    [Header("yUŒ‚‚P•‚Q‚ÌXUŒ‚ŠÔŠu‚ÍZ•bz"), SerializeField, Range(0.01f, 3f)] private float secondsForWait_Attack;
    [Header("yUŒ‚‚P•‚Q‚ÌXƒZƒbƒgŠÔŠu‚ÍZ•bz"), SerializeField, Range(1f, 11f)] private float secondsForWait_Set;
    [Header("yUŒ‚‚P•‚Q‚ÌXƒZƒbƒg‰ñ”‚ÍZz"), SerializeField, Range(1, 5)] private int attack_SetNum;
    private float elapsedTime_Attack = 0f;
    private float waitTime_Before;
    private int attackCount = 0;
    private bool leftStart;
    private int setCount = 0;
    /// <summary>
    /// Œ»İ‚ÌUŒ‚ƒpƒ^[ƒ“(1‚Æ3:”š—ô—‘‚©‚Â‚Â‚«A2:—ƒA4:ãOŸ~”šŒ‚)
    /// </summary>
    private int currentAttack = 1;
    private KindOfAttack[] pattern = new KindOfAttack[3];
    [Tooltip("”š—ô—‘~‚PA‚Â‚Â‚«~‚Q")]
    [Header("yUŒ‚‚P•‚Q‚ÌXƒpƒ^[ƒ“A”­¶Šm—¦‚ÍZz"), SerializeField, Range(0f, 100f)] private float rate_patternA;
    [Tooltip("”š—ô—‘~‚QA‚Â‚Â‚«~‚P")]
    [Header("yUŒ‚‚P•‚Q‚ÌXƒpƒ^[ƒ“B”­¶Šm—¦‚ÍZz"), SerializeField, Range(0f, 100f)] private float rate_patternB;
    private enum KindOfAttack
    {
        bombEgg = 1,
        pick = 2,
        wing = 3,
        carpetBomb = 4
    }
    [Header("<color=red>UŒ‚‚PQ”š—ô—‘</color>"), Space(5f)]
    [Header("y”š—ô—‘‚ÌX—\”õ“®ì‚ÍZ•bz"), SerializeField, Range(0.01f, 3f)] private float secondsForBeforeEgg;
    [Header("y”š—ô—‘‚ÌX’…’eˆÊ’u¶’[‚ÍZz"), SerializeField] private Transform eggImpactBlocker_Left;
    [Header("y”š—ô—‘‚ÌX’…’eˆÊ’u‰E’[‚ÍZz"), SerializeField] private Transform eggImpactBlocker_Right;

    [Header("<color=red>UŒ‚‚QQ‚Â‚Â‚«</color>"), Space(5f)]
    [Header("y‚Â‚Â‚«‚ÌX—\”õ“®ì‚ÍZ•bz"), SerializeField, Range(0.01f, 3f)] private float secondsForBeforePick;
    [Header("y‚Â‚Â‚«‚ÌX‘¬“x‚ÍZ•bz"), SerializeField, Range(0.01f, 3f)] private float secondsForPick;
    [Header("y‚Â‚Â‚«‚ÌXU‚è‰º‚ë‚µŠÔ‚ÍZ•bz"), SerializeField, Range(0.01f, 3f)] private float secondsForWaitingToUp;

    [Header("<color=red>UŒ‚‚RQ—ƒ’@‚«‚Â‚¯</color>"), Space(5f)]
    [Header("—ƒ’@‚«‚Â‚¯‚Ì‰‰ñ‚Í•Ğ—ƒ‚Ås‚¤"), SerializeField] private bool firstWingAttackIsOne;
    [Header("y—ƒ’@‚«‚Â‚¯‚ÌX–‘OŠÔ‚ÍZ•bz"), SerializeField, Range(0.01f, 10f)] private float secondsForBeforeWingAttack;
    private bool isBothWing = false;
    private bool[] wingAttackHistory;
    private bool isWingAttacking = false;
    private Vector3 currentPos;
    private Vector3 targetPos;
    private float elapsedTime_WingAttack = 0f;
    [Header("y—ƒ’@‚«‚Â‚¯‚ÌXƒƒbƒNƒIƒ“ˆÊ’u‚ÍZz"), SerializeField, Range(0f, 10f)] private float lockOnOffset;
    [SerializeField] private Transform lockOnLeftBlocker;
    [SerializeField] private Transform lockOnRightBlocker;
    [Header("y—ƒ’@‚«‚Â‚¯‚ÌX—\”õ“®ì‚ÍZ•bz"), SerializeField, Range(0.01f, 10f)] private float secondsForMoveToLockOnPos;
    [Header("y—ƒ’@‚«‚Â‚¯‚ÌXU‚è‰º‚°‘¬“x‚ÍZ•bz"), SerializeField, Range(0.01f, 10f)] private float secondsForSwingDown;
    [Header("y—ƒ’@‚«‚Â‚¯‚ÌXU‚è‰º‚°Œã‘Ò‹@‚ÍZ•bz"), SerializeField, Range(0.01f, 10f)] private float secondsForWaitAfterSwingDown;
    private float moveSpeed_swingAttack;
    private float moveDirection_swingAttack;
    [Header("y—ƒ’@‚«‚Â‚¯…’†‚ÌXˆÚ“®‰‘¬“x‚ÍZz"), SerializeField, Range(0.1f, 30f)] private float firstSpeed_wingAttackInWater;
    [Header("y—ƒ’@‚«‚Â‚¯…’†‚ÌXˆÚ“®‰Á‘¬“x‚ÍZz"), SerializeField, Range(0.1f, 30f)] private float acceleration_wingAttackInWater;
    [Header("y—ƒ’@‚«‚Â‚¯…’†‚ÌXˆÚ“®Œ¸‘¬ŠJn‚ÍZ•bz"), SerializeField, Range(0.01f, 10f)] private float secondsForWaitDeceleration_wingAttackInWater;
    [Header("y—ƒ’@‚«‚Â‚¯…’†‚ÌXˆÚ“®Œ¸‘¬“x‚ÍZz"), SerializeField, Range(0.1f, 30f)] private float deceleration_wingAttackInWater;
    [Header("y—ƒ’@‚«‚Â‚¯‚ÌX‘Ò‹@‚ÍZ•bz"), SerializeField, Range(0.01f, 10f)] private float secondsForWaitAfterWingAttack;
    [SerializeField] private Transform wingMoveLeftBlocker;
    [SerializeField] private Transform wingMoveRightBlocker;

    [Header("<color=red>UŒ‚‚SQãOŸ~”šŒ‚</color>"), Space(5f)]
    [Header("yãOŸ~”šŒ‚‚ÌX–‘OŠÔ‚ÍZ•bz"), SerializeField, Range(0.01f, 5f)] private float secondsForBeforeCarpetAttack;
    [Header("yãOŸ~”šŒ‚‚ÌX‰¡ˆÚ“®Œ¸‘¬“x‚ÍZz"), SerializeField, Range(0.1f, 5f)] private float deceleration_carpet;
    [Header("yãOŸ~”šŒ‚‚ÌX—\”õ“®ì‚ÍZ•bz"), SerializeField, Range(0.01f, 5f)] private float secondsForBeforeShooting;
    [Header("yãOŸ~”šŒ‚‚ÌX”­¶ŠJn‚ÍZ•bz"), SerializeField, Range(0.01f, 5f)] private float secondsForStartingToFall;
    private WaitForSeconds WFS_StartingToFall;
    [Header("yãOŸ~”šŒ‚‚ÌX—‘”­ËŠÔ‚ÍZ•bz"), SerializeField, Range(0.01f, 5f)] private float secondsForShooting;
    [Header("yãOŸ~”šŒ‚‚ÌX‘Ò‹@ŠÔ‚ÍZ•bz"), SerializeField, Range(0.01f, 5f)] private float secondsForWaitAfterCarpetAttack;
    [SerializeField] private CarpetBombEgg carpetBombEggPrefab;
    [Header("yãOŸ~”šŒ‚‚ÌXŒÂ”‚ÍZz"), SerializeField, Range(1, 30)] private int eggNum;
    [SerializeField] private Transform[] eggSpawnRange;
    private List<float> spawnXPosList = new List<float>();
    [Header("yãOŸ~”šŒ‚‚ÌX”­¶•p“x‚ÍZ•bz"), SerializeField, Range(0.01f, 3f)] private float secondsForWaitSpawn;
    private WaitForSeconds WFS_Spawn;
    private bool isCarpetAttacking = false;
    private bool isMove_Carpet = false;
    private float elapsedTime_CarpetAttack = 0f;
    private bool isShooting = false;
    private Coroutine coroutine;

    [SerializeField, Space(5f)] private PlayerData playerData;
    [SerializeField] private BombEggData bombData;
    [SerializeField] private TsubameNeck centerNeck;
    [SerializeField] private TsubameNeck leftNeck;
    [SerializeField] private TsubameNeck rightNeck;
    [SerializeField] private TsubameWing leftWing;
    [SerializeField] private TsubameWing rightWing;
    [SerializeField] private FlagData flagData;
    [SerializeField] private EnemyBaseParameter baseParameter;
    [SerializeField] private TsubameRenderer[] renderers;
    //[SerializeField] private Color defaultColor;

    private void Reset()
    {
        if(flagData == null)
        {
            flagData = FlagData.GetFlagDataAsset();
        }
    }

    private void OnValidate()
    {        
        if(secondsForWait_Attack < secondsForBeforePick + secondsForPick * 2 + secondsForWaitingToUp && secondsForWait_Set < secondsForBeforePick + secondsForPick * 2 + secondsForWaitingToUp - secondsForWait_Attack)
        {
            secondsForWait_Set = secondsForBeforePick + secondsForPick * 2 + secondsForWaitingToUp - secondsForWait_Attack + 0.1f;
            Debug.LogError("yƒZƒbƒgŠÔŠuz‚ğ’²®‚µ‚Ü‚µ‚½\nyUŒ‚ŠÔŠuz‚ªy—\”õ“®ìz{y‘¬“xz*2{yU‚è‰º‚ë‚µŠÔz‚æ‚è’Z‚¢‚È‚ç\nyƒZƒbƒgŠÔŠuz‚Íy—\”õ“®ìz{y‘¬“xz*2{yU‚è‰º‚ë‚µŠÔz|yUŒ‚ŠÔŠuz‚æ‚è’·‚­‚È‚¢‚Æ‚¢‚¯‚Ü‚¹‚ñ");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        bombData.ImpactBlocker_Left = eggImpactBlocker_Left;
        bombData.ImpactBlocker_Right = eggImpactBlocker_Right;

        if (leftBlocker.position.x <= playerData.PlayerTransform.position.x && playerData.PlayerTransform.position.x <= rightBlocker.position.x)
        {
            origin = playerData.PlayerTransform.position.x;
        }
        else if (playerData.PlayerTransform.position.x < leftBlocker.position.x)
        {
            origin = leftBlocker.position.x;
        }
        else
        {
            origin = rightBlocker.position.x;
        }
        MoveInit();
        Attack();
        WFS_StartingToFall = new WaitForSeconds(secondsForStartingToFall);
        WFS_Spawn = new WaitForSeconds(secondsForWaitSpawn);
        wingAttackHistory = new bool[2] { firstWingAttackIsOne, firstWingAttackIsOne };

        int initNum = Mathf.Clamp(eggNum, 1, 10);
        MyObjectPool.Instance.CreatePool(carpetBombEggPrefab, initNum, eggNum);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            Move();
        }

        if (isWaiting_Before || isWaiting_BetweenAttack || isWaiting_BetweenSet)
        {
            AttackCountDown();
        }

        if (isWingAttacking)
        {
            MoveWhileWingAttack();
        }

        if (isMove_Carpet)
        {
            MoveWhileCarpetAttack();
        }

        if (isCarpetAttacking)
        {
            CarpetAttack_Time();
        }

        if (isGetDamage)
        {
            elapsedTime_Damage += Time.deltaTime;
            if(baseParameter.damageTime <= elapsedTime_Damage)
            {
                foreach (var item in renderers)
                {
                    item.StatusChange(TsubameRenderer.Status.normal);
                }
                isGetDamage = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            hp -= playerData.AttackPower;

            foreach (var item in renderers)
            {
                item.StatusChange(TsubameRenderer.Status.damaging);
            }
            elapsedTime_Damage = 0f;
            isGetDamage = true;
            if (hp <= 0)
            {
                GameManager.Instance.GamePause(pauseTime);
                Invoke("GameClear", 0.1f); // ŠÔw’è‚È‚µ‚¾‚Æˆê’â~‘O‚ÉÀs‚³‚ê‚Ä‚µ‚Ü‚¤
            }
        }
    }

    private void GameClear()
    {
        flagData.SetFlag_False(1);
        GameManager.Instance.GameClear();
    }

    private void Move()
    {
        transform.position += moveSpeed * Time.deltaTime;

        if (leftBlocker.position.x <= playerData.PlayerTransform.position.x && playerData.PlayerTransform.position.x <= rightBlocker.position.x && origin != playerData.PlayerTransform.position.x)
        {
            origin = playerData.PlayerTransform.position.x;
        }
        if(playerData.PlayerTransform.position.x < leftBlocker.position.x && origin != leftBlocker.position.x)
        {
            origin = leftBlocker.position.x;
        }
        if(rightBlocker.position.x < playerData.PlayerTransform.position.x && origin != rightBlocker.position.x)
        {
            origin = rightBlocker.position.x;
        }
        
        if((moveDirection == 1 && origin + offset < transform.position.x) || (moveDirection == -1 && transform.position.x < origin - offset))
        {
            // Œ¸‘¬
            moveSpeed.x = Mathf.Clamp(Mathf.Abs(moveSpeed.x) - deceleration * Time.deltaTime, 0, Mathf.Abs(moveSpeed.x)) * moveDirection;
            if (Mathf.Abs(moveSpeed.x) <= 0.01f)
            {
                MoveInit();
            }
        }
        else
        {
            // ‰Á‘¬
            moveSpeed.x = Mathf.Clamp(Mathf.Abs(moveSpeed.x) + acceleration * Time.deltaTime, Mathf.Abs(moveSpeed.x), moveMaxSpeed) * moveDirection;
        }
        
    }

    private void MoveInit()
    {
        moveDirection = Mathf.Sign(origin - transform.position.x);
        moveSpeed = moveFirstVector * moveDirection;
        isMove = true;
    }

    private void Attack()
    {
        if(currentAttack == 1 || currentAttack == 3)
        {
            PickAttackOrEggAttack();
        }
        if(currentAttack == 2)
        {
            WingAttack();
        }
        if(currentAttack == 4)
        {
            CarpetAttack();
        }
    }

    private void PickAttackOrEggAttack()
    {
        if(attackCount == 0)
        {
            waitTime_Before = secondsForBefore;
            isWaiting_Before = true;
        }

        if (attackCount == 1) // ñ‡@
        {
            // ”š—ô—‘‚Æ‚Â‚Â‚«‚Ì“à–ó‚ğŒˆ‚ß‚é
            float value = Random.value * 100;
            if(rate_patternA + rate_patternB < value)
            {
                pattern[0] = KindOfAttack.bombEgg;
                pattern[1] = KindOfAttack.bombEgg;
                pattern[2] = KindOfAttack.bombEgg;
            }
            else
            {
                if (value <= rate_patternA)
                {
                    pattern[0] = KindOfAttack.bombEgg;
                    pattern[1] = KindOfAttack.pick;
                    pattern[2] = KindOfAttack.pick;
                }
                else
                {
                    pattern[0] = KindOfAttack.bombEgg;
                    pattern[1] = KindOfAttack.bombEgg;
                    pattern[2] = KindOfAttack.pick;
                }
                pattern = ShuffleArray(pattern);
            }

            if (Random.Range(0, 2) == 0)
            {
                // ¶¨‰E¨’†‰›
                leftStart = true;
                NeckAttack(leftNeck, pattern[0]);
            }
            else
            {
                // ‰E¨¶¨’†‰›
                leftStart = false;
                NeckAttack(rightNeck, pattern[0]);
            }
            isWaiting_BetweenAttack = true;
        }

        if (attackCount == 2) // ñ‡A
        {
            if (leftStart)
            {
                NeckAttack(rightNeck, pattern[1]);
            }
            else
            {
                NeckAttack(leftNeck, pattern[1]);
            }
            isWaiting_BetweenAttack = true;
        }

        if (attackCount == 3) // ñ‡B(’†‰›)
        {
            NeckAttack(centerNeck, pattern[2]);
            setCount++;
            if(setCount < attack_SetNum)
            {
                isWaiting_BetweenSet = true;
            }
            else
            {
                // UŒ‚‚R‚©UŒ‚‚S‚ÖˆÚs
                currentAttack++;
                attackCount = 0;
                Attack();
            }
        }
    }

    private T[] ShuffleArray<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            var value = array[i];
            int randomIndex = Random.Range(0, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = value;
        }
        return array;
    }

    private void NeckAttack(TsubameNeck attackNeck, KindOfAttack attack)
    {
        if(attack == KindOfAttack.bombEgg)
        {
            attackNeck.BombEggAttackInit(secondsForBeforeEgg);
        }
        else
        {
            attackNeck.PickInit(secondsForBeforePick, secondsForPick, secondsForWaitingToUp);
        }
    }

    private void WingAttack()
    {
        if (attackCount == 0)
        {
            waitTime_Before = secondsForBeforeWingAttack;
            isWaiting_Before = true;
        }
        if (attackCount == 1)
        {
            if (wingAttackHistory.Length == 2 && wingAttackHistory[0] == wingAttackHistory[1])
            {
                isBothWing = !wingAttackHistory[0];
            }
            else
            {
                if (Random.Range(0, 2) == 0)
                {
                    isBothWing = true;
                }
                else
                {
                    isBothWing = false;
                }
            }
            wingAttackHistory[0] = wingAttackHistory[1];
            wingAttackHistory[1] = isBothWing;
            if (playerData.PlayerTransform.position.x <= 0)
            {
                leftWing.InitSwingAttack(secondsForMoveToLockOnPos, secondsForSwingDown, secondsForWaitAfterSwingDown);
                if (isBothWing)
                {
                    rightWing.InitSwingAttack(secondsForMoveToLockOnPos, secondsForSwingDown, secondsForWaitAfterSwingDown);
                }
                moveDirection_swingAttack = 1;
            }
            else
            {
                rightWing.InitSwingAttack(secondsForMoveToLockOnPos, secondsForSwingDown, secondsForWaitAfterSwingDown);
                if (isBothWing)
                {
                    leftWing.InitSwingAttack(secondsForMoveToLockOnPos, secondsForSwingDown, secondsForWaitAfterSwingDown);
                }
                moveDirection_swingAttack = -1;
            }
            currentPos = transform.position;
            float xPos = Mathf.Clamp(playerData.PlayerTransform.position.x + lockOnOffset * Mathf.Sign(-playerData.PlayerTransform.position.x), lockOnLeftBlocker.position.x, lockOnRightBlocker.position.x);
            targetPos = new Vector3(xPos, transform.position.y, 0f);
            moveSpeed_swingAttack = firstSpeed_wingAttackInWater;
            elapsedTime_WingAttack = 0f;
            isWingAttacking = true;
            isMove = false;
        }
    }

    private void MoveWhileWingAttack()
    {
        elapsedTime_WingAttack += Time.deltaTime;
        if (elapsedTime_WingAttack <= secondsForMoveToLockOnPos)
        {
            transform.position = Vector3.Lerp(currentPos, targetPos, elapsedTime_WingAttack / secondsForMoveToLockOnPos);
        }
        if (!isBothWing)
        {
            if (secondsForMoveToLockOnPos + secondsForSwingDown + secondsForWaitAfterSwingDown <= elapsedTime_WingAttack && wingMoveLeftBlocker.position.x < transform.position.x && transform.position.x < wingMoveRightBlocker.position.x)
            {
                transform.position += new Vector3(moveSpeed_swingAttack * moveDirection_swingAttack, 0f, 0f) * Time.deltaTime;

                if (elapsedTime_WingAttack < secondsForMoveToLockOnPos + secondsForSwingDown + secondsForWaitAfterSwingDown + secondsForWaitDeceleration_wingAttackInWater)
                {
                    moveSpeed_swingAttack += acceleration_wingAttackInWater * Time.deltaTime * Mathf.Sign(moveSpeed_swingAttack);
                }
                else
                {
                    moveSpeed_swingAttack = Mathf.Clamp(moveSpeed_swingAttack - deceleration_wingAttackInWater * Time.deltaTime, 0f, moveSpeed_swingAttack);
                }
            }
        }
    }

    public void WingAttackFinish()
    {
        isWingAttacking = false;
        MoveInit();
        isWaiting_BetweenSet = true;
    }

    private void CarpetAttack()
    {
        if (attackCount == 0)
        {
            waitTime_Before = secondsForBeforeCarpetAttack;
            isWaiting_Before = true;
        }
        if (attackCount == 1)
        {
            spawnXPosList.Clear();
            do
            {
                float[] array = new float[3];
                array[0] = Random.Range(eggSpawnRange[0].position.x - eggSpawnRange[0].localScale.x * 0.5f, eggSpawnRange[0].position.x + eggSpawnRange[0].localScale.x * 0.5f);
                array[1] = Random.Range(eggSpawnRange[1].position.x - eggSpawnRange[1].localScale.x * 0.5f, eggSpawnRange[1].position.x + eggSpawnRange[1].localScale.x * 0.5f);
                array[2] = Random.Range(eggSpawnRange[2].position.x - eggSpawnRange[2].localScale.x * 0.5f, eggSpawnRange[2].position.x + eggSpawnRange[2].localScale.x * 0.5f);
                array = ShuffleArray(array);
                spawnXPosList.Add(array[0]);
                spawnXPosList.Add(array[1]);
                spawnXPosList.Add(array[2]);
            } while (spawnXPosList.Count < eggNum);
            leftNeck.CarpetAttack(1);
            rightNeck.CarpetAttack(1);
            centerNeck.CarpetAttack(1);
            isMove = false;
            elapsedTime_CarpetAttack = 0f;
            coroutine = null;
            isShooting = false;
            isCarpetAttacking = true;
            isMove_Carpet = true;
        }
    }

    private void MoveWhileCarpetAttack()
    {
        moveSpeed.x = Mathf.Clamp(Mathf.Abs(moveSpeed.x) - deceleration_carpet * Time.deltaTime, 0, Mathf.Abs(moveSpeed.x)) * moveDirection;
        if(moveSpeed.x == 0)
        {
            isMove_Carpet = false;
        }
        transform.position += moveSpeed * Time.deltaTime;
    }

    private void CarpetAttack_Time()
    {
        elapsedTime_CarpetAttack += Time.deltaTime;
        if(elapsedTime_CarpetAttack < secondsForBeforeShooting)
        {
            //Debug.Log("—\”õ“®ì");
            if(renderers[1].CurrentStatus != TsubameRenderer.Status.damaging && renderers[1].CurrentStatus != TsubameRenderer.Status.before)
            {
                // ñ‚ÍƒCƒ“ƒfƒbƒNƒX1, 2, 3
                for (int i = 1; i < 4; i++)
                {
                    renderers[i].StatusChange(TsubameRenderer.Status.before);
                }
            }
        }
        if(secondsForBeforeShooting <= elapsedTime_CarpetAttack)
        {
            if (!isShooting)
            {
                leftNeck.CarpetAttack(2);
                rightNeck.CarpetAttack(2);
                centerNeck.CarpetAttack(2);
                isShooting = true;
            }
            else
            {
                //Debug.Log("”­Ë’†");
                if (renderers[1].CurrentStatus != TsubameRenderer.Status.damaging && renderers[1].CurrentStatus != TsubameRenderer.Status.attacking)
                {
                    for (int i = 1; i < 4; i++)
                    {
                        renderers[i].StatusChange(TsubameRenderer.Status.attacking);
                    }
                }

                if (coroutine == null)
                {
                    coroutine = StartCoroutine(CarpetCoroutine());
                }
            }
        }
        if(secondsForBeforeShooting + secondsForShooting <= elapsedTime_CarpetAttack)
        {
            isWaiting_BetweenSet = true;
            leftNeck.CarpetAttack(3);
            rightNeck.CarpetAttack(3);
            centerNeck.CarpetAttack(3);
            isCarpetAttacking = false;
        }

    }

    private IEnumerator CarpetCoroutine()
    {
        int count = 0;
        float yPos = eggSpawnRange[0].position.y;
        while (count < eggNum)
        {
            if (count == 0)
            {
                yield return WFS_StartingToFall;
            }
            else
            {
                yield return WFS_Spawn;
            }
            CarpetBombEgg egg = MyObjectPool.Instance.GetFromPool(MyObjectPool.KindOfPool.CarpetBomb, carpetBombEggPrefab);
            egg.transform.position = new Vector3(spawnXPosList[count], yPos, 0f);
            egg.gameObject.SetActive(true);
            count++;
        }
    }

    private void AttackCountDown()
    {
        elapsedTime_Attack += Time.deltaTime;

        // –‘OŠÔ‚ÌƒJƒEƒ“ƒgƒ_ƒEƒ“
        if (isWaiting_Before)
        {
            if (waitTime_Before <= elapsedTime_Attack)
            {
                attackCount++;
                Attack();
                isWaiting_Before = false;
                elapsedTime_Attack = 0;
            }
        }

        // Šeñ‚ÌUŒ‚ŠÔŠu‚ÌƒJƒEƒ“ƒgƒ_ƒEƒ“
        if (isWaiting_BetweenAttack)
        {
            if (elapsedTime_Attack >= secondsForWait_Attack)
            {
                isWaiting_BetweenAttack = false;
                attackCount++;
                Attack();
                elapsedTime_Attack = 0;
            }
        }

        // UŒ‚ƒpƒ^[ƒ“ŠÔŠu‚ÌƒJƒEƒ“ƒgƒ_ƒEƒ“
        if (isWaiting_BetweenSet)
        {
            if(currentAttack == 1 || currentAttack == 3) // UŒ‚‚P•‚QƒZƒbƒg‚ÌŠÔŠuAŸ‚àUŒ‚‚P•‚Q
            {
                if (secondsForWait_Set <= elapsedTime_Attack)
                {
                    attackCount = 1;
                    Attack();
                    isWaiting_BetweenSet = false;
                    elapsedTime_Attack = 0;
                }
            }

            if(currentAttack == 2) // —ƒUŒ‚Œã‚Ì‘Ò‹@ŠÔ
            {
                if (secondsForWaitAfterWingAttack <= elapsedTime_Attack)
                {
                    currentAttack = 3;
                    attackCount = 0;
                    setCount = 0;
                    Attack();
                    isWaiting_BetweenSet = false;
                    elapsedTime_Attack = 0;
                }
            }

            if(currentAttack == 4) // ãOŸ~”šŒ‚Œã‚Ì‘Ò‹@ŠÔ
            {
                if(secondsForWaitAfterCarpetAttack <= elapsedTime_Attack)
                {
                    currentAttack = 1;
                    attackCount = 0;
                    setCount = 0;
                    Attack();
                    isWaiting_BetweenSet = false;
                    elapsedTime_Attack = 0;
                    isMove_Carpet = false;
                    MoveInit();
                }
            }
        }
    }

    /// <summary>
    /// ƒvƒŒƒCƒ„[‚Éƒ_ƒ[ƒW‚ğ—^‚¦‚é
    /// </summary>
    /// <param name="attackPos">UŒ‚”­¶ˆÊ’u</param>
    /// <param name="kindOfAttack">1 = ”š—ô—‘A2 = ‚Â‚Â‚«A3 = —ƒ</param>
    public void DamageToPlayer(Vector3 attackPos, int kindOfAttack)
    {
        int damage = 0;
        if (kindOfAttack == 1)
        {
            damage = bombEggAttackDamage;
        }

        if (kindOfAttack == 2)
        {
            damage = pickAttackDamage;
        }

        if(kindOfAttack == 3)
        {
            damage = wingAttackDamage;
        }

        playerData.Player.GetDamage(attackPos, damage);
    }
}
