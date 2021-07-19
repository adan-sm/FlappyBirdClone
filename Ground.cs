using SFML.Graphics;

namespace FlappyBirdClone
{
    class Ground
    {
        public const float Size = 80.0f;

        public Ground()
        {
            _groundShape = new RectangleShape(new SFML.System.Vector2f(FlappyBirdGame.WindowWidth, Size));
            _groundShape.Position = new SFML.System.Vector2f(0, FlappyBirdGame.WindowHeight - Size);
            _groundShape.FillColor = new Color(0, 192, 50);
            _groundShape.OutlineColor = new Color(0, 127, 14);
            _groundShape.OutlineThickness = 1.0f;
        }

        public void Draw(RenderWindow renderWindow)
        {
            renderWindow.Draw(_groundShape);
        }

        public float GroundPosition => _groundShape.Position.Y;

        RectangleShape _groundShape;
    }
}
