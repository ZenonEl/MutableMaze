using System;

namespace MutableMaze
{
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Player(int startX, int startY)
        {
            X = startX;
            Y = startY;
        }

        public void Move(char direction)
        {
            throw new NotImplementedException();
        }
    }
}
