using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEggBase : MonoBehaviour
{
    [SerializeField] protected PlayerData playerData;
    [SerializeField] protected BombEggData bombData;
    [SerializeField] protected SpriteRenderer myRenderer;
    [SerializeField] protected CircleCollider2D myCollider;
    [SerializeField] protected Rigidbody2D rb;
    protected Vector3 groundVector = new Vector3();
    protected bool isImpacted = false;
    protected bool isRebounded = false;
    protected bool inWater = false;
    protected Vector3 waterVector = new Vector3();
    protected float decelerationInWater;
    protected Color bombColor;
    protected float elapsedTime_Blink = 0f;
    protected int blinkCount = 0;
    protected bool hasExploded = false;
    protected float elapsedTime = 0f;
    
    protected virtual void OnEnable()
    {
        elapsedTime = 0f;
        elapsedTime_Blink = 0f;
        isImpacted = false;
        isRebounded = false;
        inWater = false;
        hasExploded = false;
        bombColor = bombData.BombColor;
        groundVector.y = bombData.ShootFirstSpeedY;
        waterVector.y = -bombData.WaterFirstSpeedY;
        transform.localScale = Vector3.one * bombData.DefaultScale;
        transform.eulerAngles = Vector3.zero;
        myCollider.radius = bombData.DefaultRadius;
        myCollider.enabled = true;
        myRenderer.sprite = bombData.EggSprite;
        myRenderer.color = bombData.DefaultColor;
        decelerationInWater = Random.Range(Mathf.Min(bombData.WaterSpeedYDeceleration, bombData.WaterSpeedYDecelerationAnother), Mathf.Max(bombData.WaterSpeedYDeceleration, bombData.WaterSpeedYDecelerationAnother));
    }

    protected virtual void Fall_Water()
    {
        if (!inWater)
        {
            waterVector.x = groundVector.x * bombData.WaterSpeedXRate * 0.01f;
            inWater = true;
        }
        rb.velocity = waterVector;
        transform.rotation = Quaternion.AngleAxis(bombData.WaterRotationSpeed * Mathf.Sign(-groundVector.x), Vector3.forward) * transform.rotation;
        waterVector.x = Mathf.Clamp(Mathf.Abs(waterVector.x) - bombData.WaterSpeedXDeceleration, 0f, bombData.SpeedX_Ground) * Mathf.Sign(groundVector.x);
        if (bombData.SecondsForWaterDeceleration <= elapsedTime)
        {
            waterVector.y = -Mathf.Clamp(Mathf.Abs(waterVector.y) - decelerationInWater, 0f, bombData.WaterFirstSpeedY);
        }
    }

    protected virtual void Blink()
    {
        elapsedTime_Blink += Time.deltaTime;
        if (blinkCount < bombData.BlinkNum)
        {
            if (elapsedTime_Blink < bombData.SecondsForFirstBlink && myRenderer.enabled)
            {
                myRenderer.enabled = false;
            }
            if (bombData.SecondsForFirstBlink <= elapsedTime_Blink && !myRenderer.enabled)
            {
                myRenderer.enabled = true;
            }
            if (bombData.SecondsForFirstBlink + bombData.SecondsForFirstDefault <= elapsedTime_Blink)
            {
                elapsedTime_Blink = 0f;
                blinkCount++;
            }
        }
        else
        {
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

    protected virtual void Explode()
    {
        transform.localScale = new Vector3(bombData.EffectSize, bombData.EffectSize, bombData.EffectSize);
        myRenderer.sprite = bombData.BombSprite;
        myRenderer.color = bombColor;
        if (!myRenderer.enabled)
        {
            myRenderer.enabled = true;
        }
        myCollider.radius = bombData.BombRadius;
        elapsedTime = 0f;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        hasExploded = true;
    }

    protected virtual void FadeOut()
    {
        if (bombData.SecondsForKeepingDamage <= elapsedTime && myCollider.enabled)
        {
            //ダメージ終了
            myCollider.enabled = false;
            elapsedTime = 0f;
        }

        if (!myCollider.enabled)
        {
            bombColor.a = Mathf.Lerp(1f, 0f, elapsedTime / bombData.SecondsForFadeOut);
            myRenderer.color = bombColor;
            if (bombColor.a == 0f)
            {
                ReleaseToPool();
            }
        }
    }

    protected virtual void ReleaseToPool()
    {
        Debug.LogError("ReleaseToPool()は処理をoverrideしてください");
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Wall"))
        {
            groundVector.x *= -1f;
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !playerData.Player.IsPlayerInvincible)
        {
            GameObject tsubame = GameObject.Find("Tsubame");
            if(tsubame != null)
            {
                tsubame.GetComponent<Tsubame>().DamageToPlayer(transform.position, 1);
            }
        }
    }
}
