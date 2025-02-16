using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJSON;
//using Facebook.MiniJSON;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using UnityEngine.EventSystems;

public static class Utilss {
    /// <summary>
    /// Get 0 to 360 angle.
    /// </summary>
    public static float Get360Angle(float angle) {
        while (angle < 0f) {
            angle += 360f;
        }
        while (360f < angle) {
            angle -= 360f;
        }
        return angle;
    }
    
    public static float GetShiftedAngle(int wayIndex, float baseAngle, float betweenAngle) {
        float angle = wayIndex % 2 == 0 ?
                      baseAngle - (betweenAngle * (float)wayIndex / 2f) :
                      baseAngle + (betweenAngle * Mathf.Ceil((float)wayIndex / 2f));
        return angle;
    }
    public static Vector2 GetPointOnRing(float aMinRadius, float aMaxRadius) {
        var v = UnityEngine.Random.insideUnitCircle;
        return v.normalized * aMinRadius + v * (aMaxRadius - aMinRadius);
    }
    public static Vector3 ToXZ(this Vector2 aVec) {
        return new Vector3(aVec.x, 0, aVec.y);
    }
    public static Vector3 ToXZ(this Vector2 aVec, float aYValue) {
        return new Vector3(aVec.x, aYValue, aVec.y);
    }
    public static int GetPercentageRemaining(int num1, int num2) {
        float per = ((float)num1 / (float)num2) * 100f;
        return (int)per;
    }

    public static bool IsInRange(int value, int min, int max) {
        if (value >= min && value <= max) {
            return true;
        }
        return false;
    }
    public static bool IsInRange(float value, float min, float max) {
        if (value >= min && value <= max) {
            return true;
        }
        return false;
    }
    public static float SinDeg(float deg) {
        return Mathf.Sin(deg * Mathf.Deg2Rad);
    }
    public static float CosDeg(float deg) {
        return Mathf.Cos(deg * Mathf.Deg2Rad);
    }
    public static bool RoughlyEqual(float a, float b) {
        float treshold = 0.00001f;
        //Debug.Log("A " + a + " " + b);
        return (Mathf.Abs(a - b) <= treshold);
    }
    public static float AngleBetweenVectors(Vector2 v1, Vector2 v2) {
        Vector2 fromVector2 = v1;
        Vector2 toVector2 = v2;

        float ang = Vector2.Angle(fromVector2, toVector2);
        Vector3 cross = Vector3.Cross(fromVector2, toVector2);

        if (cross.z > 0)
            ang = 360 - ang;

        return ang;
    }
    public static float AngleBetweenVectorsNegative(Vector2 v1, Vector2 v2) {
        Vector2 fromVector2 = v1;
        Vector2 toVector2 = v2;

        float ang = Vector2.Angle(fromVector2, toVector2);
        Vector3 cross = Vector3.Cross(fromVector2, toVector2);

        return ang;
    }
    public static double[] SolveQuadratic(double a, double b, double c) {
        double[] rArray;
        double sqrtpart = b * b - 4 * a * c;
        double x, x1, x2, img;
        if (sqrtpart > 0) {
            x1 = (-b + System.Math.Sqrt(sqrtpart)) / (2 * a);
            x2 = (-b - System.Math.Sqrt(sqrtpart)) / (2 * a);
            //Debug.Log("Two Real Solutions: " + x1 + " or " + x2);
            rArray = new double[2];
            rArray[0] = x1;
            float num1 = Mathf.Atan((float)x1) * Mathf.Rad2Deg;
            rArray[1] = x2;
            return rArray;
        } else if (sqrtpart < 0) {
            sqrtpart = -sqrtpart;
            x = -b / (2 * a);
            img = System.Math.Sqrt(sqrtpart) / (2 * a);
            //Debug.Log("Two Imaginary Solutions: " + x + " + " + img + " i or " + x + " + " + img +" i");
            rArray = new double[1];
            rArray[0] = 90;
            return rArray;
        } else {
            x = (-b + System.Math.Sqrt(sqrtpart)) / (2 * a);
            //Debug.Log("One Real Solution: " + x);
            rArray = new double[1];
            rArray[0] = x;
            return rArray;
        }
    }
    public static string TrimString(string s) {
        string ss = s.Substring(1, s.Length - 2);
        return ss;
    }
    public static string GetTimeStringFromSecond(double time) {
        int totalSecond = (int)time;
        int min = totalSecond / 60;
        int sec = totalSecond % 60;
        int hour = min / 60;
        min = min % 60;
        return (hour >= 10 ? hour.ToString() : "0" + hour.ToString()) + ":" + (min >= 10 ? min.ToString() : "0" + min.ToString()) + ":" + (sec >= 10 ? sec.ToString() : "0" + sec.ToString());
    }
    public static string GetTimeStringFromSecond2(double time) {
        int totalSecond = (int)time;
        int min = totalSecond / 60;
        int sec = totalSecond % 60;
        int hour = min / 60;
        min = min % 60;
        return (min >= 10 ? min.ToString() : "0" + min.ToString()) + ":" + (sec >= 10 ? sec.ToString() : "0" + sec.ToString());
    }
    public static string GetTimeStringFromSecond3(double time) {
        int totalSecond = (int)time;
        int min = totalSecond / 60;
        int sec = totalSecond % 60;
        int hour = min / 60;
        min = min % 60;
        return (min >= 10 ? min.ToString() : "0" + min.ToString()) + "':" + (sec >= 10 ? sec.ToString() : "0" + sec.ToString())+"\"";
    }
    private static System.Random rng = new System.Random();
    /// <summary>
    /// Shuffle List
    /// </summary>
    /// <typeparam name="T">List type.</typeparam>
    /// <param name="list"> Input list.</param>
    public static void Shuffle<T>(this IList<T> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static byte[] GetBytes(string str) {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }
    public static string GetString(byte[] bytes) {
        char[] chars = new char[bytes.Length / sizeof(char)];
        System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }
    public static bool isLeft(Vector3 a, Vector3 b, Vector3 c) {
        return ((b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x)) > 0;
    }
    public static float GetBiger(float a, float b) {
        if (a < b) {
            return b;
        } else {
            return a;
        }
    }
    public static int GetBiger(int a, int b) {
        if (a < b) {
            return b;
        } else {
            return a;
        }
    }
    public static Vector3 RotateVector(Vector3 v1, float angle) {
        return Quaternion.Euler(0, -angle, 0) * v1;
    }
    public static float GetMax(List<float> list) {
        if (list.Count == 0) return 0;
        float max = list[0];
        for (int i = 0; i < list.Count; i++) {
            if (list[i] > max) {
                max = list[i];
            }
        }
        return max;
    }
    public static float GetMin(List<float> list) {
        if (list.Count == 0) return 0;
        float min = list[0];
        for (int i = 0; i < list.Count; i++) {
            if (list[i] < min) {
                min = list[i];
            }
        }
        return min;
    }
    public static T[] RemoveItemFromArray<T>(T[] source, int index) {
        T[] dest = new T[source.Length - 1];
        if (index > 0) {
            Array.Copy(source, 0, dest, 0, index);
        }
        if (index < source.Length - 1) {
            Array.Copy(source, index + 1, dest, index, source.Length - 1 - index);
        }
        return dest;
    }
    public static float CountLength(List<Vector3> list) {
        if (list.Count == 0) return 0;
        float length = 0;
        for (int i = 1; i < list.Count; i++) {
            Vector3 v = list[i] - list[i - 1];
            length += v.magnitude;
        }
        return length;
    }
    public static Color GetColor(float r, float g, float b) {
        float max = 255f;
        Color c = new Color(r / max, g / max, b / max);
        return c;
    }
    public static Color GetColor(string hex) {
        Color color;
        if (ColorUtility.TryParseHtmlString(hex, out color)) {
        }
        return color;
    }
    public static float bezLength3(Vector3[] b) {
        Vector3 p0 = (b[0] - b[1]);
        Vector3 p1 = (b[2] - b[1]);
        Vector3 p2;
        Vector3 p3 = (b[3] - b[2]);
        float l0 = p0.magnitude;
        float l1 = p1.magnitude;
        float l3 = p3.magnitude;
        if (l0 > 0f) p0 /= l0;
        if (l1 > 0f) p1 /= l1;
        if (l3 > 0f) p3 /= l3;
        p2 = -p1;
        float a = Mathf.Abs(Vector3.Dot(p0, p1)) + Mathf.Abs(Vector3.Dot(p2, p3));
        if ((a > 1.98f) || ((l0 + l1 + l3) < ((4f - a) * 8f))) {
            return l0 + l1 + l3;
        } // if
        Vector3[] bl = new Vector3[4];
        Vector3[] br = new Vector3[4];
        bl[0] = b[0];
        bl[1] = (b[0] + b[1]) * .5f;
        Vector3 mid = (b[1] + b[2]) * .5f;
        bl[2] = (bl[1] + mid) * .5f;
        br[3] = b[3];
        br[2] = (b[2] + b[3]) * .5f;
        br[1] = (br[2] + mid) * .5f;
        br[0] = (br[1] + bl[2]) * .5f;
        bl[3] = br[0];
        return bezLength3(bl) + bezLength3(br);
    } // bezLength3
    public static int GetTotalZero(int number) {
        int num = 0;
        number = number / 10;
        while (number > 0) {
            num++;
            number = number / 10;
        }
        return num;
    }
    public static Vector2 PerpendicularClockwise(Vector2 vector2) {
        return new Vector2(-vector2.y, vector2.x);
    }

    public static Vector2 PerpendicularCounterClockwise(Vector2 vector2) {
        return new Vector2(vector2.y, -vector2.x);
    }
    public static List<object> DeserializeJSONFriends(string response) {
        //var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
        var responseObject = new Dictionary<string, object>();
        object friendsH;
        var friends = new List<object>();
        if (responseObject.TryGetValue("invitable_friends", out friendsH)) {
            friends = (List<object>)(((Dictionary<string, object>)friendsH)["data"]);
        }
        if (responseObject.TryGetValue("friends", out friendsH)) {
            friends.AddRange((List<object>)(((Dictionary<string, object>)friendsH)["data"]));
        }
        return friends;
    }
    public static float CalculateTextSize(string message, Text chatText) {
        TextGenerator textGen = new TextGenerator();
        TextGenerationSettings generationSettings = chatText.GetGenerationSettings(chatText.rectTransform.rect.size);
        float width = textGen.GetPreferredWidth(message, generationSettings);
        float height = textGen.GetPreferredHeight(message, generationSettings);
        return width;
    }
    public static Vector3 ClampCamera(Vector3 pos) {
        Vector3 v = Camera.main.transform.position;
        if (pos.x < v.x - 5) {
            pos.x = v.x - 5;
        }
        if (pos.x > v.x + 5) {
            pos.x = v.x + 5;
        }
        if (pos.y > v.y + 9f) {
            pos.y = v.y + 9f;
        }
        return pos;
    }
    public static string GetTextCoolDown(double secondTotal) {
        int iTotalSecond = (int)secondTotal;
        int day = iTotalSecond / 86400;
        int min = iTotalSecond / 60;
        int sec = iTotalSecond % 60;
        int hour = min / 60;
        hour = hour - day * 24;
        min = min % 60;

        string d = "";
        if (day > 0) {
            d = day + "D:";
        }
        string h = "";
        if (hour < 10) {
            h = "0" + hour + "H:";
        } else {
            h = hour + "H:";
        }
        string m = "";
        if (min < 10) {
            m = "0" + min + "M:";
        } else {
            m = min + "M:";
        }
        string s = "";
        if (sec < 10) {
            s = "0" + sec + "S";
        } else {
            s = sec + "S";
        }
        string txtCooldown = d + h + m + s;
        return txtCooldown;
    }
    public static string GetTextCoolDown2(double secondTotal) {
        int iTotalSecond = (int)secondTotal;
        int day = iTotalSecond / 86400;
        int min = iTotalSecond / 60;
        int sec = iTotalSecond % 60;
        int hour = min / 60;
        hour = hour - day * 24;
        min = min % 60;

        string d = "";
        if (day > 0) {
            d = day + ":";
        }
        string h = "";
        if (hour < 10) {
            h = "0" + hour + ":";
        } else {
            h = hour + ":";
        }
        string m = "";
        if (min < 10) {
            m = "0" + min + ":";
        } else {
            m = min + ":";
        }
        string s = "";
        if (sec < 10) {
            s = "0" + sec + "";
        } else {
            s = sec + "";
        }
        string txtCooldown = d + h + m + s;
        return txtCooldown;
    }
    public static string EnCryptMD5(string strEnCrypt, string key) {
        try {
            byte[] keyArr;
            byte[] EnCryptArr = UTF8Encoding.UTF8.GetBytes(strEnCrypt);
            MD5CryptoServiceProvider MD5Hash = new MD5CryptoServiceProvider();
            keyArr = MD5Hash.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider();
            tripDes.Key = keyArr;
            tripDes.Mode = CipherMode.ECB;
            tripDes.Padding = PaddingMode.PKCS7;
            ICryptoTransform transform = tripDes.CreateEncryptor();
            byte[] arrResult = transform.TransformFinalBlock(EnCryptArr, 0, EnCryptArr.Length);
            return Convert.ToBase64String(arrResult, 0, arrResult.Length);
        } catch (Exception) { }
        return "";
    }
    public static string DeCryptMD5(string strDecypt, string key) {
        try {
            byte[] keyArr;
            byte[] DeCryptArr = Convert.FromBase64String(strDecypt);
            MD5CryptoServiceProvider MD5Hash = new MD5CryptoServiceProvider();
            keyArr = MD5Hash.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider();
            tripDes.Key = keyArr;
            tripDes.Mode = CipherMode.ECB;
            tripDes.Padding = PaddingMode.PKCS7;
            ICryptoTransform transform = tripDes.CreateDecryptor();
            byte[] arrResult = transform.TransformFinalBlock(DeCryptArr, 0, DeCryptArr.Length);
            return UTF8Encoding.UTF8.GetString(arrResult);
        } catch (Exception) { }
        return "";
    }
    private static System.Random random = new System.Random();
    public static string RandomString(int length) {
        const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTabcdefghijklmnopqrst";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
    public static int GetWorldByLevel(int level) {
        int world = (level - 1) / 25 + 1;
        if (world < 1) world = 1;
        return world;
    }
    public static int GetMilestone(int level) {
        if (level == 0) return 0;
        int milestone = 0;
        int range = 10;
        while (level > range) {
            level -= range;
            milestone++;
        }
        return milestone;
    }
    public static IEnumerable<TValue> RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict) {
        System.Random rand = new System.Random();
        List<TValue> values = Enumerable.ToList(dict.Values);
        int size = dict.Count;
        while (true) {
            yield return values[rand.Next(size)];
        }
    }
    public static T ParseEnum<T>(string value) {
        return (T)Enum.Parse(typeof(T), value, ignoreCase: true);
    }
    public static int GetVersionCode() {
        AndroidJavaClass contextCls = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject context = contextCls.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageMngr = context.Call<AndroidJavaObject>("getPackageManager");
        string packageName = context.Call<string>("getPackageName");
        AndroidJavaObject packageInfo = packageMngr.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);
        return packageInfo.Get<int>("versionCode");
    }

    //int versionName =  context().getPackageManager().getPackageInfo(context().getPackageName(), 0).versionName;
    public static string GetVersionName() {
        AndroidJavaClass contextCls = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject context = contextCls.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageMngr = context.Call<AndroidJavaObject>("getPackageManager");
        string packageName = context.Call<string>("getPackageName");
        AndroidJavaObject packageInfo = packageMngr.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);
        return packageInfo.Get<string>("versionName");
    }
    public static bool IsBetween(float value, float bound1, float bound2) {
        return (value >= Math.Min(bound1, bound2) && value <= Math.Max(bound1, bound2));
    }
    public static bool IsBetween(Vector3 from, Vector3 to, float v) {
        float distance = Mathf.Abs(from.x - to.x);
        bool isIn =  distance <= v;
        return isIn;
    }
    public static bool IsPointerOverGameObject() {
        //check mouse
        if (EventSystem.current.currentSelectedGameObject != null) {
            //Debug.Log("Select " + EventSystem.current.currentSelectedGameObject.name);
            return true;
        }
        //check touch
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                return true;
        }

        return false;
    }
    /// <summary>
    /// lay so luong cac phan tu trong enum
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static int GetEnumCount<T>() where T : Enum
    {
        return Enum.GetNames(typeof(T)).Length;
    }
    //public static int sortEquipItem(EquipItemData x, EquipItemData y) {
    //    if (x.GetLevel() > y.GetLevel()) {
    //        return -1;
    //    } else if (x.GetLevel() < y.GetLevel()) {
    //        return 1;
    //    }
    //    if(x.GetRarity()> y.GetRarity()) {
    //        return -1;
    //    } else if (x.GetRarity() < y.GetRarity()) {
    //        return 1;
    //    }
    //    if (x.GetRank() > y.GetRank()) {
    //        return -1;
    //    } else if (x.GetRank() < y.GetRank()) {
    //        return 1;
    //    }
    //    if (x.GetRace() > y.GetRace()) {
    //        return -1;
    //    } else if (x.GetRace() < y.GetRace()) {
    //        return 1;
    //    }
    //    return 1;
    //}
}
public class MiniPool<T> : List<T>
{
    public GameObject prefab;
    public Transform parent;
    public void Init(int amount, GameObject prefab, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
        while (Count < amount)
        {
            GameObject go = GameObject.Instantiate(prefab, parent);
            go.transform.localScale = new Vector3(1, 1, 1);
            T t = go.GetComponent<T>();
            Add(t);
        }
    }
    public T AddNewItem() {
        GameObject go = GameObject.Instantiate(prefab, parent);
        go.transform.localScale = new Vector3(1, 1, 1);
        T t = go.GetComponent<T>();
        Add(t);
        return t;
    }
}
//public static class Debug {
//    public static void LogEditor(this MonoBehaviour player, string log) {
//#if UNITY_EDITOR
//        UnityEngine.Debug.Log(player.name+": " + log);
//#endif
//    }
//    public static void LogAll(this MonoBehaviour player, string log) {
//        UnityEngine.Debug.Log(player.name + ": " + log);
//    }
//    public static void LogAndroid(this MonoBehaviour player, string log) {
//#if UNITY_ANDROID
//        UnityEngine.Debug.Log(player.name+": " + log);
//#endif
//    }
//    public static void LogIOS(this MonoBehaviour player, string log) {
//#if UNITY_IPHONE
//        UnityEngine.Debug.Log(player.name+": " + log);
//#endif
//    }
//    public static void Log(string log) {
//        UnityEngine.Debug.Log(log);
//    }
//    public static void LogError(string log) {
//        UnityEngine.Debug.LogError(log);
//    }
//    public static void LogException(Exception e) {
//        UnityEngine.Debug.LogException(e);
//    }
//}