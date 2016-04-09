using System;
using System.Linq;

namespace MyFirstGeneticAlgorithm
{
    public class Chromosome
    {
        /// <summary>
        /// The minimum number of genes in each chromosome (will be increased to pad out to byte-even)
        /// </summary>
        public const int MinChromosomeLength = 6;
        
        /// <summary>
        /// Random instance
        /// </summary>
        private static Random _random = new Random();
        
        /// <summary>
        /// Set of valid gene values
        /// </summary>
        public static readonly char[] GeneValues
            = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '-', '*', '/' };
        
        /// <summary>
        /// How many bits are required to encode a gene to binary
        /// </summary>
        public static int BitsPerGene
        {
            get
            {
                // 2^n = GeneValues count.
                // n will tell us how many bits we need to encode each gene value.
                var result = Math.Ceiling(Math.Log(GeneValues.Length) / Math.Log(2));
                return (int) result;
            }
        }
        
        /// <summary>
        /// The size in bytes of a single chromosome
        /// </summary>
        public static int ChromosomeByteSize
        {
            get
            {
                var bits = MinChromosomeLength * BitsPerGene;
                while (bits % 8 != 0)
                {
                    bits += BitsPerGene;
                }
                return (int) (bits / 8);
            }
        }
        
        /// <summary>
        /// Gene make up of the chromosome
        /// </summary>
        private byte[] _genes { get; set; }
        
        public Chromosome()
        {
            // random chromosome
            _genes = new byte[ChromosomeByteSize];
            _random.NextBytes(_genes);
        }
        
        /// <summary>
        /// Returns a binary string representation of the chromosome
        /// </summary>
        public string ToBinaryString()
        {
            return string.Join("", _genes.Select(g => Convert.ToString(g, 2).PadLeft(8, '0')));
        }
        
        /// <summary>
        /// Returns a decoded string representation of the chromosome
        /// </summary>
        override public string ToString()
        {
            byte[] currentBuffer = new byte[(BitsPerGene / 8) + 1]; 
            int currentByte = 0;
            while (true)
            {
                int resultIndex = 0;
                for (var i = 0; i < currentBuffer.Length; i++)
                {
                    currentBuffer[i] = _genes[currentByte + i];
                }
            }
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Evaluate the 'equation' represented by this chromosome
        /// </summary>
        /// <returns>The resulting double</returns>
        public double Evaluate()
        {
            throw new NotImplementedException();
            // double? result = null;
            // for (var i = 0; i < _genes.Length; i++)
            // {
            //     // If we're an operator, apply operation
            //     if (_genes[i].IsOperator)
            //     {
            //         // Ensure there is a number beyond this operator we can actually use
            //         // And ensure we have something stored for the left side of the operation
            //         if (result != null && (i + 1) < _genes.Length && !_genes[i + 1].IsOperator)
            //         {
            //             // Apply the operation
            //             switch (_genes[i].Value)
            //             {
            //                 case '+':
            //                     result = result + _genes[i + 1].Value;
            //                     break;
            //                 case '-':
            //                     result = result - _genes[i + 1].Value;
            //                     break;
            //                 case '*':
            //                     result = result * _genes[i + 1].Value;
            //                     break;
            //                 case '/':
            //                     result = result / _genes[i + 1].Value; // TODO: Catch divide by zero
            //                     break;
            //             }
            //             // Skip over number
            //             i++;
            //             continue;
            //         }
            //     }
            //     else
            //     {
            //         // If we have nothing stored, grab the value
            //         if (result == null)
            //         {
            //             result = _genes[i].Value;
            //         }
            //     }
            // }
            // return result == null ? 0 : (double)result;
        }
    }
}