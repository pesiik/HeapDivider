using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HeapDividerOptimalAlgorithm
{
    public class HeapDivider
    {
        private int _countOfStones;

        private int _leftLastIndex;
        private int _rightLastIndex;

        private int[] _allIndexes;
        private int[] _weights;

        private List<int> _rightIndexes, _leftIndexes;

        public HeapDivider(int countOfStones)
        {
            _countOfStones = countOfStones;
            _allIndexes = new int[countOfStones];

            _rightIndexes = new List<int>();
            _leftIndexes = new List<int>();

            _weights = new int[countOfStones];
        }


        public void DivisionDescendingOrder()
        {
            List<int> sorted = _weights.OrderBy(i =>i).ToList();
            for (int i = sorted.Count-2; i >= 0; i -= 2)
            {
                _rightIndexes.Add(i);
            }
            for (int i = sorted.Count-1; i >= 0; i-= 2)
            {
                _leftIndexes.Add(i);
            }
            _weights = sorted.ToArray();
        }

        private int GetCellValue(int index)
        {
            var cellValue = _allIndexes[index];
            return cellValue;
        }

        public void FillingWeightsArray(string[] weight)
        {
            for (int i = 0; i < _weights.Length; i++)
            {
                _weights[i] = int.Parse(weight[i]);
            }
        }

        public int ComprasionDifferenceOfHeaps()
        {
            int difference = Int32.MaxValue, tempDifferent = 0, firstDifference = 0;
            int leftSumm = 0, rightSumm = 0;
            int i = 0;
            int rightExtremeIndex = _leftIndexes.Min(), left;

            for (;;)
            {
                i++;
                foreach (int index in _leftIndexes)
                {
                    leftSumm += _weights[index];
                }
                foreach (int index in _rightIndexes)
                {
                    rightSumm += _weights[index];
                }
                tempDifferent = leftSumm - rightSumm;
                leftSumm = 0;
                rightSumm = 0;                
                if (Math.Abs(difference) >= (Math.Abs(tempDifferent)))
                {
                    difference = tempDifferent;
                }
                
                if (tempDifferent > 0)
                {
                    rightExtremeIndex = _leftIndexes.Min();
                    _rightIndexes.Add(rightExtremeIndex);
                    _leftIndexes.Remove(rightExtremeIndex);
                }
                else if (tempDifferent < 0)
                {
                    rightExtremeIndex = _rightIndexes.Min();
                    _leftIndexes.Add(rightExtremeIndex);
                    _rightIndexes.Remove(rightExtremeIndex);
                }
                else
                {
                    difference = tempDifferent;
                    return difference;
                }

                if (i >= _countOfStones * 2) // todo
                    return Math.Abs(difference);
            }
        }

    }
}