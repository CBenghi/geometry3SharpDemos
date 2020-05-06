using System;

namespace geometry3Test
{
    internal class ConsoleColorController : IDisposable
    {
        ConsoleColor savedColor;

        public ConsoleColorController()
        {
            savedColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        public void Dispose()
        {
            Console.ForegroundColor = savedColor;
        }
    }
}