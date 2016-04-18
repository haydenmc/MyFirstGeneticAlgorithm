using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFirstGeneticAlgorithm
{
    public class Chromosome
    {
        /// <summary>
        /// The minimum number of genes in each chromosome (will be increased to pad out to byte-even)
        /// </summary>
        public const int MinChromosomeLength = 8;
        
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
        /// Returns the actual gene count per chromosome
        /// </summary>
        public static int GeneCount
        {
            get
            {
                return (ChromosomeByteSize * 8) / BitsPerGene;
            }
        }
        
        /// <summary>
        /// Gene make up of the chromosome
        /// </summary>
        private byte[] _genes { get; set; }
        
        public int[] Genes
        {
            get
            {
                int[] genes = new int[GeneCount];
                for (var i = 0; i < GeneCount; i++)
                {
                    int geneIndex = 0;
                    int byteIndex = (i * BitsPerGene) / 8;
                    for (var j = 0; j < Math.Ceiling(BitsPerGene / 8.0); j++)
                    {
                        byte geneByte = _genes[byteIndex + j];
                        byte geneOffset = (byte) ((i * BitsPerGene) % 8);
                        if (8 / BitsPerGene > 0)
                        {
                            byte geneSegment = geneByte;
                            geneSegment = (byte) (geneSegment >> (8 - (geneOffset + BitsPerGene)));
                            geneSegment = (byte) (geneSegment & ((1 << BitsPerGene) - 1));
                            geneByte = geneSegment;
                        }
                        geneIndex = geneIndex | ((int) geneByte << ((int) (Math.Ceiling(BitsPerGene / 8.0)) - (j + 1))* BitsPerGene);
                    }
                    genes[i] = geneIndex;
                }
                return genes;
            }
        }
        
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
            return string.Join(" ", _genes.Select(g => Convert.ToString(g, 2).PadLeft(8, '0')));
        }
        
        /// <summary>
        /// Returns a decoded string representation of the chromosome
        /// </summary>
        override public string ToString()
        {
            return string.Join(" ", Genes.Select(g => g < GeneValues.Length ? GeneValues[g] : ' '));
        }
        
        /// <summary>
        /// Evaluate the 'equation' represented by this chromosome
        /// </summary>
        /// <returns>The resulting double</returns>
        public double Evaluate()
        {
            var genes = Genes;
            var stack = new Stack<double>();
            for (var i = 0; i < genes.Length; i++)
            {
                var gene = genes[i];
                if (gene >= GeneValues.Length)
                {
                    continue;
                }
                if (GeneValues[gene] == '+' || GeneValues[gene] == '-' || GeneValues[gene] == '*' || GeneValues[gene] == '/')
                {
                    if (stack.Count < 2)
                    {
                        continue; // Not enough operands
                    }
                    double result = 0;
                    double right = stack.Pop();
                    double left = stack.Pop();
                    switch (GeneValues[gene])
                    {
                        case '+':
                            result = left + right;
                            break;
                        case '-':
                            result = left - right;
                            break;
                        case '*':
                            result = left * right;
                            break;
                        case '/':
                            result = left / right; // TODO: Catch divide by zero
                            break;
                    }
                    stack.Push(result);
                }
                else
                {
                    stack.Push(char.GetNumericValue(GeneValues[gene]));
                }
            }
            return stack.Skip(stack.Count - 1).FirstOrDefault();
        }
    }
}