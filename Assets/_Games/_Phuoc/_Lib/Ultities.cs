using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ultities
{
    private static float gravity = 10f;
    /// <summary>
    /// (floor, ceiling) -> (min, max)
    /// </summary>
    /// <param name="value"></param>
    /// <param name="floor"></param>
    /// <param name="ceiling"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float GetValue(float value, float floor, float ceiling, float min, float max)
    {
        return (ceiling == floor) ? 0 : (value - floor) * (max - min) / (ceiling - floor) + min;
    }

    //tinh toan ve parabol theo thoi gian
    public static Vector2 CalculateLinePoint(Vector2 throwPoint, Vector2 force, float wind, float t)
    {
        float x = (force.x * t) + (wind * t * t * 0.5f);
        float y = (force.y * t) - (gravity * t * t * 0.5f);
        return new Vector2(x + throwPoint.x, y + throwPoint.y);
    }

    //tinh toan ty le velocity de ban toi 1 diem cho trc
    public static float CaculateRatioVelocity(Vector2 throwPoint, Vector2 force, Vector2 targetPoint, float wind)
    {
        float dx = targetPoint.x - throwPoint.x;
        float dy = targetPoint.y - throwPoint.y;

        //return Mathf.Sqrt((gravity * dx * dx) / (2 * force.x * ( dx * force.y + dy * force.x)));

        //float t = Mathf.Sqrt(2 * (dx * force.y - dy * force.x) / (wind * force.y + gravity * force.x));
        float t = Mathf.Sqrt(2 * (dx * force.y - dy * force.x) / (wind * force.y + gravity * force.x));

        float vx = (dx - wind * t * t * 0.5f) / t;
        //float vy = (dy + gravity * t * t * 0.5f) / t;

        //float x = vx * t + 0.5f * wind * t * t + throwPoint.x;
        //float y = vy * t - 0.5f * gravity * t * t + throwPoint.y;

        return vx / force.x;
    }

    public static float TimeToTarget(Vector2 throwPoint, Vector2 force, Vector2 targetPoint, float wind)
    {
        float dx = targetPoint.x - throwPoint.x;
        float dy = targetPoint.y - throwPoint.y;

        float t = Mathf.Sqrt(2 * (dx * force.y - dy * force.x) / (wind * force.y + gravity * force.x));

        return t;
    }

    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static List<T> SortOrder<T>(List<T> list)
    {
        return list.OrderBy(d => System.Guid.NewGuid()).Take(list.Count).ToList();
    }
}
