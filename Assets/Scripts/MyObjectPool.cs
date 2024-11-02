using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MyObjectPool : MonoBehaviour
{
    public static MyObjectPool Instance;
    private Dictionary<KindOfPool, object> poolDic = new Dictionary<KindOfPool, object>(); // object = �I�u�W�F�N�g�v�[��

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
    /// �I�u�W�F�N�g�v�[�����쐬����
    /// </summary>
    /// <param name="prefab">�g���܂킵�����v���n�u</param>
    /// <param name="defaultNum">�����쐬��</param>
    /// <param name="maxNum">�ő�X�g�b�N��</param>
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
    /// �����ɓo�^����i��ށF�v�[���j
    /// </summary>
    /// <param name="pool">�쐬�����I�u�W�F�N�g�v�[��</param>
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
    /// �I�u�W�F�N�g�v�[���ɃC���X�^���X��v������B
    /// </summary>
    /// <param name="kind">�v�[���̎��</param>
    /// <param name="request">�Ԃ�l�̌^</param>
    /// <returns></returns>
    public T GetFromPool<T>(KindOfPool kind, T request) where T : MonoBehaviour
    {
        ObjectPool<T> pool = poolDic[kind] as ObjectPool<T>;
        return pool.Get();
    }

    /// <summary>
    /// �I�u�W�F�N�g�v�[���ɃC���X�^���X��ԋp����B
    /// </summary>
    /// <param name="kind">�v�[���̎��</param>
    /// <param name="obj">�ԋp����C���X�^���X</param>
    public void ReleaseToPool<T>(KindOfPool kind, T obj) where T : MonoBehaviour
    {
        ObjectPool<T> pool = poolDic[kind] as ObjectPool<T>;
        pool.Release(obj);
    }

    /// <summary>
    /// �V�����C���X�^���X���쐬����
    /// </summary>
    private T OnCreate<T>(T prefab) where T : MonoBehaviour
    {
        var obj = Instantiate(prefab, transform);
        obj.gameObject.SetActive(false);
        return obj;
    }

    /// <summary>
    /// �v�[������C���X�^���X�����o�������̏���
    /// </summary>
    private void OnGet<T>(T objInstance) where T : MonoBehaviour
    {
        
    }

    /// <summary>
    /// �C���X�^���X���v�[���ɖ߂������̏���
    /// </summary>
    private void OnRelease<T>(T objInstance) where T : MonoBehaviour
    {
        objInstance.gameObject.SetActive(false);
    }

    /// <summary>
    /// �C���X�^���X�����ɍő�l�܂ŃX�g�b�N����Ă��ĕԋp�o�����폜����ۂ̏���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objInstance"></param>
    private void MyOnDestroy<T>(T objInstance) where T : MonoBehaviour
    {
        Destroy(objInstance.gameObject);
    }
}
