using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace phirSOFT.ImageSlideShow
{
    public static class RectExtension
    {
        public static Size GetSize(this Rect rect)
        {
            return new Size(rect.Width, rect.Height);
        }

        public static double GetAspectRatio(this Rect rect)
        {
            return rect.Width / rect.Height;
        }

        public static double GetAspectRatio(this Size size)
        {
            return size.Width / size.Height;
        }

        public static Vector2 GetOffset(this Rect rect)
        {
            return new Vector2((float) rect.X, (float) rect.Y);
        }

        public static Point AsPoint(this Vector2 vector)
        {
            return new Point(vector.X, vector.Y);
        }

        public static Point CenterWithinRect(this Size size, Rect targetRect)
        {
            return ((targetRect.GetSize().ToVector2() - size.ToVector2()) / 2 + targetRect.GetOffset()).AsPoint();
        }
    }
}
