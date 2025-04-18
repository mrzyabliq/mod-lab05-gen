using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace generator
{
    public class CharGenerator
    {
        private List<string> syms = new List<string>();
        private List<int> weights = new List<int>();
        private List<int> upper_bounds = new List<int>();
        public int summ;
        private Random random = new Random();
        public CharGenerator(string filename)
        {
            using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, filename)))
            {
                string input;
                string[] line;
                Int32 sum = 0;
                while ((input = reader.ReadLine()) != null)
                {
                    line = input.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    int i = 0;
                    syms.Add(line[i + 1]);
                    weights.Add(Int32.Parse(line[2]));
                    sum += Int32.Parse(line[2]);
                    upper_bounds.Add(sum);
                }
                summ = sum;
            }
        }
        public string getSym()
        {
            var ch = random.Next(0, summ);
            for (int i = 0; i < upper_bounds.Count; i++)
            {
                if (ch <= upper_bounds[i])
                {
                    return syms[i];
                }
            }
            return "";
        }
        public int getSize()
        {
            return syms.Count;
        }
        public int getSymWeight(string sym) => weights[syms.FindIndex(x => x == sym)];
    }
    public class WordGenerator
    {
        private List<string> syms = new List<string>();
        private List<Double> weights = new List<Double>();
        private List<Double> upper_bounds = new List<Double>();
        public Double summ;
        private Random random = new Random();
        public WordGenerator(string filename)
        {
            using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, filename)))
            {
                string input;
                string[] line;
                Double sum = 0;
                while ((input = reader.ReadLine()) != null)
                {
                    line = input.Replace('.', ',').Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    syms.Add(line[1]);
                    weights.Add(Double.Parse(line[4]));
                    sum += Double.Parse(line[4]);
                    upper_bounds.Add(sum);
                }
                summ = sum;
            }
        }
        public string getSym()
        {
            var ch = random.Next(0, (Int32)summ);
            for (int i = 0; i < upper_bounds.Count; i++)
            {
                if (ch <= upper_bounds[i])
                {
                    return syms[i];
                }
            }
            return "";
        }
        public int getSize()
        {
            return syms.Count;
        }
        public Double getSymWeight(string sym) => weights[syms.FindIndex(x => x == sym)];
    }
    class Program
    {
        static void Main(string[] args)
        {
            CharGenerator gen = new CharGenerator("bigrammweights.txt");
            SortedDictionary<string, int> stat = new SortedDictionary<string, int>();
            string result = "";
            for (int i = 0; i < 1000; i++)
            {
                string ch = gen.getSym();
                result += ch;
                if (stat.ContainsKey(ch))
                    stat[ch]++;
                else
                    stat.Add(ch, 1); Console.Write(ch);
            }
            Console.Write('\n');
            using (StreamWriter reader = new StreamWriter(Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "../Results/gen-1.txt"), false, Encoding.UTF8))
            {
                reader.WriteLine(result);
            }
            WordGenerator genWord = new WordGenerator("wordweights.txt");
            SortedDictionary<string, int> statWord = new SortedDictionary<string, int>();
            result = "";
            for (int i = 0; i < 1000; i++)
            {
                string ch = genWord.getSym();
                result += ch + " ";
                if (statWord.ContainsKey(ch))
                    statWord[ch]++;
                else
                    statWord.Add(ch, 1); Console.Write(ch + " ");
            }
            Console.Write('\n');
            using (StreamWriter reader = new StreamWriter(Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "../Results/gen-2.txt"), false, Encoding.UTF8))
            {
                reader.WriteLine(result);
            }

            using (StreamWriter reader = new StreamWriter(Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "../Results/graph_bi_data.txt"), true, Encoding.UTF8))
            {
                foreach (KeyValuePair<string, int> entry in stat) reader.WriteLine(entry.Key+" "+(entry.Value / 1000.0).ToString()+" " +((Double)gen.getSymWeight(entry.Key) / gen.summ).ToString());
            }


            using (StreamWriter reader = new StreamWriter(Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "../Results/graph_word_data.txt"), true, Encoding.UTF8))
            {
                foreach (KeyValuePair<string, int> entry in statWord) reader.WriteLine(entry.Key+" "+(entry.Value / 1000.0).ToString()+" "+((Double)genWord.getSymWeight(entry.Key) / genWord.summ).ToString());
            }
        }
    }
}

