using System.Collections.Generic;

namespace quickdo_terminal
{
    public interface IInputService
    {
        List<ConsoleLine> ParseAndRunInput(string[] args);
    }
}