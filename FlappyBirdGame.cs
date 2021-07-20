using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Diagnostics;
using System.IO;

namespace FlappyBirdClone
{
    class FlappyBirdGame
    {
        public const string BestScoreFilePath = "data.bin";
        public static float TimeStep = 1 / 60.0f;
        public static float MaxAccumulatedTickTime = 5.0f * TimeStep;

        public const int WindowWidth = 1024;
        public const int WindowHeight = 768;

        public const float ScrollingSpeed = 50.0f;

        public void Run()
        {
            InitializeWindow();
            InitializeGameLevel();

            InitializeBestScore();

            double accumulator = 0.0;

            var stopWatch = Stopwatch.StartNew();

            while (_window.IsOpen)
            {
                if (_currentState == State.Playing)
                {
                    accumulator = ApplyPhysics(accumulator, stopWatch);

                    CheckForGameOver();
                }

                Draw();
            }
        }

        private void CheckForGameOver()
        {
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

            if (_currentState == State.GameOver)
            {
                var gameOverString = $"Game over !\r\nPress Space to restart\r\n";

                if (_bestScore < _score)
                { 
                    using var file = new BinaryWriter(File.Open(BestScoreFilePath, FileMode.Create));
                    file.Write(_score);
                    _bestScore = _score;

                    gameOverString += $"\r\n!!New record: {(int)_score}!!";
                }

                _gameOverText.DisplayedString = gameOverString;
                _gameOverText.Position = new Vector2f((WindowWidth - _gameOverText.GetGlobalBounds().Width) / 2, (WindowHeight - _gameOverText.GetGlobalBounds().Height) / 2);
            }
        }

        private void InitializeBestScore()
        {
            if (!File.Exists(BestScoreFilePath))
            {
                _bestScore = 0.0f;
                return;
            }

            using var file = new BinaryReader(File.Open(BestScoreFilePath, FileMode.Open));
            _bestScore = file.ReadSingle();
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

                _score += TimeStep;

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

                _scoreText.DisplayedString = ((int)_score).ToString();
                _window.Draw(_scoreText);
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
            _score = 0;
        }

        private void InitializeWindow()
        {
            _window = new RenderWindow(new VideoMode(WindowWidth, WindowHeight), "SFML graphics with OpenGL", Styles.Close);

            _window.SetVerticalSyncEnabled(true);

            _window.Closed += (_, __) => _window.Close();
            _window.KeyReleased += Window_KeyReleased;

            var font = new Font("Ocean Summer.ttf");

            _gameOverText = new Text(string.Empty, font)
            {
                FillColor = Color.Black,
                CharacterSize = 48
            };

            _scoreText = new Text(string.Empty, font)
            {
                FillColor = Color.Black,
                CharacterSize = 48,
                Position = new Vector2f(10, 10)
            };
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
                    InitializeGameLevel();
                }
            }
        }

        RenderWindow _window;
        Bird _bird;
        Ground _ground;
        Columns _columns;
        float _score;
        float _bestScore;

        Text _gameOverText;
        Text _scoreText;
        
        State _currentState = State.Playing;

        enum State
        {
            Playing,
            GameOver
        }

        static readonly Color CornFlowerBlue = new Color(154, 206, 235);
    }
}
