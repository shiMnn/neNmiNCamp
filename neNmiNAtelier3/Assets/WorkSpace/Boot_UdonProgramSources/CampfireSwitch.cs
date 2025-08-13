
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CampfireSwitch : UdonSharpBehaviour
{
    [SerializeField ]private ParticleSystem[] _particle = null;
    private bool _display = true;
    [SerializeField] private float _lightRange = 3.5f;
    [SerializeField] private Light _light = null;

    [SerializeField] GameObject _se = null;

    // 遷移時間
    private int _counterMax = 720;
    bool _processing = false;
    int _counter = 0;

    public void Update()
    {
        if (_processing)
        {
            float fValue = 0.0f;
            if (_display)
            {// 表示
                fValue = Mathf.Lerp(0.0f, _lightRange, (float)((float)_counter / (float)_counterMax));
            }
            else
            {// 非表示
                fValue = Mathf.Lerp(_lightRange, 0.0f, (float)((float)_counter / (float)_counterMax));
            }
            _counter++;

            if(_light != null)
            {
                _light.range = fValue;
            }
            if(_counter >= _counterMax)
            {
                _processing = false;
            }
        }
    }

    public override void Interact()
    {
        if (!_processing)
        {
            _display = !_display;
            _processing = true;
            _counter = 0;
       
            foreach (var p in _particle)
            {
                var emission = p.emission;
                emission.enabled = _display;
            }

            if(_se!= null)
            {
                _se.SetActive(_display);
            }
        }
    }
}
