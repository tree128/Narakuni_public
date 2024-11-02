using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MyObjectPool : MonoBehaviour
{
    public static MyObjectPool Instance;
    private Dictionary<KindOfPool, object> poolDic = new Dictionary<KindOfPool, object>(); // object = オブジェクトプール

    public enum KindOfPool
    {
        BombEggA,
        BombEggB,
        BombEggC,
        CarpetBomb
    }
    
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// オブジェクトプールを作成する
    /// </summary>
    /// <param name="prefab">使いまわしたいプレハブ</param>
    /// <param name="defaultNum">初期作成数</param>
    /// <param name="maxNum">最大ストック数</param>
    public void CreatePool<T>(T prefab, int defaultNum, int maxNum) where T: MonoBehaviour
    {
        ObjectPool<T> pool = new ObjectPool<T>(
                createFunc: () => OnCreate(prefab),
                actionOnGet: (a) => OnGet(a),
                actionOnRelease: (a) => OnRelease(a),
                actionOnDestroy: (a) => MyOnDestroy(a),
                collectionCheck: true,
                defaultCapacity: defaultNum,
                maxSize: maxNum
                );
        SetDictionary(pool);

        T[] array = new T[defaultNum];
        for (int i = 0; i < defaultNum; i++)
        {
            array[i] = pool.Get();
        }

        foreach (var item in array)
        {
            pool.Release(item);
        }
    }

    /// <summary>
    /// 辞書に登録する（種類：プール）
    /// </summary>
    /// <param name="pool">作成したオブジェクトプール</param>
    private void SetDictionary<T>(ObjectPool<T> pool) where T : MonoBehaviour
    {
        var obj = pool.Get();
        switch (obj.GetType().ToString())
        {
            case "BombEggA":
                poolDic.Add(KindOfPool.BombEggA, pool);
                break;
            case "BombEggB":
                poolDic.Add(KindOfPool.BombEggB, pool);
                break;
            case "BombEggC":
                poolDic.Add(KindOfPool.BombEggC, pool);
                break;
            case "CarpetBombEgg":
                poolDic.Add(KindOfPool.CarpetBomb, pool);
                break;
            default:
                break;
        }
        pool.Release(obj);
    }

    /// <summary>
    /// オブジェクトプールにインスタンスを要求する。
    /// </summary>
    /// <param name="kind">プールの種類</param>
    /// <param name="request">返り値の型</param>
    /// <returns></returns>
    public T GetFromPool<T>(KindOfPool kind, T request) where T : MonoBehaviour
    {
        ObjectPool<T> pool = poolDic[kind] as ObjectPool<T>;
        return pool.Get();
    }

    /// <summary>
    /// オブジェクトプールにインスタンスを返却する。
    /// </summary>
    /// <param name="kind">プールの種類</param>
    /// <param name="obj">返却するインスタンス</param>
    public void ReleaseToPool<T>(KindOfPool kind, T obj) where T : MonoBehaviour
    {
        ObjectPool<T> pool = poolDic[kind] as ObjectPool<T>;
        pool.Release(obj);
    }

    /// <summary>
    /// 新しいインスタンスを作成する
    /// </summary>
    private T OnCreate<T>(T prefab) where T : MonoBehaviour
    {
        var obj = Instantiate(prefab, transform);
        obj.gameObject.SetActive(false);
        return obj;
    }

    /// <summary>
    /// プールからインスタンスを取り出した時の処理
    /// </summary>
    private void OnGet<T>(T objInstance) where T : MonoBehaviour
    {
        
    }

    /// <summary>
    /// インスタンスをプールに戻した時の処理
    /// </summary>
    private void OnRelease<T>(T objInstance) where T : MonoBehaviour
    {
        objInstance.gameObject.SetActive(false);
    }

    /// <summary>
    /// インスタンスが既に最大値までストックされていて返却出来ず削除する際の処理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objInstance"></param>
    private void MyOnDestroy<T>(T objInstance) where T : MonoBehaviour
    {
        Destroy(objInstance.gameObject);
    }
}
