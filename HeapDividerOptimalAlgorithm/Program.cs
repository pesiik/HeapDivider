using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeapDividerOptimalAlgorithm
{
    class Program
    {
        private static readonly char[] _delimiters = new char[] { ' ', '\t', '\n', '\r' };
        private static uint _minimalDiffReached;

        static void Main()
        {
            uint[] stoneWeights = GetInput();

            var minimalPossibleDifference = FindMinimalDifference(stoneWeights);

            Console.WriteLine(minimalPossibleDifference);
        }

        private static uint FindMinimalDifference(uint[] stoneWeights)
        {
            _minimalDiffReached = uint.MaxValue;
            Array.Sort<uint>(stoneWeights);

            byte[] leftPileIndexes, rightPileIndexes;
            MakeInitialDivide(stoneWeights, out leftPileIndexes, out rightPileIndexes);
            UpdateMinimalDifference(stoneWeights, leftPileIndexes, rightPileIndexes);

            if (_minimalDiffReached == 0)
            {
                return 0;
            }
            else
            {
                MakeDivides(stoneWeights, leftPileIndexes, rightPileIndexes);
            }

            return _minimalDiffReached;
        }

        private static void MakeInitialDivide(uint[] stoneWeights, out byte[] leftPileIndexes, out byte[] rightPileIndexes)
        {
            var stonesAmount = stoneWeights.Length;

            var leftPileLength = (byte)Math.Ceiling((decimal)stonesAmount / 2);
            leftPileIndexes = new byte[leftPileLength];
            int cellIndex = leftPileLength - 1;
            for (int index = stonesAmount - 1; index >= 0; index -= 2)
            {
                leftPileIndexes[cellIndex] = (byte)index;
                cellIndex--;
            }

            var rightPileLength = stonesAmount - leftPileLength;
            rightPileIndexes = new byte[rightPileLength];
            cellIndex = rightPileLength - 1;
            for (int index = stonesAmount - 2; index >= 0; index -= 2)
            {
                rightPileIndexes[cellIndex] = (byte)index;
                cellIndex--;
            }
        }

        private static void MakeDivides(uint[] stoneWeights, byte[] leftPileIndexes, byte[] rightPileIndexes)
        {
            var indexesToMove = new byte[leftPileIndexes.Length + 1]; // все элементы массива равны 0 по умолчанию
            indexesToMove[0] = 1; // начинаем с перекладывания самого маленького камня        
            var indexesToMoveAmount = leftPileIndexes.Length;

            for (;;)
            {
                var leftPileCurrent = leftPileIndexes
                    .ToList();
                var rightPileCurrent = rightPileIndexes
                    .ToList();

                var indexesToSwing = indexesToMove
                    .Where(index => index != 0)
                    .Select(index => index - 1)
                    .ToArray(); // to remove

                // Перекладывание выбранных камней из кучи в кучу.
                foreach (var indexToSwing in indexesToSwing)
                {
                    var index = leftPileCurrent[indexToSwing];
                    leftPileCurrent.RemoveAt(indexToSwing);

                    rightPileCurrent.Add(index);
                }

                UpdateMinimalDifference(stoneWeights, leftPileCurrent, rightPileCurrent);

                if (_minimalDiffReached == 0 || false)
                {
                    return;
                }

                // Готовим следующий набор.
                bool isSuccessfully = false;
                for (int i = 0; i < indexesToMoveAmount; i++)
                {
                    var digit0 = indexesToMove[i];
                    var digit1 = indexesToMove[i + 1];

                    if (digit1 + 1 < digit0) // можно инкрементить
                    {
                        indexesToMove[i + 1]++;

                        isSuccessfully = true;
                        break;
                    }
                }

                if (!isSuccessfully) // инкрементируем младший разряд
                {
                    if (indexesToMove[0] < indexesToMoveAmount)
                    {
                        indexesToMove[0]++;

                        // Занулили все старшие разряды.
                        for (int i = 1; i < indexesToMove.Length; i++)
                        {
                            indexesToMove[i] = 0;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private static uint[] AssemblePile(uint[] stoneWeights, IEnumerable<byte> pileIndexes)
        {
            var assembledPile = pileIndexes
                .Select(index => stoneWeights[index])
                .ToArray();

            return assembledPile;
        }

        private static void UpdateMinimalDifference(uint[] stoneWeights, IEnumerable<byte> leftPileIndexes, IEnumerable<byte> rightPileIndexes)
        {
            var leftPileAssembled = AssemblePile(stoneWeights, leftPileIndexes);
            var rightPileAssembled = AssemblePile(stoneWeights, rightPileIndexes);

            var difference = GetDifference(leftPileAssembled, rightPileAssembled);
            _minimalDiffReached = (uint)Math.Min(Math.Abs(difference), _minimalDiffReached);
        }

        private static int GetDifference(uint[] leftPile, uint[] rightPile)
        {
            var leftPileSum = leftPile.Sum(stoneWeight => stoneWeight);
            var rightPileSum = rightPile.Sum(stoneWeight => stoneWeight);

            var difference = (int)(leftPileSum - rightPileSum);
            return difference;
        }

        /// <returns>Array of stones weights.</returns>
        private static uint[] GetInput()
        {
            var firstString = Console.ReadLine().Trim(_delimiters);
            var stonesAmount = byte.Parse(firstString);

            var secondString = Console.ReadLine().Trim(_delimiters);
            var stoneWeights = secondString
                .Split(_delimiters)
                .Where(stoneWeight => !string.IsNullOrWhiteSpace(stoneWeight))
                .Take(stonesAmount)
                .Select(stoneWeight => uint.Parse(stoneWeight))
                .ToArray();

            return stoneWeights;
        }
    }
}
