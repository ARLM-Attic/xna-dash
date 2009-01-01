namespace XNADash.Sprites
{
    public class MovementVector
    {
        #region DirectionX enum

        public enum DirectionX
        {
            Left = -1,
            None = 0,
            Right = 1
        }

        #endregion

        #region DirectionY enum

        public enum DirectionY
        {
            Up = -1,
            None = 0,
            Down = 1
        }

        #endregion

        private float horizontalAcceleration = 5f;
        private float horizontalDeceleration = 10f;

        private float horizontalVelocity;
        private float maxHorizontalVelocity = 20f;
        private float maxVerticalVelocity = 20f;
        private float verticalVelocity;
        private float weight = 1f;
        private DirectionX xDirection = DirectionX.None;
        private DirectionY yDirection = DirectionY.None;

        /// <summary>
        /// The horizontal speed of the sprite
        /// </summary>
        public float HorizontalVelocity
        {
            get { return horizontalVelocity; }
            set
            {
                if (value <= maxHorizontalVelocity && value >= 0)
                    horizontalVelocity = value;
            }
        }

        /// <summary>
        /// The vertical speed to the sprite
        /// </summary>
        public float VerticalVelocity
        {
            get { return verticalVelocity; }
            set
            {
                if (value <= maxVerticalVelocity && value >= 0)
                    verticalVelocity = value;
            }
        }

        /// <summary>
        /// The maximum speed the sprite is allowed to go vertically
        /// </summary>
        public float MaxHorizontalVelocity
        {
            get { return maxHorizontalVelocity; }
            set
            {
                if (value <= float.MaxValue && value >= 0)
                    maxHorizontalVelocity = value;
            }
        }

        /// <summary>
        /// The maximum speed the sprite is allowed to go horizontally
        /// </summary>
        public float MaxVerticalVelocity
        {
            get { return maxVerticalVelocity; }
            set
            {
                if (value <= float.MaxValue && value >= 0)
                    maxVerticalVelocity = value;
            }
        }

        /// <summary>
        /// How much should the speed accelerate in horizontal direction
        /// </summary>
        public float HorizontalAcceleration
        {
            get { return horizontalAcceleration; }
            set
            {
                if (value <= float.MaxValue && value >= 0)
                    horizontalAcceleration = value;
            }
        }

        /// <summary>
        /// How much should the speed deaccelerate in horizontal direction
        /// </summary>
        public float HorizontalDeceleration
        {
            get { return horizontalDeceleration; }
            set
            {
                if (value <= float.MaxValue && value >= 0)
                    horizontalDeceleration = value;
            }
        }

        /// <summary>
        /// Used when calculating gravity
        /// </summary>
        public float Weight
        {
            get { return weight; }
            set
            {
                if (value <= float.MaxValue && value >= float.MinValue)
                    weight = value;
            }
        }

        public bool IsOnGround { get; set; }

        /// <summary>
        /// In what direction are we moving along the x axis
        /// </summary>
        public DirectionX XDirection
        {
            get { return xDirection; }
            set { xDirection = value; }
        }

        /// <summary>
        /// In what direction are we moving along the y axis
        /// </summary>
        public DirectionY YDirection
        {
            get { return yDirection; }
            set { yDirection = value; }
        }

        /// <summary>
        /// Determines if the vector can be defined as "moving"
        /// </summary>
        public bool IsMoving()
        {
            bool returnValue = false;

            if (verticalVelocity != 0 || horizontalVelocity != 0)
                returnValue = true;

            return returnValue;
        }
    }
}