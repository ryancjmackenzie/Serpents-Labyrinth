using System;
using System.Collections.Generic;

public class Grid
{
    private int gridSize;
    private char[][] grid;

    private Player[] players;

    public Grid(int size)
    {
        gridSize = size;
        grid = new char[gridSize][];
        
        for (int i = 0; i < gridSize; i++)
        {
            grid[i] = new char[gridSize];
        }

        players = new Player[4]; // Assuming a maximum of 4 players
    }
}