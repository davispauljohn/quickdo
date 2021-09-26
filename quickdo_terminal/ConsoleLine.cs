using System;

namespace quickdo_terminal
{
    public class ConsoleLine
    {
        public string Text { get; private set; }
        public ConsoleColor Colour { get; private set; }

        public ConsoleLine(string text, ConsoleColor colour)
        {
            Text = text;
            Colour = colour;
        }
    }
}