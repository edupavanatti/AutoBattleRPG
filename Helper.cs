using System;

namespace AutoBattle
{
    /// <summary>
    /// Class dedicated to hold 'helper' methods.
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// Generates a random int between two given values.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>An int ranging between the min and max values.</returns>
        public static int GetRandomInt(int min, int max)
        {
            var rand = new Random();
            int index = rand.Next(min, max);
            return index;
        }
    }
}
