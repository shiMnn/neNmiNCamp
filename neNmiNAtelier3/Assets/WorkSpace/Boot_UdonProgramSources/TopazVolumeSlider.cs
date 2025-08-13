
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TopazVolumeSlider : UdonSharpBehaviour
{
    [SerializeField] private UnityEngine.UI.Slider _slider;
    [SerializeField] private AudioSource[] _audiosource = null;

    private void Start()
    {
        OnSliderChanged();
    }

    public void OnSliderChanged()
    {
        Debug.Log("hgr");
        if (_slider == null) return;
        float fVolume = _slider.value;
        if (_audiosource != null)
        {
            Debug.Log("hgrhgr");
            foreach (var audioObj in _audiosource)
            {
                Debug.Log($"{fVolume}");
                audioObj.volume = fVolume;
            }
        }
    }
}
