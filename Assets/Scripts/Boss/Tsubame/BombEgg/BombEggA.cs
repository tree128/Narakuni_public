using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEggA : BombEggBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        float direction = Mathf.Sign(playerData.PlayerTransform.position.x - transform.position.x);
        float offset = Random.Range(Mathf.Min(bombData.ImpactXPos_A, bombData.ImpactXPosAnother_A), Mathf.Max(bombData.ImpactXPos_A, bombData.ImpactXPosAnother_A));
        groundVector.x = Mathf.Clamp(Mathf.Abs(playerData.PlayerTransform.position.x + offset - transform.position.x), 0f, bombData.ImpactMaxDistance_A) / bombData.SpeedX_Ground;
        groundVector.x *= direction;
    }

    private void FixedUpdate()
    {
        if (!isImpacted)
        {
            if(bombData.ImpactYPos < transform.position.y)
            {
                //Debug.Log("”­ŽË");
                rb.velocity = groundVector;
                transform.rotation = Quaternion.AngleAxis(bombData.ShootRotationSpeed * Mathf.Sign(-groundVector.x), Vector3.forward) * transform.rotation;
                groundVector.y -= bombData.ShootSpeedYDeceleration;
            }
            else
            {
                //Debug.Log("’…’e");
                groundVector.y = bombData.ReboundFirstSpeed;
                rb.velocity = groundVector;
                isImpacted = true;
            }
        }
        else
        {
            if (isRebounded && transform.position.y <= bombData.FallWaterYPos)
            {
                //Debug.Log("—Ž…");
                elapsedTime += Time.deltaTime;
                if (!hasExploded)
                {
                    Fall_Water();
                    if (bombData.SecondsForStartingToBlink <= elapsedTime)
                    {
                        Blink();
                        if (bombData.SecondsForStartingToBlink + bombData.SecondsForBomb <= elapsedTime)
                        {
                            Explode();
                        }
                    }
                }
                else
                {
                    FadeOut();
                }
            }
            else
            {
                //Debug.Log("’µ’e");
                rb.velocity = groundVector;
                transform.rotation = Quaternion.AngleAxis(bombData.ReboundRotationSpeed * Mathf.Sign(-groundVector.x), Vector3.forward) * transform.rotation;
                groundVector.y -= bombData.ReboundSpeedYDeceleration;
                if (bombData.ImpactYPos < transform.position.y && !isRebounded)
                {
                    isRebounded = true;
                }
            }
        }
    }

    protected override void ReleaseToPool()
    {
        var a = this;
        MyObjectPool.Instance.ReleaseToPool(MyObjectPool.KindOfPool.BombEggA, a);
    }
}
