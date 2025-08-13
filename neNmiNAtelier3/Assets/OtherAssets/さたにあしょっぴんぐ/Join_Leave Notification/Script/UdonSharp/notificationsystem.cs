
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class notificationsystem : UdonSharpBehaviour
{
    public AudioSource join_sound;
    public AudioSource leave_sound;
    public Text textmesh;
    public Image join_icon;
    public Image leave_icon;
    public Canvas canv;
    public Outline outline;
    public float speed = 0.01f;
    public string playerjoin = "Notification";
    public string joined = "が参加しました。";
    public string leave = "が退出しました。";
    private float Damping = 1.0f;

    Image icon;

    float Opacity = 0.0f;
    bool toggle = false;
    bool toggle2 = false;

    void Start()
    {
        canv.gameObject.SetActive(false);
        toggle = false;
        toggle2 = false;
        Opacity = 0.0f;
        setOpacity(Opacity);
        join_icon.gameObject.SetActive(false);
        leave_icon.gameObject.SetActive(false);
    }
    private void setOpacity(float opacity)
    {
        setTextOpacity(opacity);
        setJoinIconOpacity(opacity);
        setLeaveIconOpacity(opacity);
    }
    private void setTextOpacity(float opacity)
    {
        textmesh.material.color = new Color(textmesh.material.color.r, textmesh.material.color.g, textmesh.material.color.b, opacity);
    }
    private void setIconOpacity(float opacity)
    {
        if (icon == null)
            return;

        icon.material.color = new Color(icon.material.color.r, icon.material.color.g, icon.material.color.b, opacity);
    }
    private void setJoinIconOpacity(float opacity)
    {
        if (join_icon == null)
            return;

        join_icon.material.color = new Color(join_icon.material.color.r, join_icon.material.color.g, join_icon.material.color.b, opacity);
    }
    private void setLeaveIconOpacity(float opacity)
    {
        if (leave_icon == null)
            return;

        leave_icon.material.color = new Color(leave_icon.material.color.r, leave_icon.material.color.g, leave_icon.material.color.b, opacity);
    }
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        join_sound.Play();
        string playername = player.displayName;

        join_icon.gameObject.SetActive(true);
        leave_icon.gameObject.SetActive(false);

        startAnimation(join_icon, $"{playerjoin}\n  {playername} {joined}");
    }
    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        leave_sound.Play();
        string playername = player.displayName;

        join_icon.gameObject.SetActive(false);
        leave_icon.gameObject.SetActive(true);

        startAnimation(leave_icon, $"{playerjoin}\n  {playername} {leave}");
    }
    private void startAnimation(Image image, string msg)
    {
        textmesh.text = msg;
        icon = image;
        toggle = true;
        toggle2 = false;
        Opacity = 0.0f;
        canv.gameObject.SetActive(true);
    }

    public void Update()
    {
        if (Networking.LocalPlayer == null || !Networking.LocalPlayer.IsValid())
        {
            return;
        }

        var head = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
        if (Networking.LocalPlayer.IsUserInVR())
        {
            this.transform.position = Vector3.Lerp(this.transform.position, head.position, this.Damping * Time.deltaTime);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, head.rotation, this.Damping * Time.deltaTime);
        }
        else
        {
            this.transform.position = head.position;
            this.transform.rotation = head.rotation;
        }

        if (toggle)
        {
            Opacity = Opacity + speed;
            if (textmesh.material != null)
            {
                setTextOpacity(Opacity);
                setIconOpacity(Opacity);

                if (Opacity >= 1.0f)
                {
                    Opacity = 1.0f;

                    setTextOpacity(1.0f);
                    setIconOpacity(1.0f);

                    toggle = false;
                    toggle2 = true;
                }
            }
        }

        if (toggle2)
        {
            Opacity = Opacity - speed;
            if (textmesh.material != null)
            {
                setTextOpacity(Opacity);
                setIconOpacity(Opacity);

                if (Opacity <= 0.0f)
                {
                    Opacity = 0.0f;

                    setTextOpacity(0.0f);
                    setIconOpacity(0.0f);

                    toggle = false;
                    toggle2 = true;
                    canv.gameObject.SetActive(false);
                    icon.gameObject.SetActive(false);
                }
            }
        }
    }
}
