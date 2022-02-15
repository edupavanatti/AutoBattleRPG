using System;
using System.Collections.Generic;
using System.Text;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Grid
    {
        public List<GridBox> grids = new List<GridBox>();
        // Fixed a typo with this property name.
        public int xLength;
        public int yLength;
        StringBuilder currentBattlefield;

        public Grid(int Lines, int Columns)
        {
            xLength = Lines;
            yLength = Columns;
            Console.WriteLine("\nThe battle field has been created!\n");

            for (int i = 0; i < Lines; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    // Removed the 'occupied' attribute from the constructor as it always starts false.
                    GridBox newBox = new GridBox(j, i, Columns * i + j);
                    grids.Add(newBox);
                }
            }
        }

        // Prints the matrix that indicates the tiles of the battlefield.
        // Changed this method so that it only redraws the battlefield when the characters move or when a character dies.
        public void drawBattlefield(bool shouldRedraw)
        {
            if (shouldRedraw)
            {
                currentBattlefield = new StringBuilder();
                int currentIndex = 0;

                // Changed the battlefield to be drawn based on the defined X and Y values.
                for (int i = 0; i < xLength; i++)
                {
                    for (int j = 0; j < yLength; j++)
                    {
                        GridBox currentgrid = grids[currentIndex];
                        currentIndex++;

                        if (currentgrid.ocupied)
                        {
                            if (currentgrid.isOcupiedByPlayer)
                            {
                                currentBattlefield.Append("[O]\t");
                            }
                            else
                            {
                                currentBattlefield.Append("[X]\t"); ;
                            }
                        }
                        else
                        {
                            currentBattlefield.Append("[ ]\t");
                        }
                    }

                    currentBattlefield.Append("\n\n");
                }
            }

            Console.Write(currentBattlefield);
        }
    }
}
