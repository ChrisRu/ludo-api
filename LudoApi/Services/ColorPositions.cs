using System;
using System.Collections.Generic;
using System.Linq;
using LudoApi.Models;

namespace LudoApi.Services
{
    public static class ColorPositions
    {
        public const int BoardSize = 40;

        private const int WinningSpots = 4;

        public static int StartPosition(Color color)
        {
            var colorCount = Enum.GetValues(typeof(Color)).Length;

            if ((int)color >= colorCount)
            {
                throw new IndexOutOfRangeException("Unknown color");
            }
            
            return (int) color * (BoardSize / colorCount);
        }

        public static bool OutsideWinningPosition(Color color, int pieceLocation)
        {
            return WinPositions(color).Any(position => pieceLocation > position);
        }

        public static IEnumerable<int> WinPositions(Color color)
        {
            return Enumerable.Range(StartPosition(color) + BoardSize, WinningSpots);
        }
    }
}