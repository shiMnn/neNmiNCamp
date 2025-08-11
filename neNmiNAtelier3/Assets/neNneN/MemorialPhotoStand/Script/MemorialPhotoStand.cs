using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;
using Newtonsoft.Json.Linq;
using VRC.Udon.Common.Interfaces;

enum State
{
    Display,         // 表示中
    Switching,       // 切替実行中
}

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class MemorialPhotoStand : UdonSharpBehaviour
{
    [Header("▼触らない")]
    public Renderer[] _renderer = null;
    [Header("▼表示したい画像をドラッグして設定して下さい")]
    public Texture[] _textures = null;
    [Header("▼画像を表示する時間")]
    public float _displaySeconds = 8.0f;
    [Header("▼画像が切り替わるのにかかる時間")]
    public float _fadeDuration = 2.0f;
    
    private float _fadeTimer = 0.0f;
    private float _switchTimer = 0.0f;

    State _currentState = State.Display;
    [UdonSynced, FieldChangeCallback(nameof(CurrentTextureIndex))]
    private int _currentTextureIndex = 0;
    public int CurrentTextureIndex
    {
        get
        {
            return _currentTextureIndex;
        }
        set
        {
            _currentTextureIndex = value;

            _currentState = State.Switching;
            _fadeTimer = 0.0f;
        }
    }

    private void Start()
    {
        if(_renderer != null && _renderer.Length >= 2)
        {
            _renderer[0].gameObject.SetActive(true);
            _renderer[1].gameObject.SetActive(true);

            _renderer[0].material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            _renderer[1].material.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }

        SwitchTexture();
    }

    private void Update() 
    {
        if(_renderer == null || _renderer.Length <= 0) { return; }
        if(_textures == null || _textures.Length <= 0) { return; }


        switch (_currentState)
        {
            case State.Display:
                {// 表示中
                    if (Networking.IsOwner(this.gameObject))
                    {// 同期ホスト
                        if (UpdateSwitch())
                        {// 切替タイミング到達
                            UpdateTextureIndex();
                        }
                    }
                }
                break;
            case State.Switching:
                {// フェード処理
                    UpdateFade();
                }
                break;
        }
    }


    /// <summary>
    /// 画像切替の更新
    /// </summary>
    /// <returns>trueで切替タイミング</returns>
    bool UpdateSwitch()
    {
        if (_textures.Length >= 2)
        {
            _switchTimer += Time.deltaTime;

            if (_switchTimer > _displaySeconds)
            {
                Debug.Log($"[Memorial Photo Stand] Switch photo.");
                _switchTimer = 0.0f;
                return true;
            }
        }

        return false;
    }

    void UpdateFade()
    {
        _fadeTimer += Time.deltaTime;
        
        if (_fadeTimer > _fadeDuration)
        {
            _renderer[0].material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            _renderer[1].material.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

            this.SwitchTexture();
            _currentState = State.Display;
        }
        else
        {
            float fValue = _fadeTimer / _fadeDuration;
            
            Mathf.Lerp(0.0f, 1.0f, fValue);

            _renderer[0].material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f - fValue);
            _renderer[1].material.color = new Color(1.0f, 1.0f, 1.0f, fValue);
        }
    }

    /// <summary>
    /// 表示する画像のINDEX更新
    /// </summary>
    void UpdateTextureIndex()
    {
        int nIndex = CurrentTextureIndex + 1;
        CurrentTextureIndex = (nIndex >= _textures.Length) ? 0 : nIndex;

        // 同期要求
        RequestSerialization();
    }

    /// <summary>
    /// 画像の更新
    /// </summary>
    void SwitchTexture()
    {
        int nIndex = CurrentTextureIndex;
        int nNextIndex = CurrentTextureIndex + 1;
        nNextIndex = (nNextIndex >= _textures.Length) ? 0 : nNextIndex;


        var texture = _textures[nIndex];
        var nextTexture = _textures[nNextIndex];

        _renderer[0].material.mainTexture = texture;
        _renderer[1].material.mainTexture = nextTexture;
    }
}
