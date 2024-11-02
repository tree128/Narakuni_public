using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;
    public static CameraManager Instance => instance;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CinemachineConfiner2D confiner;
    [SerializeField] private CompositeCollider2D compositeCollider;
    [SerializeField] private GameObject blocker_Ground;
    [SerializeField] private GameObject blocker_Water;
    [SerializeField] private bool useOnGround;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            confiner.InvalidateCache();
        }
    }

    private void OnValidate()
    {
        if (useOnGround && !blocker_Ground.activeInHierarchy)
        {
            blocker_Ground.SetActive(true);
            blocker_Water.SetActive(false);
            confiner.InvalidateCache();
        }
        if (!useOnGround && !blocker_Water.activeInHierarchy)
        {
            blocker_Ground.SetActive(false);
            blocker_Water.SetActive(true);
            confiner.InvalidateCache();
        }
    }
    public void ChangeCameraVersion()
    {
        useOnGround = !useOnGround;
        if (useOnGround)
        {
            if (!blocker_Ground.gameObject.activeInHierarchy)
            {
                blocker_Ground.gameObject.SetActive(true);
            }
            if (blocker_Water.gameObject.activeInHierarchy)
            {
                blocker_Water.gameObject.SetActive(false);
            }
        }
        else
        {
            if (blocker_Ground.gameObject.activeInHierarchy)
            {
                blocker_Ground.gameObject.SetActive(false);
            }
            if (!blocker_Water.gameObject.activeInHierarchy)
            {
                blocker_Water.gameObject.SetActive(true);
            }
        }
        confiner.InvalidateCache();
    }
}
