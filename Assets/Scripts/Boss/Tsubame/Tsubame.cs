using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tsubame : MonoBehaviour
{
    [Header("�y�c�o����X�̗͂́Z�z"), SerializeField, Range(1, 1000)] private int hp;
    private bool isGetDamage = false;
    private float elapsedTime_Damage = 0f;
    [Header("�y�{�X���j�̈ꎞ��~�́Z�b�z"), SerializeField, Range(0.01f, 10f)] private float pauseTime;

    [Header("<color=red>�_���[�W</color>"), Space(5f)]
    [Header("�y����X�_���[�W�́Z�z"), SerializeField, Range(1, 50)] private int pickAttackDamage;
    [Header("�y���@������X�_���[�W�́Z�z"), SerializeField, Range(1, 50)] private int wingAttackDamage;
    [Header("�y��������X�_���[�W�́Z�z"), SerializeField, Range(1, 50)] private int bombEggAttackDamage;

    private float moveDirection;
    private Vector3 moveFirstVector = new Vector3(0.01f, 0f, 0f);
    private Vector3 moveSpeed;
    [Header("<color=red>�ړ�</color>"), Space(5f)]
    [Header("�y�U���P���Q��X���ړ������x�́Z�z"), SerializeField, Range(0.01f, 10f)] private float acceleration;
    [Header("�y�U���P���Q��X���ړ��ō����x�́Z�z"), SerializeField, Range(0.1f, 10f)] private float moveMaxSpeed;
    private float origin;
    [Header("�U���P���Q��X���ړ������x�́Z"), SerializeField, Range(0.01f, 10f)] private float deceleration;
    [Header("�U���P���Q��X���ړ��h�ꕝ�́Z"), SerializeField, Range(0.1f, 10f)] private float offset;
    [SerializeField] private Transform leftBlocker;
    [SerializeField] private Transform rightBlocker;
    private bool isMove;

    /// <summary>
    /// ���O���Ԃ�true��Ԃ�
    /// </summary>
    private bool isWaiting_Before = false;
    /// <summary>
    /// �U���P���Q�̍U���Ԋu����true��Ԃ�
    /// </summary>
    private bool isWaiting_BetweenAttack = false;
    /// <summary>
    /// �U���p�^�[�����ς��҂����Ԃ�true��Ԃ�
    /// </summary>
    private bool isWaiting_BetweenSet = false;

    [Header("<color=red>�U���P���Q</color>"), Space(5f)]
    [Header("�y�U���P���Q��X���O���Ԃ́Z�b�z"), SerializeField, Range(0.01f, 5f)] private float secondsForBefore;
    [Header("�y�U���P���Q��X�U���Ԋu�́Z�b�z"), SerializeField, Range(0.01f, 3f)] private float secondsForWait_Attack;
    [Header("�y�U���P���Q��X�Z�b�g�Ԋu�́Z�b�z"), SerializeField, Range(1f, 11f)] private float secondsForWait_Set;
    [Header("�y�U���P���Q��X�Z�b�g�񐔂́Z�z"), SerializeField, Range(1, 5)] private int attack_SetNum;
    private float elapsedTime_Attack = 0f;
    private float waitTime_Before;
    private int attackCount = 0;
    private bool leftStart;
    private int setCount = 0;
    /// <summary>
    /// ���݂̍U���p�^�[��(1��3:�����������A2:���A4:�O�~����)
    /// </summary>
    private int currentAttack = 1;
    private KindOfAttack[] pattern = new KindOfAttack[3];
    [Tooltip("�������~�P�A���~�Q")]
    [Header("�y�U���P���Q��X�p�^�[��A�����m���́Z�z"), SerializeField, Range(0f, 100f)] private float rate_patternA;
    [Tooltip("�������~�Q�A���~�P")]
    [Header("�y�U���P���Q��X�p�^�[��B�����m���́Z�z"), SerializeField, Range(0f, 100f)] private float rate_patternB;
    private enum KindOfAttack
    {
        bombEgg = 1,
        pick = 2,
        wing = 3,
        carpetBomb = 4
    }
    [Header("<color=red>�U���P�Q������</color>"), Space(5f)]
    [Header("�y��������X�\������́Z�b�z"), SerializeField, Range(0.01f, 3f)] private float secondsForBeforeEgg;
    [Header("�y��������X���e�ʒu���[�́Z�z"), SerializeField] private Transform eggImpactBlocker_Left;
    [Header("�y��������X���e�ʒu�E�[�́Z�z"), SerializeField] private Transform eggImpactBlocker_Right;

    [Header("<color=red>�U���Q�Q��</color>"), Space(5f)]
    [Header("�y����X�\������́Z�b�z"), SerializeField, Range(0.01f, 3f)] private float secondsForBeforePick;
    [Header("�y����X���x�́Z�b�z"), SerializeField, Range(0.01f, 3f)] private float secondsForPick;
    [Header("�y����X�U�艺�낵���Ԃ́Z�b�z"), SerializeField, Range(0.01f, 3f)] private float secondsForWaitingToUp;

    [Header("<color=red>�U���R�Q���@����</color>"), Space(5f)]
    [Header("���@�����̏���͕З��ōs��"), SerializeField] private bool firstWingAttackIsOne;
    [Header("�y���@������X���O���Ԃ́Z�b�z"), SerializeField, Range(0.01f, 10f)] private float secondsForBeforeWingAttack;
    private bool isBothWing = false;
    private bool[] wingAttackHistory;
    private bool isWingAttacking = false;
    private Vector3 currentPos;
    private Vector3 targetPos;
    private float elapsedTime_WingAttack = 0f;
    [Header("�y���@������X���b�N�I���ʒu�́Z�z"), SerializeField, Range(0f, 10f)] private float lockOnOffset;
    [SerializeField] private Transform lockOnLeftBlocker;
    [SerializeField] private Transform lockOnRightBlocker;
    [Header("�y���@������X�\������́Z�b�z"), SerializeField, Range(0.01f, 10f)] private float secondsForMoveToLockOnPos;
    [Header("�y���@������X�U�艺�����x�́Z�b�z"), SerializeField, Range(0.01f, 10f)] private float secondsForSwingDown;
    [Header("�y���@������X�U�艺����ҋ@�́Z�b�z"), SerializeField, Range(0.01f, 10f)] private float secondsForWaitAfterSwingDown;
    private float moveSpeed_swingAttack;
    private float moveDirection_swingAttack;
    [Header("�y���@����������X�ړ������x�́Z�z"), SerializeField, Range(0.1f, 30f)] private float firstSpeed_wingAttackInWater;
    [Header("�y���@����������X�ړ������x�́Z�z"), SerializeField, Range(0.1f, 30f)] private float acceleration_wingAttackInWater;
    [Header("�y���@����������X�ړ������J�n�́Z�b�z"), SerializeField, Range(0.01f, 10f)] private float secondsForWaitDeceleration_wingAttackInWater;
    [Header("�y���@����������X�ړ������x�́Z�z"), SerializeField, Range(0.1f, 30f)] private float deceleration_wingAttackInWater;
    [Header("�y���@������X�ҋ@�́Z�b�z"), SerializeField, Range(0.01f, 10f)] private float secondsForWaitAfterWingAttack;
    [SerializeField] private Transform wingMoveLeftBlocker;
    [SerializeField] private Transform wingMoveRightBlocker;

    [Header("<color=red>�U���S�Q�O�~����</color>"), Space(5f)]
    [Header("�y�O�~������X���O���Ԃ́Z�b�z"), SerializeField, Range(0.01f, 5f)] private float secondsForBeforeCarpetAttack;
    [Header("�y�O�~������X���ړ������x�́Z�z"), SerializeField, Range(0.1f, 5f)] private float deceleration_carpet;
    [Header("�y�O�~������X�\������́Z�b�z"), SerializeField, Range(0.01f, 5f)] private float secondsForBeforeShooting;
    [Header("�y�O�~������X�����J�n�́Z�b�z"), SerializeField, Range(0.01f, 5f)] private float secondsForStartingToFall;
    private WaitForSeconds WFS_StartingToFall;
    [Header("�y�O�~������X�����ˎ��Ԃ́Z�b�z"), SerializeField, Range(0.01f, 5f)] private float secondsForShooting;
    [Header("�y�O�~������X�ҋ@���Ԃ́Z�b�z"), SerializeField, Range(0.01f, 5f)] private float secondsForWaitAfterCarpetAttack;
    [SerializeField] private CarpetBombEgg carpetBombEggPrefab;
    [Header("�y�O�~������X���́Z�z"), SerializeField, Range(1, 30)] private int eggNum;
    [SerializeField] private Transform[] eggSpawnRange;
    private List<float> spawnXPosList = new List<float>();
    [Header("�y�O�~������X�����p�x�́Z�b�z"), SerializeField, Range(0.01f, 3f)] private float secondsForWaitSpawn;
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
            Debug.LogError("�y�Z�b�g�Ԋu�z�𒲐����܂���\n�y�U���Ԋu�z���y�\������z�{�y���x�z*2�{�y�U�艺�낵���ԁz���Z���Ȃ�\n�y�Z�b�g�Ԋu�z�́y�\������z�{�y���x�z*2�{�y�U�艺�낵���ԁz�|�y�U���Ԋu�z��蒷���Ȃ��Ƃ����܂���");
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
                Invoke("GameClear", 0.1f); // ���Ԏw��Ȃ����ƈꎞ��~�O�Ɏ��s����Ă��܂�
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
            // ����
            moveSpeed.x = Mathf.Clamp(Mathf.Abs(moveSpeed.x) - deceleration * Time.deltaTime, 0, Mathf.Abs(moveSpeed.x)) * moveDirection;
            if (Mathf.Abs(moveSpeed.x) <= 0.01f)
            {
                MoveInit();
            }
        }
        else
        {
            // ����
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

        if (attackCount == 1) // ��@
        {
            // �������Ƃ��̓�������߂�
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
                // �����E������
                leftStart = true;
                NeckAttack(leftNeck, pattern[0]);
            }
            else
            {
                // �E����������
                leftStart = false;
                NeckAttack(rightNeck, pattern[0]);
            }
            isWaiting_BetweenAttack = true;
        }

        if (attackCount == 2) // ��A
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

        if (attackCount == 3) // ��B(����)
        {
            NeckAttack(centerNeck, pattern[2]);
            setCount++;
            if(setCount < attack_SetNum)
            {
                isWaiting_BetweenSet = true;
            }
            else
            {
                // �U���R���U���S�ֈڍs
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
            //Debug.Log("�\������");
            if(renderers[1].CurrentStatus != TsubameRenderer.Status.damaging && renderers[1].CurrentStatus != TsubameRenderer.Status.before)
            {
                // ��̓C���f�b�N�X1, 2, 3
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
                //Debug.Log("���˒�");
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

        // ���O���Ԃ̃J�E���g�_�E��
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

        // �e��̍U���Ԋu�̃J�E���g�_�E��
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

        // �U���p�^�[���Ԋu�̃J�E���g�_�E��
        if (isWaiting_BetweenSet)
        {
            if(currentAttack == 1 || currentAttack == 3) // �U���P���Q�Z�b�g�̊Ԋu�A�����U���P���Q
            {
                if (secondsForWait_Set <= elapsedTime_Attack)
                {
                    attackCount = 1;
                    Attack();
                    isWaiting_BetweenSet = false;
                    elapsedTime_Attack = 0;
                }
            }

            if(currentAttack == 2) // ���U����̑ҋ@����
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

            if(currentAttack == 4) // �O�~������̑ҋ@����
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
    /// �v���C���[�Ƀ_���[�W��^����
    /// </summary>
    /// <param name="attackPos">�U�������ʒu</param>
    /// <param name="kindOfAttack">1 = �������A2 = ���A3 = ��</param>
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
