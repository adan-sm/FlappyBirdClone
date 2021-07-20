using SFML.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace FlappyBirdClone
{
    class Columns
    {
        public Columns()
        {
            _columns = new List<Column>();

            // populate first column
            _columns.Add(new Column(FlappyBirdGame.WindowHeight / 2, FlappyBirdGame.WindowWidth / 2));

            // populate next columns
            var columnCount = 25;
            for (int i = 1; i < columnCount; i++)
            {
                var position = _columns[i - 1].Position + Column.Width / 2.0f + Column.SpaceBetweenColumns;
                var height = _columns[i - 1].Height;

                _columns.Add(new Column(height, (int)position));
            }
        }

        public void FixUpdate()
        {
            _columns.ForEach(c => c.FixUpdate());

            if (_columns[0].Position <= -Column.Width)
            {
                _columns.Add(new Column(_columns.Last().Height, (int)(_columns.Last().Position + Column.SpaceBetweenColumns)));

                _columns.RemoveAt(0);
            }
        }

        public bool IntersectWithBird(Bird bird)
        {
            return _columns.Any(c => c.Intersect(bird.Position, Bird.BirdRadius));
        }

        public void Draw(RenderWindow renderWindow)
        {
            _columns.ForEach(c => c.Draw(renderWindow));
        }

        private List<Column> _columns;
    }
}
