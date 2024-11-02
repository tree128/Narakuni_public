using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpetBombEgg : BombEggBase
{
    [SerializeField] private CarpetBombEggData carpetData;
    private bool isGroundBomb = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (Random.value * 100 <= carpetData.Rate)
        {
            isGroundBomb = true;
        }
        else
        {
            isGroundBomb = false;
        }
        groundVector.y = -carpetData.FirstSpeed_Ground;
    }

    private void FixedUpdate()
    {
        if (isGroundBomb)
        {
            GroundBomb();
        }
        else
        {
            WaterBomb();
        }
    }

    private void Fall()
    {
        rb.velocity = groundVector;
        groundVector.y = -Mathf.Clamp(Mathf.Abs(groundVector.y) + carpetData.Acceleration, carpetData.FirstSpeed_Ground, carpetData.MaxSpeed);
    }

    private void GroundBomb()
    {
        elapsedTime += Time.deltaTime;
        if (carpetData.GroundBombYpos < transform.position.y)
        {
            Fall();
            if (carpetData.SecondsForStartingToBlink_Ground <= elapsedTime)
            {
                // “_–Å
                elapsedTime_Blink += Time.deltaTime;
                if (elapsedTime_Blink < bombData.SecondsForSecondBlink && myRenderer.enabled)
                {
                    myRenderer.enabled = false;
                }
                if (bombData.SecondsForSecondBlink <= elapsedTime_Blink && !myRenderer.enabled)
                {
                    myRenderer.enabled = true;
                }
                if (bombData.SecondsForSecondBlink + bombData.SecondsForSecondDefault <= elapsedTime_Blink)
                {
                    elapsedTime_Blink = 0f;
                }
            }
        }
        else
        {
            if (!hasExploded)
            {
                Explode();
            }
            else
            {
                FadeOut();
            }
        }
    }

    protected override void Fall_Water()
    {
        rb.velocity = waterVector;
        if (bombData.SecondsForWaterDeceleration <= elapsedTime)
        {
            waterVector.y = -Mathf.Clamp(Mathf.Abs(waterVector.y) - decelerationInWater, 0f, bombData.WaterFirstSpeedY);
        }
    }

    private void WaterBomb()
    {
        if (!isImpacted)
        {
            if (bombData.ImpactYPos < transform.position.y)
            {
                Fall();
            }
            else
            {
                isImpacted = true;
            }
        }
        else
        {
            if(transform.position.y < bombData.FallWaterYPos)
            {
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
        }
    }

    protected override void ReleaseToPool()
    {
        var d = this;
        MyObjectPool.Instance.ReleaseToPool(MyObjectPool.KindOfPool.CarpetBomb, d);
    }
}
