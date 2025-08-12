
using System.Runtime.Remoting;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LightSwitch : UdonSharpBehaviour
{
    [SerializeField] private GameObject[] _pps = null;
    private int _ppsIndex = 0;


    public override void Interact()
    {
        Debug.Log("hgr");
        if (_pps != null && _pps.Length == 4)
        {
            foreach (var obj in _pps)
            {
                obj.SetActive(false);
            }

            ++_ppsIndex;
            if (_ppsIndex >= 5)
            {
                _ppsIndex = 0;
            }
            if (_ppsIndex == 4)
            {// 全部空
            }
            else
            {
                _pps[_ppsIndex].SetActive(true);
            }
        }
        }
    }
