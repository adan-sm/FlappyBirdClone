﻿using SFML.Graphics;
using SFML.Window;
using System.Diagnostics;

namespace FlappyBirdClone
{
    class FlappyBirdGame
    {
        public static float TimeStep = 1 / 60.0f;
        public static float MaxAccumulatedTickTime = 5.0f * TimeStep;

        public const int WindowWidth = 1024;
        public const int WindowHeight = 768;

        public void Run()
        {
            InitializeWindow();
            InitializeGameLevel();

            double accumulator = 0.0;

            var stopWatch = Stopwatch.StartNew();

            while (_window.IsOpen)
            {
                if (_currentState == State.Playing)
                {
                    accumulator = ApplyPhysics(accumulator, stopWatch);

                    // check bird intersection with ground
                    if (_bird.Position.Y + Bird.BirdRadius / 2.0f >= _ground.GroundPosition)
                    {
                        _currentState = State.GameOver;
                    }

                    // check bird intersection with any column
                    if (_columns.IntersectWithBird(_bird))
                    {
                        _currentState = State.GameOver;
                    }
                }

                Draw();
            }
        }

        private double ApplyPhysics(double accumulator, Stopwatch stopWatch)
        {
            var elapsedTime = stopWatch.ElapsedMilliseconds;
            stopWatch.Restart();

            accumulator += elapsedTime;

            if (accumulator > MaxAccumulatedTickTime) accumulator = MaxAccumulatedTickTime;

            while (accumulator >= TimeStep)
            {
                _columns.FixUpdate();
                _bird.FixUpdate();

                accumulator -= TimeStep;
            }

            return accumulator;
        }

        private void Draw()
        {
            _window.DispatchEvents();
            _window.Clear(CornFlowerBlue);

            if(_currentState == State.Playing)
            {
                _ground.Draw(_window);
                _bird.Draw(_window);
                _columns.Draw(_window);
            }
            else
            {
                _window.Draw(_gameOverText);
            }

            _window.Display();
        }

        private void InitializeGameLevel()
        {
            _bird = new Bird();
            _ground = new Ground();
            _columns = new Columns();
        }

        private void InitializeWindow()
        {
            _window = new RenderWindow(new VideoMode(WindowWidth, WindowHeight), "SFML graphics with OpenGL", Styles.Close);

            _window.SetVerticalSyncEnabled(true);

            _window.Closed += (_, __) => _window.Close();
            _window.KeyReleased += Window_KeyReleased;

            var font = new Font("Ocean Summer.ttf");

            _gameOverText = new Text("Game over !\r\nPress Space to restart", font);

            _gameOverText.FillColor = Color.Black;
            _gameOverText.Position = new SFML.System.Vector2f((WindowWidth - _gameOverText.GetGlobalBounds().Width) / 2, (WindowHeight - _gameOverText.GetGlobalBounds().Height) / 2);
            _gameOverText.CharacterSize = 48;
        }

        private void Window_KeyReleased(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Space)
            {
                if(_currentState == State.Playing)
                {
                    _bird.Jump();
                }
                else
                {
                    _bird = new Bird();
                    _columns = new Columns();

                    _currentState = State.Playing;
                }
            }
        }

        RenderWindow _window;
        Bird _bird;
        Ground _ground;
        Text _gameOverText;
        Columns _columns;

        State _currentState = State.Playing;

        enum State
        {
            Playing,
            GameOver
        }

        static readonly Color CornFlowerBlue = new Color(154, 206, 235);
    }
}
