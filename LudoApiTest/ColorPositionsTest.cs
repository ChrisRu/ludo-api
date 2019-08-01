using System;
using LudoApi.Models;
using LudoApi.Services;
using Xunit;

namespace LudoApiTest
{
    public class ColorPositionsTest
    {
        [Fact]
        public void GetStartPositionBlue()
        {
            const Color color = Color.Blue;

            var startPosition = ColorPositions.StartPosition(color);

            Assert.Equal(10, startPosition);
        }

        [Fact]
        public void GetStartPositionGreen()
        {
            const Color color = Color.Green;

            var startPosition = ColorPositions.StartPosition(color);

            Assert.Equal(30, startPosition);
        }

        [Fact]
        public void GetStartPositionRed()
        {
            const Color color = Color.Red;

            var startPosition = ColorPositions.StartPosition(color);

            Assert.Equal(0, startPosition);
        }

        [Fact]
        public void GetStartPositionUnknown()
        {
            const Color color = (Color) 5;

            Assert.Throws<IndexOutOfRangeException>(() => ColorPositions.StartPosition(color));
        }

        [Fact]
        public void GetStartPositionYellow()
        {
            const Color color = Color.Yellow;

            var startPosition = ColorPositions.StartPosition(color);

            Assert.Equal(20, startPosition);
        }

        [Fact(Skip = "Not yet implemented")]
        public void GetWinningPositions()
        {
            const Color color = Color.Blue;

            var winPositions = ColorPositions.WinPositions(color);

            Assert.Empty(winPositions);
        }
    }
}