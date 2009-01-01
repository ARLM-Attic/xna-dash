#region

using Microsoft.Xna.Framework;

#endregion

namespace XNADash.Collision
{
    public class CollisionUtility
    {
        public static bool Intersects(Rectangle a, Rectangle b)
        {
            // check if two Rectangles intersect
            return (a.Right > b.Left && a.Left < b.Right &&
                    a.Bottom > b.Top && a.Top < b.Bottom);
        }

        public static bool Touches(Rectangle a, Rectangle b)
        {
            // check if two Rectangles intersect or touch sides
            return (a.Right >= b.Left && a.Left <= b.Right &&
                    a.Bottom >= b.Top && a.Top <= b.Bottom);
        }
    }
}