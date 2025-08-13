using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(Notification_System))]
public class NotificationSystem_UI : Editor
{
    GUIStyle style = GUIStyle.none;

    public override void OnInspectorGUI()
    {
        style.fontSize = 18;
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;

        GUILayout.Label("Join/Leave Notification System", style);

        Notification_System notificationsystem = target as Notification_System;

        notificationsystem.notify_udon = EditorGUILayout.ObjectField("Notification_System", notificationsystem.notify_udon, typeof(notificationsystem), true) as notificationsystem;

        if (notificationsystem.notify_udon != null)
        {
            var udonscript = notificationsystem.notify_udon;

            GUILayout.Space(5);

            style.fontSize = 12;
            GUILayout.Label("サウンド", style);

            GUILayout.Space(5);

            udonscript.join_sound = EditorGUILayout.ObjectField("入った際のサウンド", udonscript.join_sound, typeof(AudioSource), true) as AudioSource;
            if (udonscript.join_sound != null)
                udonscript.join_sound.clip = EditorGUILayout.ObjectField("サウンドファイル", udonscript.join_sound.clip, typeof(AudioClip), true) as AudioClip;

            EditorUtility.SetDirty(udonscript.join_sound);
            
            GUILayout.Space(5);
            udonscript.leave_sound = EditorGUILayout.ObjectField("抜けた際のサウンド", udonscript.leave_sound, typeof(AudioSource), true) as AudioSource;
            if (udonscript.leave_sound != null)
                udonscript.leave_sound.clip = EditorGUILayout.ObjectField("サウンドファイル", udonscript.leave_sound.clip, typeof(AudioClip), true) as AudioClip;

            EditorUtility.SetDirty(udonscript.leave_sound);

            GUILayout.Space(5);

            style.fontSize = 12;
            GUILayout.Label("キャンバス", style);

            GUILayout.Space(5);

            udonscript.canv = EditorGUILayout.ObjectField("キャンバス", udonscript.canv, typeof(Canvas), true) as Canvas;
            GUILayout.Space(5);

            EditorUtility.SetDirty(udonscript.canv);

            style.fontSize = 12;
            GUILayout.Label("スプライト", style);

            GUILayout.Space(5);

            udonscript.join_icon = EditorGUILayout.ObjectField("入った際のスプライト", udonscript.join_icon, typeof(Image), true) as Image;
            if (udonscript.join_icon != null)
                udonscript.join_icon.sprite = EditorGUILayout.ObjectField("アイコン", udonscript.join_icon.sprite, typeof(Sprite), true) as Sprite;

            EditorUtility.SetDirty(udonscript.join_icon);

            GUILayout.Space(5);

            udonscript.leave_icon = EditorGUILayout.ObjectField("抜けた際のスプライト", udonscript.leave_icon, typeof(Image), true) as Image;
            if (udonscript.leave_icon != null)
                udonscript.leave_icon.sprite = EditorGUILayout.ObjectField("アイコン", udonscript.leave_icon.sprite, typeof(Sprite), true) as Sprite;

            EditorUtility.SetDirty(udonscript.leave_icon);

            GUILayout.Space(5);

            style.fontSize = 12;
            GUILayout.Label("テキスト", style);

            GUILayout.Space(5);

            udonscript.speed = EditorGUILayout.FloatField("表示されるスピード", udonscript.speed);

            GUILayout.Space(5);

            udonscript.textmesh = EditorGUILayout.ObjectField("テキスト", udonscript.textmesh, typeof(Text), true) as Text;
            if (udonscript.textmesh != null)
            {
                udonscript.playerjoin = EditorGUILayout.TextField("通知のタイトル", udonscript.playerjoin);
                udonscript.joined = EditorGUILayout.TextField("入った際の文字", udonscript.joined);
                udonscript.leave = EditorGUILayout.TextField("抜けた際の文字", udonscript.leave);
                udonscript.textmesh.font = EditorGUILayout.ObjectField("フォント", udonscript.textmesh.font, typeof(Font), true) as Font;
                udonscript.textmesh.color = EditorGUILayout.ColorField("色", udonscript.textmesh.color);
            }
            GUILayout.Space(5);

            style.fontSize = 12;
            GUILayout.Label("輪郭線", style);

            GUILayout.Space(5);

            udonscript.outline = EditorGUILayout.ObjectField("輪郭線", udonscript.outline, typeof(Outline), true) as Outline;
            if (udonscript.outline != null)
            {
                udonscript.outline.effectColor = EditorGUILayout.ColorField("カラー", udonscript.outline.effectColor);
            }
            GUILayout.Space(10);

            EditorUtility.SetDirty(udonscript);
        }
    }
}
