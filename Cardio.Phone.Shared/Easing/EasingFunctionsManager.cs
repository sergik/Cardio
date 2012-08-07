using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Easing
{
    public class EasingFunctionsManager
    {
        //static public List<Vector2> GetPoints(Vector2 startPosition, Vector2 endPosition, int pointsCount, IEasingTransformer transformer)
        //{
        //    var xCoords = new List<float>();

        //    float delta = (endPosition.X - startPosition.X)/pointsCount;
        //    for (int i = 0; i <= pointsCount; i++)
        //    {
        //        xCoords.Add(startPosition.X + delta * i);
        //    }

        //    var yCoords = shiftFunction(xCoords, startPosition, endPosition);

        //    return yCoords.Select((t, i) => new Vector2(xCoords[i], t)).ToList();
        //}

        //public static List<float> LinearShift(List<float> xCoords, Vector2 startPoint, Vector2 endPoint)
        //{
        //    float k = (startPoint.Y - endPoint.Y) / (startPoint.X - endPoint.X);
        //    float b = startPoint.Y - k*startPoint.X;

        //    return xCoords.Select(x => x*k + b).ToList();
        //}

        //public static List<float> SinShift(List<float> xCoords, Vector2 startPoint, Vector2 endPoint)
        //{
        //    var r = new Random();
        //    float ratio = 1;
        //    float k = (startPoint.Y - endPoint.Y -
        //               (float)
        //               (-10*Math.Sin(MathHelper.ToRadians(startPoint.X)) + 10*Math.Sin(MathHelper.ToRadians(endPoint.X))))/
        //               (startPoint.X - endPoint.X);
        //    float b = startPoint.Y - k * (float)(10*Math.Sin(MathHelper.ToRadians(startPoint.X * ratio)) + (float)Math.Sin(MathHelper.ToRadians(startPoint.X)));

        //    return xCoords.Select(x => (float)(Math.Sin(MathHelper.ToRadians(x*ratio)) * k + b + 10*Math.Sin(MathHelper.ToRadians(x)))).ToList();
        //}

        //public static List<float> ParabolicShift(List<float> xCoords, Vector2 startPoint, Vector2 endPoint)
        //{
        //    float b = (startPoint.Y - startPoint.X*startPoint.X - endPoint.Y + endPoint.X * endPoint.X) / (startPoint.X - endPoint.X);
        //    float c = endPoint.Y - (endPoint.X * endPoint.X + b * endPoint.X);

        //    return xCoords.Select(x => x*x + b*x + c).ToList();
        //}

        public static int GetPointsCount(TimeSpan movingTime, TimeSpan updateRate)
        {
            return Convert.ToInt32(movingTime.TotalMilliseconds/updateRate.TotalMilliseconds);
        }
    }
}
