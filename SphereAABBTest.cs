using SFML.Graphics;
using SFML.System;
using System;

namespace FlappyBirdClone
{
    static class SphereAABBTest
    {
        // go with an eay implementation
        public static bool Intersect(Vector2f circlePosition, float circleRadius, FloatRect rectangleShape)
        {
            // get box closest point to sphere center by clamping
            var x = Math.Max(rectangleShape.Left, Math.Min(circlePosition.X, rectangleShape.Left + rectangleShape.Width));
            var y = Math.Max(rectangleShape.Top, Math.Min(circlePosition.Y, rectangleShape.Top + rectangleShape.Height));

            // this is the same as isPointInsideSphere
            var distance = Math.Sqrt((x - circlePosition.X) * (x - circlePosition.X) +
                                     (y - circlePosition.Y) * (y - circlePosition.Y));

            return distance < circleRadius;
        }
    }
}
