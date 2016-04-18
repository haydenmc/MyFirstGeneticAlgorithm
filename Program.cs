using System;

namespace MyFirstGeneticAlgorithm
{
    public class Program
    {
        public static void Main(string[] args)
        {
            double targetNumber = 0;
            if (args.Length < 1 || !double.TryParse(args[0], out targetNumber))
            {
                Console.WriteLine("Expecting number as first argument.");
                return;
            }
            new Program(targetNumber).Start();
        }
        
        public double TargetNumber { get; private set; }
        
        public Program(double targetNumber)
        {
            TargetNumber = targetNumber;
        }
        
        public void Start()
        {
            Console.WriteLine($"Attempting to find equation that evaluates to {TargetNumber}.");
            Console.WriteLine($"Bits per gene: {Chromosome.BitsPerGene} bits.");
            Console.WriteLine($"Chromosome size: {Chromosome.ChromosomeByteSize} bytes.");
            Console.WriteLine($"Genes per chromosome: {Chromosome.GeneCount} genes.");
            for (var i = 0; i < 5; i++)
            {
                var chromosome = new Chromosome();
                Console.WriteLine($"{chromosome.ToBinaryString()}\n\t{chromosome.ToString()} = {chromosome.Evaluate()}\n\tFitness: {EvaluateFitness(chromosome)}");
            }
        }
        
        private double EvaluateFitness(Chromosome chromosome)
        {
            var chromosomeValue = chromosome.Evaluate();
            var distance = Math.Abs(chromosomeValue - TargetNumber);
            if (distance == 0)
            {
                return 1;
            }
            else
            {
                return 1 / distance;
            }
        }
        
    }
}
