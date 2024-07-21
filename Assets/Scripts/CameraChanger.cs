using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カメラのモードを設定
[HideInInspector,Serializable]
public enum CameraModeType
{
    Default,
    Sitting,
    CampFire,
}

/// <summary>
/// カメラのモードを変更するクラス
/// </summary>
public class CameraChanger : MonoBehaviour
{
    [SerializeField]
    private CameraModeType cameraType;
    [SerializeField]
    private CameraManager cameraManager;
    // 初期状態のカメラパラメータ
    private CameraManager.Parameter defaultParameter;
    // 座っているキャラクターのカメラパラメータ
    [SerializeField]
    private CameraManager.Parameter sittingParameter;
    // 焚火付近のキャラクターのカメラパラメータ
    [SerializeField]
    private CameraManager.Parameter campFireParameter;
    // 変更回数のカウンター
    public int changeCount = 0;
    private int maxCount = 0;

    private Sequence cameraSequence;

    public static CameraChanger instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        // デフォルトカメラのパラメータを設定
        defaultParameter = cameraManager.Param.Clone();
        maxCount = Enum.GetValues(typeof(CameraModeType)).Length;
    }

    // Update is called once per frame
    void Update()
    {
        if(changeCount >= maxCount)
        {
            changeCount = 0;
        }
        
        if(changeCount == 0)
        {
            SwitchCamera(CameraModeType.Default);
        }
        else if(changeCount == 1)
        {
            SwitchCamera(CameraModeType.Sitting);
        }
        else if (changeCount == 2)
        {
            SwitchCamera(CameraModeType.CampFire);
        }
    }

    /// <summary>
    /// カメラのモードを切り替える
    /// </summary>
    /// <param name="type">カメラモード</param>
    public void SwitchCamera(CameraModeType type)
    {
        // カメラの切り替え間隔
        float duration = 2.0f;

        // カメラモードごとにパラメータを設定
        switch (type)
        {
            case CameraModeType.Default:
                defaultParameter.position = defaultParameter.trackTarget.position;
                break;
            case CameraModeType.Sitting:
                sittingParameter.position = sittingParameter.trackTarget.position;
                sittingParameter.angles = cameraManager.Param.angles;
                transform.eulerAngles = new Vector3(0.0f, cameraManager.Param.angles.y, 0.0f);
                break;
            case CameraModeType.CampFire:
                campFireParameter.position = campFireParameter.trackTarget.position;
                campFireParameter.angles = cameraManager.Param.angles;
                transform.eulerAngles = new Vector3(0.0f, cameraManager.Param.angles.y, 0.0f);
                break;
        }

        cameraManager.Param.trackTarget = null;
        CameraManager.Parameter startCameraParam = cameraManager.Param.Clone();
        CameraManager.Parameter endCameraParam = GetCameraParameter(type);

        // カメラの遷移アニメーション
        cameraSequence?.Kill();
        // Sequenceを生成
        cameraSequence = DOTween.Sequence();
        // tweenをつなげる
        cameraSequence.Append(
            DOTween.To(() => 0.0f,
            t => CameraManager.Parameter.Lerp(startCameraParam, endCameraParam, t, cameraManager.Param),
            1.0f, duration).SetEase(Ease.OutQuart));

        switch (type)
        {
            case CameraModeType.Default:
                cameraSequence.OnUpdate(() => CameraManager.UpdateTrackTargetBlend(defaultParameter));
                break;
            case CameraModeType.Sitting:
                cameraSequence.OnUpdate(() => sittingParameter.position = sittingParameter.trackTarget.position);
                break;
            case CameraModeType.CampFire:
                cameraSequence.OnUpdate(() => campFireParameter.position = campFireParameter.trackTarget.position);
                break;
        }

        cameraSequence.AppendCallback(() => cameraManager.Param.trackTarget = endCameraParam.trackTarget);
    }

    /// <summary>
    /// モードごとのカメラのパラメータを取得
    /// </summary>
    /// <param name="type">カメラモード</param>
    /// <returns>モードに応じたカメラパラメータ</returns>
    private CameraManager.Parameter GetCameraParameter(CameraModeType type)
    {
        switch (type)
        {
            case CameraModeType.Default:
                return defaultParameter;
            case CameraModeType.Sitting:
                return sittingParameter;
            case CameraModeType.CampFire:
                return campFireParameter;
            default:
                return null;
        }
    }
}
