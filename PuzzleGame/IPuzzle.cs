
namespace Puzzle
{
    delegate void WinPuzzleDelageta();

    interface IPuzzle
    {
        event WinPuzzleDelageta WinPuzzle;
        string LawGame { get; }
    }
}
