using UnityEngine;
using System.Collections;


namespace Extensions
{
    public static class Vibration
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#elif !UNITY_IOS && !UNITY_STANDALONE
        public static AndroidJavaClass unityPlayer;
        public static AndroidJavaObject currentActivity;
        public static AndroidJavaObject vibrator;
#endif

        public static void Vibrate()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            vibrator.Call("vibrate");
#endif
#if !UNITY_STANDALONE && !UNITY_EDITOR
            Handheld.Vibrate();
#endif
        }


        public static void Vibrate(long milliseconds)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            vibrator.Call("vibrate", milliseconds);
#endif
#if !UNITY_STANDALONE && !UNITY_EDITOR
            Handheld.Vibrate();
#endif
        }

        public static void Vibrate(long[] pattern, int repeat)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            vibrator.Call("vibrate", pattern, repeat);
#endif
#if !UNITY_STANDALONE && !UNITY_EDITOR
            Handheld.Vibrate();
#endif
        }

        public static bool SupportsVibrator()
        {
            return SystemInfo.supportsVibration;
        }

        public static void Cancel()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            vibrator.Call("cancel");
#endif
        }
    }
}