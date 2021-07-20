using SFML.Graphics;
using SFML.System;
using System;

namespace FlappyBirdClone
{
    class Column
    {
        public const float Width = 80.0f;
        public const float Difference = 80.0f;
        public const float Speed = 50.0f;
        public const float SpaceBetweenColumns = 250.0f;

        public Column(int previousSize, int position)
        {
            var range = (previousSize - 50, previousSize + 50);
            Height = _random.Next(range.Item1, range.Item2);
            Position = position;

            InitializeGraphics();
        }

        private void InitializeGraphics()
        {
            var baseSize = FlappyBirdGame.WindowHeight - Ground.Size;

            _bottom = new RectangleShape(new SFML.System.Vector2f(Width, baseSize - Height - Difference));
            _top = new RectangleShape(new SFML.System.Vector2f(Width, Height - Difference));

            _bottom.FillColor = new Color(124, 46, 34);
            _top.FillColor = new Color(124, 46, 34);

            UpdatePosition();
        }

        public void FixUpdate()
        {
            Position -= FlappyBirdGame.TimeStep * Speed;

            UpdatePosition();
        }

        private void UpdatePosition()
        {
            _bottom.Position = new SFML.System.Vector2f(Position, FlappyBirdGame.WindowHeight - Ground.Size - _bottom.Size.Y);
            _top.Position = new SFML.System.Vector2f(Position, 0);
        }

        public void Draw(RenderWindow renderWindow)
        {
            renderWindow.Draw(_bottom);
            renderWindow.Draw(_top);
        }

        public bool Intersect(Vector2f birdPosition, float birdRadius) => SphereAABBTest.Intersect(birdPosition, birdRadius, _bottom.GetGlobalBounds()) || SphereAABBTest.Intersect(birdPosition, birdRadius, _top.GetGlobalBounds());

        public int Height { get; }

        public float Position { get; private set; }

        RectangleShape _bottom;
        RectangleShape _top;

        static Random _random = new Random();
    }
}
