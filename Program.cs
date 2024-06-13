using System.Text;
using static System.Console;
Title = "Conway's Game of Life";
Random rand = new();
bool GetRandBool()
    => rand.Next() < (1 << 30);
int CountNeighbors(bool TopLeft, bool TopMiddle, bool TopRight, bool Left, bool Right, bool BottomLeft, bool BottomMiddle, bool BottomRight)
    => (TopLeft ? 1 : 0)
     + (TopMiddle ? 1 : 0)
     + (TopRight ? 1 : 0)
     + (Left ? 1 : 0)
     + (Right ? 1 : 0)
     + (BottomLeft ? 1 : 0)
     + (BottomMiddle ? 1 : 0)
     + (BottomRight ? 1 : 0);
bool BehaveByNeighbors(bool ThisAlive, int NeighborCount)
    => ThisAlive ? (NeighborCount is 2 or 3) : (NeighborCount == 3);
bool Behave(bool TopLeft, bool TopMiddle, bool TopRight, bool Left, bool Center, bool Right, bool BottomLeft, bool BottomMiddle, bool BottomRight)
    => BehaveByNeighbors(Center, CountNeighbors(TopLeft, TopMiddle, TopRight, Left, Right, BottomLeft, BottomMiddle, BottomRight));
int Mod(int Left, int Right)
    => Left < 0 ? ((Left % Right) + Right) : (Left % Right);
while (true)
{
    Clear();
    bool[,] values = new bool[WindowHeight - 1, WindowWidth - 1];
    bool[,] nextValues = new bool[values.GetLength(0), values.GetLength(1)];
    for (int i = 0; i < values.GetLength(0); i++)
        for (int j = 0; j < values.GetLength(1); j++)
            values[i, j] = GetRandBool();
    while (true)
    {
        StringBuilder sb = new();
        for (int i = 0; i < values.GetLength(0); i++)
        {
            for (int j = 0; j < values.GetLength(1); j++)
                _ = sb.Append(values[i, j] ? '#' : ' ');
            _ = sb.AppendLine();
        }
        Write(sb.ToString());
        CursorLeft = 0;
        CursorTop = 0;
        if (KeyAvailable)
        {
            ConsoleKeyInfo info = ReadKey(true);
            if (info.Modifiers == 0 && info.Key == ConsoleKey.R)
                break;
        }
        else if (
                values.GetLength(0) != WindowHeight - 1 ||
                values.GetLength(1) != WindowWidth - 1)
            break;
        int d0 = values.GetLength(0);
        int d1 = values.GetLength(1);
        for (int i = 0; i < d0; i++)
        {
            for (int j = 0; j < d1; j++)
                nextValues[i, j] = Behave(
                    values[Mod(i - 1, d0), Mod(j - 1, d1)],
                    values[Mod(i - 1, d0), j],
                    values[Mod(i - 1, d0), Mod(j + 1, d1)],
                    values[i, Mod(j - 1, d1)],
                    values[i, j],
                    values[i, Mod(j + 1, d1)],
                    values[Mod(i + 1, d0), Mod(j - 1, d1)],
                    values[Mod(i + 1, d0), j],
                    values[Mod(i + 1, d0), Mod(j + 1, d1)]);
            _ = sb.AppendLine();
        }
        (values, nextValues) = (nextValues, values);
        Thread.Sleep(100);
    }
}