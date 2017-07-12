using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeapDividerOptimalAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            int countOfStones = int.Parse(Console.ReadLine());
            
            HeapDivider heapDivider = new HeapDivider(countOfStones);

            string[] weightsString = Console.ReadLine().Split(' ');
            heapDivider.FillingWeightsArray(weightsString);
            heapDivider.DivisionDescendingOrder();
            int difference = heapDivider.ComprasionDifferenceOfHeaps();
            Console.WriteLine(difference);
        }
    }
}
