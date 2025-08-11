
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[ExecuteInEditMode]
public class CustomWind : UdonSharpBehaviour
{
    [Header("General Parameters")]
    [Tooltip("Wind Speed in Kilometers per hour")]
    public float WindSpeed = 30;
    [Range(0.0f, 2.0f)]
    [Tooltip("Wind Turbulence in percentage of wind Speed")]
    public float Turbulence = 0.25f;


    [Header("Noise Parameters")]
    [Tooltip("Texture used for wind turbulence")]
    public Texture2D NoiseTexture;
    [Tooltip("Size of one world tiling patch of the Noise Texture, for bending trees")]
    public float FlexNoiseWorldSize = 175.0f;
    [Tooltip("Size of one world tiling patch of the Noise Texture, for leaf shivering")]
    public float ShiverNoiseWorldSize = 10.0f;

    [Header("Gust Parameters")]
    [Tooltip("Texture used for wind gusts")]
    public Texture2D GustMaskTexture;
    [Tooltip("Size of one world tiling patch of the Gust Texture, for leaf shivering")]
    public float GustWorldSize = 600.0f;

    [Tooltip("Wind Gust Speed in Kilometers per hour")]
    public float GustSpeed = 50;
    [Tooltip("Wind Gust Influence on trees")]
    public float GustScale = 1.0f;

    Vector4 pos1 = new Vector4();
    Vector4 pos2 = new Vector4();
    Vector4 pos3 = new Vector4();
    Vector4 pos4 = new Vector4();
    Vector4 radius = new Vector4();

    // Use this for initialization
    void Start()
    {
        ApplySettings();
    }

    // Update is called once per frame
    void Update()
    {
        ApplySettings();
    }

    void OnValidate()
    {
        ApplySettings();
    }

    void ApplySettings()
    {
        VRCShader.SetGlobalTexture(VRCShader.PropertyToID("_UdonWIND_SETTINGS_TexNoise"), NoiseTexture);
        VRCShader.SetGlobalTexture(VRCShader.PropertyToID("_UdonWIND_SETTINGS_TexGust"), GustMaskTexture);
        VRCShader.SetGlobalVector(VRCShader.PropertyToID("_UdonWIND_SETTINGS_WorldDirectionAndSpeed"), GetDirectionAndSpeed());
        VRCShader.SetGlobalFloat(VRCShader.PropertyToID("_UdonWIND_SETTINGS_FlexNoiseScale"), 1.0f / Mathf.Max(0.01f, FlexNoiseWorldSize));
        VRCShader.SetGlobalFloat(VRCShader.PropertyToID("_UdonWIND_SETTINGS_ShiverNoiseScale"), 1.0f / Mathf.Max(0.01f, ShiverNoiseWorldSize));
        VRCShader.SetGlobalFloat(VRCShader.PropertyToID("_UdonWIND_SETTINGS_Turbulence"), WindSpeed * Turbulence);
        VRCShader.SetGlobalFloat(VRCShader.PropertyToID("_UdonWIND_SETTINGS_GustSpeed"), GustSpeed);
        VRCShader.SetGlobalFloat(VRCShader.PropertyToID("_UdonWIND_SETTINGS_GustScale"), GustScale);
        VRCShader.SetGlobalFloat(VRCShader.PropertyToID("_UdonWIND_SETTINGS_GustWorldScale"), 1.0f / Mathf.Max(0.01f, GustWorldSize));



        pos1 = new Vector4(0, 0, 0, 0);
        radius[0] = 0.1f;

        pos2 = new Vector4(0, 0, 0, 0);
        radius[1] = 0.1f;
       
        pos3 = new Vector4(0, 0, 0, 0);
        radius[2] = 0.1f;
       
        pos4 = new Vector4(0, 0, 0, 0);
        radius[3] = 0.1f;



        VRCShader.SetGlobalMatrix(VRCShader.PropertyToID("_UdonWIND_SETTINGS_Points"), new Matrix4x4(pos1, pos2, pos3, pos4));
        VRCShader.SetGlobalVector(VRCShader.PropertyToID("_UdonWIND_SETTINGS_Points_Radius"), radius);


    }

    Vector4 GetDirectionAndSpeed()
    {
        Vector3 dir = transform.forward.normalized;
        return new Vector4(dir.x, dir.y, dir.z, WindSpeed * 0.2777f);
    }
}
