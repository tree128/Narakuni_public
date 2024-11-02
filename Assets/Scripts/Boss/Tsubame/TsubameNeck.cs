using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TsubameNeck : MonoBehaviour
{
    [SerializeField] private BoxCollider2D attackCollider;// プレイヤーへのダメージ
    [SerializeField] private TsubameRenderer myRenderer;
    private bool isPickAttack = false;
    private float elapsedTime_Pick;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float secondsForBeforePick;
    private float secondsForPickAttack;
    private float secondsForWaitingToUp;
    private bool isFinished_Outward = false;
    private Vector3 defaultPos;
    private float contactPointWithGround = 0.25f;
    private bool isChecked = false;
    [SerializeField] private BombEggA bombEggAPrefab;// BombEggBaseだとオブジェクトプールで不具合が出る
    [SerializeField] private BombEggB bombEggBPrefab;
    [SerializeField] private BombEggC bombEggCPrefab;
    [SerializeField] private Transform bombEggSpawnPos;
    private bool isBombEggAttack = false;
    private float elapsedTime_Egg = 0f;
    private float secondsForBeforeShoot;

    private void Start()
    {
        defaultPos = transform.localPosition;
        int defaultNum = 2;
        int maxNum = 3;
        
        if(gameObject.name.Contains("Center"))
        {
            MyObjectPool.Instance.CreatePool(bombEggAPrefab, defaultNum, maxNum);
        }
        else if(gameObject.name.Contains("Left"))
        {
            MyObjectPool.Instance.CreatePool(bombEggBPrefab, defaultNum, maxNum);
        }
        else
        {
            MyObjectPool.Instance.CreatePool(bombEggCPrefab, defaultNum, maxNum);
        }       
    }

    private void Update()
    {
        if (isPickAttack)
        {
            PickAttack();
        }
        if (isBombEggAttack)
        {
            BombEggAttack();
        }
    }

    public void PickInit(float beforeTime, float attackTime, float waitTime)
    {
        if(myRenderer.CurrentStatus != TsubameRenderer.Status.damaging)
        {
            myRenderer.StatusChange(TsubameRenderer.Status.before);
        }
        secondsForBeforePick = beforeTime;
        secondsForPickAttack = attackTime;
        secondsForWaitingToUp = waitTime;
        isFinished_Outward = false;
        isChecked = false;
        elapsedTime_Pick = 0f;
        isPickAttack = true;
    }

    private void PickAttack()
    {
        elapsedTime_Pick += Time.deltaTime;

        if (!isFinished_Outward) // 往路
        {
            // ダメージリアクション後、状態に合わせ色を変化
            if(elapsedTime_Pick < secondsForBeforePick && myRenderer.CurrentStatus != TsubameRenderer.Status.damaging && myRenderer.CurrentStatus != TsubameRenderer.Status.before)
            {
                myRenderer.StatusChange(TsubameRenderer.Status.before);
            }
            if (secondsForBeforePick <= elapsedTime_Pick && myRenderer.CurrentStatus != TsubameRenderer.Status.damaging && myRenderer.CurrentStatus != TsubameRenderer.Status.attacking)
            {
                myRenderer.StatusChange(TsubameRenderer.Status.attacking);
            }

            if (secondsForBeforePick <= elapsedTime_Pick && elapsedTime_Pick <= secondsForBeforePick + secondsForPickAttack)
            {
                if (!attackCollider.enabled)
                {
                    attackCollider.enabled = true;
                    startPos = transform.localPosition;
                    targetPos = transform.parent.InverseTransformPoint(new Vector3(transform.position.x, contactPointWithGround + transform.lossyScale.y * 0.5f, 0f));
                }

                transform.localPosition = Vector3.Lerp(startPos, targetPos, (elapsedTime_Pick - secondsForBeforePick) / secondsForPickAttack);
            }

            if(secondsForBeforePick + secondsForPickAttack < elapsedTime_Pick && !isChecked)
            {
                if (transform.localPosition != targetPos)
                {
                    transform.localPosition = targetPos;
                }
                isChecked = true;
            }

            if (secondsForBeforePick + secondsForPickAttack + secondsForWaitingToUp <= elapsedTime_Pick) // 復路へ切り替え
            {
                startPos = transform.localPosition;
                targetPos = defaultPos;
                elapsedTime_Pick = 0f;
                isFinished_Outward = true;
            }
        }
        else // 復路
        {
            if (elapsedTime_Pick <= secondsForPickAttack)
            {
                if (myRenderer.CurrentStatus != TsubameRenderer.Status.damaging && myRenderer.CurrentStatus != TsubameRenderer.Status.attacking)
                {
                    myRenderer.StatusChange(TsubameRenderer.Status.attacking);
                }
                transform.localPosition = Vector3.Lerp(startPos, targetPos, elapsedTime_Pick / secondsForPickAttack);
            }
            else
            {
                if(transform.localPosition != defaultPos)
                {
                    transform.localPosition = defaultPos;
                }
                myRenderer.StatusChange(TsubameRenderer.Status.normal);
                attackCollider.enabled = false;
                isPickAttack = false;
            }
        }
    }

    public void BombEggAttackInit(float time)
    {
        secondsForBeforeShoot = time;
        elapsedTime_Egg = 0f;
        isBombEggAttack = true;
    }

    private void BombEggAttack()
    {
        elapsedTime_Egg += Time.deltaTime;
        if(elapsedTime_Egg < secondsForBeforeShoot && myRenderer.CurrentStatus != TsubameRenderer.Status.damaging && myRenderer.CurrentStatus != TsubameRenderer.Status.before)
        {
            myRenderer.StatusChange(TsubameRenderer.Status.before);
        }
        if(secondsForBeforeShoot <= elapsedTime_Egg)
        {
            BombEggBase egg;
            if (gameObject.name.Contains("Center"))
            {
                egg = MyObjectPool.Instance.GetFromPool(MyObjectPool.KindOfPool.BombEggA, bombEggAPrefab);
            }
            else if (gameObject.name.Contains("Left"))
            {
                egg = MyObjectPool.Instance.GetFromPool(MyObjectPool.KindOfPool.BombEggB, bombEggBPrefab);
            }
            else
            {
                egg = MyObjectPool.Instance.GetFromPool(MyObjectPool.KindOfPool.BombEggC, bombEggCPrefab);
            }
            egg.transform.position = bombEggSpawnPos.position;
            egg.gameObject.SetActive(true);

            if (myRenderer.CurrentStatus != TsubameRenderer.Status.damaging)
            {
                myRenderer.StatusChange(TsubameRenderer.Status.normal);
            }
            isBombEggAttack = false;
        }
    }

    /// <summary>
    /// 1:予備動作、2:卵発射中、3:解除
    /// </summary>
    public void CarpetAttack(int num)
    {
        if (myRenderer.CurrentStatus == TsubameRenderer.Status.damaging) return;

        if(num == 1)
        {
            myRenderer.StatusChange(TsubameRenderer.Status.before);
        }
        if(num == 2)
        {
            myRenderer.StatusChange(TsubameRenderer.Status.attacking);
        }
        if(num == 3)
        {
            myRenderer.StatusChange(TsubameRenderer.Status.normal);
        }
    }
}
