using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

/// <summary>
/// ����ʍs���B
/// ��{��Trigger��ԂŁA�v���C���[�̕��������ʒu�ɂ��鎞����Collider�ɂȂ�
/// </summary>
public class OwBlock : MonoBehaviour
{
    [SerializeField] private BoxCollider2D myCollider;
    private BoxCollider2D playerCollider;
    private float myTopPos;
    private bool isVisible = false;
    //[SerializeField] private PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        myTopPos = transform.position.y + transform.localScale.y * 0.5f;
        myCollider.enabled = false;
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isVisible) return;
        Profiler.BeginSample("�I�I�I�^�[�Q�b�g�I�I�I");
        /*if (myTopPos >= playerData.PlayerCollider.transform.position.y + playerData.PlayerCollider.offset.y - playerData.PlayerCollider.size.y * 0.5f && !myCollider.isTrigger)
        {
            myCollider.isTrigger = true;
        }

        if (myTopPos < playerData.PlayerCollider.transform.position.y + playerData.PlayerCollider.offset.y - playerData.PlayerCollider.size.y * 0.5f && myCollider.isTrigger)
        {
            myCollider.isTrigger = false;
        }*/
        if (myTopPos >= playerCollider.transform.position.y + playerCollider.offset.y - playerCollider.size.y * 0.5f && !myCollider.isTrigger)
        {
            myCollider.isTrigger = true;
        }

        if (myTopPos < playerCollider.transform.position.y + playerCollider.offset.y - playerCollider.size.y * 0.5f && myCollider.isTrigger)
        {
            myCollider.isTrigger = false;
        }
        Profiler.EndSample();
    }

    private void OnBecameVisible()
    {
        isVisible = true;
        if (!myCollider.enabled)
        {
            myCollider.enabled = true;
        }
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
        if (myCollider.enabled)
        {
            myCollider.enabled = false;
        }
    }
}
