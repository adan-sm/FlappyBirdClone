using SFML.Graphics;
using SFML.System;

namespace FlappyBirdClone
{
    class Bird
    {
        public const float BirdRadius = 15.0f;

        public Bird()
        {
            _circleShape = new CircleShape(BirdRadius);
            _circleShape.FillColor = Color.Blue;

            _circleShape.Position = new Vector2f(FlappyBirdGame.WindowWidth / 2.0f, FlappyBirdGame.WindowHeight / 2.0f);
        }

        public void FixUpdate()
        {
            ComputePosition();
        }

        public void Jump()
        {
            _jumpRequested = true;
        }

        private void ComputePosition()
        {
            var forces = new Vector2f(0, 10.0f); // apply gravity
            var position = _circleShape.Position;

            position += _velocity * FlappyBirdGame.TimeStep;
            _velocity += forces * FlappyBirdGame.TimeStep; // mass of 1 implied

            if (_jumpRequested)
            {
                _velocity = new Vector2f(0, -50.0f);
                _jumpRequested = false;
            }

            _circleShape.Position = position;
        }

        public void Draw(RenderWindow renderWindow)
        {
            renderWindow.Draw(_circleShape);
        }

        // by copy
        public Vector2f Position => _circleShape.Position;

        Vector2f _velocity;
        CircleShape _circleShape;
        bool _jumpRequested;
    }
}
