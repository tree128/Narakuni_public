using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tsubame : MonoBehaviour
{
    [Header("【ツバメのX体力は〇】"), SerializeField, Range(1, 1000)] private int hp;
    private bool isGetDamage = false;
    private float elapsedTime_Damage = 0f;
    [Header("【ボス撃破の一時停止は〇秒】"), SerializeField, Range(0.01f, 10f)] private float pauseTime;

    [Header("<color=red>ダメージ</color>"), Space(5f)]
    [Header("【つつきのXダメージは〇】"), SerializeField, Range(1, 50)] private int pickAttackDamage;
    [Header("【翼叩きつけのXダメージは〇】"), SerializeField, Range(1, 50)] private int wingAttackDamage;
    [Header("【爆裂卵のXダメージは〇】"), SerializeField, Range(1, 50)] private int bombEggAttackDamage;

    private float moveDirection;
    private Vector3 moveFirstVector = new Vector3(0.01f, 0f, 0f);
    private Vector3 moveSpeed;
    [Header("<color=red>移動</color>"), Space(5f)]
    [Header("【攻撃１＆２のX横移動加速度は〇】"), SerializeField, Range(0.01f, 10f)] private float acceleration;
    [Header("【攻撃１＆２のX横移動最高速度は〇】"), SerializeField, Range(0.1f, 10f)] private float moveMaxSpeed;
    private float origin;
    [Header("攻撃１＆２のX横移動減速度は〇"), SerializeField, Range(0.01f, 10f)] private float deceleration;
    [Header("攻撃１＆２のX横移動揺れ幅は〇"), SerializeField, Range(0.1f, 10f)] private float offset;
    [SerializeField] private Transform leftBlocker;
    [SerializeField] private Transform rightBlocker;
    private bool isMove;

    /// <summary>
    /// 事前時間にtrueを返す
    /// </summary>
    private bool isWaiting_Before = false;
    /// <summary>
    /// 攻撃１＆２の攻撃間隔中にtrueを返す
    /// </summary>
    private bool isWaiting_BetweenAttack = false;
    /// <summary>
    /// 攻撃パターンが変わる待ち時間にtrueを返す
    /// </summary>
    private bool isWaiting_BetweenSet = false;

    [Header("<color=red>攻撃１＆２</color>"), Space(5f)]
    [Header("【攻撃１＆２のX事前時間は〇秒】"), SerializeField, Range(0.01f, 5f)] private float secondsForBefore;
    [Header("【攻撃１＆２のX攻撃間隔は〇秒】"), SerializeField, Range(0.01f, 3f)] private float secondsForWait_Attack;
    [Header("【攻撃１＆２のXセット間隔は〇秒】"), SerializeField, Range(1f, 11f)] private float secondsForWait_Set;
    [Header("【攻撃１＆２のXセット回数は〇】"), SerializeField, Range(1, 5)] private int attack_SetNum;
    private float elapsedTime_Attack = 0f;
    private float waitTime_Before;
    private int attackCount = 0;
    private bool leftStart;
    private int setCount = 0;
    /// <summary>
    /// 現在の攻撃パターン(1と3:爆裂卵かつつき、2:翼、4:絨毯爆撃)
    /// </summary>
    private int currentAttack = 1;
    private KindOfAttack[] pattern = new KindOfAttack[3];
    [Tooltip("爆裂卵×１、つつき×２")]
    [Header("【攻撃１＆２のXパターンA発生確率は〇】"), SerializeField, Range(0f, 100f)] private float rate_patternA;
    [Tooltip("爆裂卵×２、つつき×１")]
    [Header("【攻撃１＆２のXパターンB発生確率は〇】"), SerializeField, Range(0f, 100f)] private float rate_patternB;
    private enum KindOfAttack
    {
        bombEgg = 1,
        pick = 2,
        wing = 3,
        carpetBomb = 4
    }
    [Header("<color=red>攻撃１＿爆裂卵</color>"), Space(5f)]
    [Header("【爆裂卵のX予備動作は〇秒】"), SerializeField, Range(0.01f, 3f)] private float secondsForBeforeEgg;
    [Header("【爆裂卵のX着弾位置左端は〇】"), SerializeField] private Transform eggImpactBlocker_Left;
    [Header("【爆裂卵のX着弾位置右端は〇】"), SerializeField] private Transform eggImpactBlocker_Right;

    [Header("<color=red>攻撃２＿つつき</color>"), Space(5f)]
    [Header("【つつきのX予備動作は〇秒】"), SerializeField, Range(0.01f, 3f)] private float secondsForBeforePick;
    [Header("【つつきのX速度は〇秒】"), SerializeField, Range(0.01f, 3f)] private float secondsForPick;
    [Header("【つつきのX振り下ろし時間は〇秒】"), SerializeField, Range(0.01f, 3f)] private float secondsForWaitingToUp;

    [Header("<color=red>攻撃３＿翼叩きつけ</color>"), Space(5f)]
    [Header("翼叩きつけの初回は片翼で行う"), SerializeField] private bool firstWingAttackIsOne;
    [Header("【翼叩きつけのX事前時間は〇秒】"), SerializeField, Range(0.01f, 10f)] private float secondsForBeforeWingAttack;
    private bool isBothWing = false;
    private bool[] wingAttackHistory;
    private bool isWingAttacking = false;
    private Vector3 currentPos;
    private Vector3 targetPos;
    private float elapsedTime_WingAttack = 0f;
    [Header("【翼叩きつけのXロックオン位置は〇】"), SerializeField, Range(0f, 10f)] private float lockOnOffset;
    [SerializeField] private Transform lockOnLeftBlocker;
    [SerializeField] private Transform lockOnRightBlocker;
    [Header("【翼叩きつけのX予備動作は〇秒】"), SerializeField, Range(0.01f, 10f)] private float secondsForMoveToLockOnPos;
    [Header("【翼叩きつけのX振り下げ速度は〇秒】"), SerializeField, Range(0.01f, 10f)] private float secondsForSwingDown;
    [Header("【翼叩きつけのX振り下げ後待機は〇秒】"), SerializeField, Range(0.01f, 10f)] private float secondsForWaitAfterSwingDown;
    private float moveSpeed_swingAttack;
    private float moveDirection_swingAttack;
    [Header("【翼叩きつけ水中のX移動初速度は〇】"), SerializeField, Range(0.1f, 30f)] private float firstSpeed_wingAttackInWater;
    [Header("【翼叩きつけ水中のX移動加速度は〇】"), SerializeField, Range(0.1f, 30f)] private float acceleration_wingAttackInWater;
    [Header("【翼叩きつけ水中のX移動減速開始は〇秒】"), SerializeField, Range(0.01f, 10f)] private float secondsForWaitDeceleration_wingAttackInWater;
    [Header("【翼叩きつけ水中のX移動減速度は〇】"), SerializeField, Range(0.1f, 30f)] private float deceleration_wingAttackInWater;
    [Header("【翼叩きつけのX待機は〇秒】"), SerializeField, Range(0.01f, 10f)] private float secondsForWaitAfterWingAttack;
    [SerializeField] private Transform wingMoveLeftBlocker;
    [SerializeField] private Transform wingMoveRightBlocker;

    [Header("<color=red>攻撃４＿絨毯爆撃</color>"), Space(5f)]
    [Header("【絨毯爆撃のX事前時間は〇秒】"), SerializeField, Range(0.01f, 5f)] private float secondsForBeforeCarpetAttack;
    [Header("【絨毯爆撃のX横移動減速度は〇】"), SerializeField, Range(0.1f, 5f)] private float deceleration_carpet;
    [Header("【絨毯爆撃のX予備動作は〇秒】"), SerializeField, Range(0.01f, 5f)] private float secondsForBeforeShooting;
    [Header("【絨毯爆撃のX発生開始は〇秒】"), SerializeField, Range(0.01f, 5f)] private float secondsForStartingToFall;
    private WaitForSeconds WFS_StartingToFall;
    [Header("【絨毯爆撃のX卵発射時間は〇秒】"), SerializeField, Range(0.01f, 5f)] private float secondsForShooting;
    [Header("【絨毯爆撃のX待機時間は〇秒】"), SerializeField, Range(0.01f, 5f)] private float secondsForWaitAfterCarpetAttack;
    [SerializeField] private CarpetBombEgg carpetBombEggPrefab;
    [Header("【絨毯爆撃のX個数は〇】"), SerializeField, Range(1, 30)] private int eggNum;
    [SerializeField] private Transform[] eggSpawnRange;
    private List<float> spawnXPosList = new List<float>();
    [Header("【絨毯爆撃のX発生頻度は〇秒】"), SerializeField, Range(0.01f, 3f)] private float secondsForWaitSpawn;
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
            Debug.LogError("【セット間隔】を調整しました\n【攻撃間隔】が【予備動作】＋【速度】*2＋【振り下ろし時間】より短いなら\n【セット間隔】は【予備動作】＋【速度】*2＋【振り下ろし時間】−【攻撃間隔】より長くないといけません");
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
                Invoke("GameClear", 0.1f); // 時間指定なしだと一時停止前に実行されてしまう
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
            // 減速
            moveSpeed.x = Mathf.Clamp(Mathf.Abs(moveSpeed.x) - deceleration * Time.deltaTime, 0, Mathf.Abs(moveSpeed.x)) * moveDirection;
            if (Mathf.Abs(moveSpeed.x) <= 0.01f)
            {
                MoveInit();
            }
        }
        else
        {
            // 加速
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

        if (attackCount == 1) // 首�@
        {
            // 爆裂卵とつつきの内訳を決める
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
                // 左→右→中央
                leftStart = true;
                NeckAttack(leftNeck, pattern[0]);
            }
            else
            {
                // 右→左→中央
                leftStart = false;
                NeckAttack(rightNeck, pattern[0]);
            }
            isWaiting_BetweenAttack = true;
        }

        if (attackCount == 2) // 首�A
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

        if (attackCount == 3) // 首�B(中央)
        {
            NeckAttack(centerNeck, pattern[2]);
            setCount++;
            if(setCount < attack_SetNum)
            {
                isWaiting_BetweenSet = true;
            }
            else
            {
                // 攻撃３か攻撃４へ移行
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
            //Debug.Log("予備動作");
            if(renderers[1].CurrentStatus != TsubameRenderer.Status.damaging && renderers[1].CurrentStatus != TsubameRenderer.Status.before)
            {
                // 首はインデックス1, 2, 3
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
                //Debug.Log("発射中");
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

        // 事前時間のカウントダウン
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

        // 各首の攻撃間隔のカウントダウン
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

        // 攻撃パターン間隔のカウントダウン
        if (isWaiting_BetweenSet)
        {
            if(currentAttack == 1 || currentAttack == 3) // 攻撃１＆２セットの間隔、次も攻撃１＆２
            {
                if (secondsForWait_Set <= elapsedTime_Attack)
                {
                    attackCount = 1;
                    Attack();
                    isWaiting_BetweenSet = false;
                    elapsedTime_Attack = 0;
                }
            }

            if(currentAttack == 2) // 翼攻撃後の待機時間
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

            if(currentAttack == 4) // 絨毯爆撃後の待機時間
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
    /// プレイヤーにダメージを与える
    /// </summary>
    /// <param name="attackPos">攻撃発生位置</param>
    /// <param name="kindOfAttack">1 = 爆裂卵、2 = つつき、3 = 翼</param>
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
