using System;
using System.Collections.Generic;

public class Player
{
    public string Name { get; private set; }
    public int Score { get; private set; }

    public Player(string name)
    {
        Name = name;
        Score = 0;
    }

    public void AddScore(int points)
    {
        Score += points;
    }
}