using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEggC : BombEggBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        float offset = Random.Range(Mathf.Min(bombData.ImpactXPos, bombData.ImpactXPosAnother), Mathf.Max(bombData.ImpactXPos, bombData.ImpactXPosAnother));
        float playerXPos = Mathf.Clamp(playerData.PlayerTransform.position.x + offset, transform.position.x + bombData.ImpactMinDistance, bombData.ImpactBlocker_Right.position.x);
        groundVector.x = (playerXPos - transform.position.x) / bombData.SpeedX_Ground;
    }

    private void FixedUpdate()
    {
        if (!isImpacted)
        {
            if (bombData.ImpactYPos < transform.position.y)
            {
                //Debug.Log("”­ŽË");
                rb.velocity = groundVector;
                transform.rotation = Quaternion.AngleAxis(bombData.ShootRotationSpeed, Vector3.forward) * transform.rotation;
                groundVector.y -= bombData.ShootSpeedYDeceleration;
            }
            else
            {
                //Debug.Log("’…’e");
                float offset = Random.Range(Mathf.Min(bombData.FallWaterXPos, bombData.FallWaterXPosAnother), Mathf.Max(bombData.FallWaterXPos, bombData.FallWaterXPosAnother));
                float playerXPos = Mathf.Clamp(playerData.PlayerTransform.position.x - offset, transform.position.x - bombData.FallWaterXPosLong, transform.position.x - bombData.FallWaterXPosShort);
                groundVector.x = (playerXPos - transform.position.x) / bombData.SpeedX_Rebound;

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
                transform.rotation = Quaternion.AngleAxis(bombData.ReboundRotationSpeed, Vector3.forward) * transform.rotation;
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
        var c = this;
        MyObjectPool.Instance.ReleaseToPool(MyObjectPool.KindOfPool.BombEggC, c);
    }
}
