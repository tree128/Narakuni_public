using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy003 : EnemyBase
{
    [SerializeField] private Enemy003Parameter param;
    [SerializeField] private Enemy003_Searcher searcher;
    private Vector3 speed;
    private bool isRush = false;
    private float rushSpeed;
    private bool isFreeze_Discover = false;
    private bool isFreeze_Crash = false;
    private float elapedSeconds = 0;
    private string willCrashWallName = "";

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        currentHp = param.HP;
        damage = param.Damage;
        knockBackParam = param.KnockBackParameters;
        speed = new Vector3(-transform.localScale.x * param.Speed, 0, 0);
        searcher.Init(param.SearchRadius);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (isFreeze_Discover || isFreeze_Crash)
        {
            Freeze();
        }
        else if (onGround)
        {
            if (isRush)
            {
                transform.position += new Vector3(rushSpeed, 0, 0) * Time.deltaTime;
                rushSpeed = Mathf.Clamp(rushSpeed * (1 + param.Acceleration), -param.MaxSpeed, param.MaxSpeed);
            }
            else
            {
                transform.position += speed * Time.deltaTime;
            }
        }
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.GetContact(0).normal.y >= 0.95f)
        {
            if (!onGround && willCrashWallName != "")
            {
                // 衝突するはずの壁の名前を取得する
                RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Sign(speed.x), 0), float.PositiveInfinity, EnvironmentLayer);
                willCrashWallName = hit.collider.name;
            }
            onGround = true;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        WallCheck(collision);
    }

    protected override void WallCheck(Collision2D collision)
    {
        base.WallCheck(collision);
        if (atWall)
        {
            if (wallDirection != 2)
            {
                transform.localScale = new Vector2(wallDirection, transform.localScale.y);
                speed = new Vector3(-transform.localScale.x * param.Speed, 0, 0);

            }
            else
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            }
            atWall = false;
            wallDirection = 0;
            if (collision.collider.name == willCrashWallName)
            {
                isFreeze_Crash = true;
                isRush = false;
            }
        }
    }

    public void Discover()
    {
        if (isRush) return;

        // プレイヤーを向く
        float direction = Mathf.Sign(transform.position.x - playerData.PlayerTransform.position.x);
        if (direction != transform.localScale.x)
        {
            transform.localScale = new Vector2(direction, transform.localScale.y);
        }
        // 硬直
        isFreeze_Discover = true;
    }

    private void Freeze()
    {
        if (isFreeze_Discover)
        {
            elapedSeconds += Time.deltaTime;
            if (elapedSeconds >= param.FreezeTime_Discover)
            {
                // 衝突するはずの壁の名前を取得する
                RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(-transform.localScale.x, 0), float.PositiveInfinity, EnvironmentLayer);
                willCrashWallName = hit.collider.name;
                transform.position += new Vector3(Mathf.Sign(transform.position.x - hit.transform.position.x) * 0.01f, 0f);

                elapedSeconds = 0;
                rushSpeed = param.Speed * -transform.localScale.x;
                isRush = true;
                isFreeze_Discover = false;
            }
        }

        if (isFreeze_Crash)
        {
            elapedSeconds += Time.deltaTime;
            if (elapedSeconds >= param.FreezeTime_Crash)
            {
                searcher.Init(param.SearchRadius);
                elapedSeconds = 0;
                willCrashWallName = "";
                isRush = false;
                isFreeze_Crash = false;
            }
        }
    }
}
