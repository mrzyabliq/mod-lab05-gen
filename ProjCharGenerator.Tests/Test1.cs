using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using generator;

namespace GeneratorTests
{
    [TestClass]
    public class CombinedGeneratorTests
    {
        [TestMethod]
        public void CharGen_Test1()
        {
            var gen = new CharGenerator("bigrammweights.txt");
            Assert.AreEqual(3, gen.getSize());
        }

        [TestMethod]
        public void CharGen_Test2()
        {
            var gen = new CharGenerator("bigrammweights.txt");
            var valid = new HashSet<string> { "aa", "bb", "cc" };
            
            for (int i = 0; i < 100; i++)
            {
                Assert.IsTrue(valid.Contains(gen.getSym()));
            }
        }

        [TestMethod]
        public void CharGen_Test3()
        {
            var gen = new CharGenerator("bigrammweights.txt");
            var stats = new Dictionary<string, int>();
            
            for (int i = 0; i < 10000; i++)
            {
                var sym = gen.getSym();
                if (stats.ContainsKey(sym))
                    stats[sym]++;
                else
                    stats.Add(sym, 1);
            }
            
            Assert.IsTrue(stats["aa"] < stats["bb"]);
            Assert.IsTrue(stats["bb"] < stats["cc"]);
        }

        [TestMethod]
        public void CharGen_Test4()
        {
            var gen = new CharGenerator("empty_file.txt"); 
            Assert.AreEqual(0, gen.getSize());
        }

        [TestMethod]
        public void CharGen_Test5()
        {
            var gen = new CharGenerator("empty_file.txt");
            Assert.AreEqual("", gen.getSym());
        }

        [TestMethod]
        public void CharGen_Test6()
        {
            File.WriteAllText("zero.txt", "1 tt 0.0");
            var gen = new CharGenerator("zero.txt");
            Assert.AreEqual("", gen.getSym());
        }
        [TestMethod]
        public void WordGen_Test7()
        {
            var gen = new WordGenerator("wordweights.txt");
            Assert.AreEqual(3, gen.getSize());
        }

        [TestMethod]
        public void WordGen_Test8()
        {
            var gen = new WordGenerator("wordweights.txt");
            var valid = new HashSet<string> { "word1", "word2", "word3" };
            
            for (int i = 0; i < 100; i++)
            {
                Assert.IsTrue(valid.Contains(gen.getSym()));
            }
        }

        [TestMethod]
        public void WordGen_Test9()
        {
            var gen = new WordGenerator("wordweights.txt");
            var stats = new Dictionary<string, int>();
            
            for (int i = 0; i < 10000; i++)
            {
                var sym = gen.getSym();
                if (stats.ContainsKey(sym))
                    stats[sym]++;
                else
                    stats.Add(sym, 1);
            }
            
            Assert.IsTrue(stats["word1"] > stats["word2"]);
            Assert.IsTrue(stats["word2"] > stats["word3"]);
        }

        [TestMethod]
        public void WordGen_Test10()
        {
            File.WriteAllText(Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "zero_word.txt"), "1 testword aa aa 0.0");
            var gen = new WordGenerator("zero.txt");
            Assert.AreEqual("", gen.getSym());
        }

        [TestMethod]
        public void WordGen_Test11()
        {
            File.WriteAllText(Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "unicode.txt"), "1 слово aa aa 5.0", Encoding.UTF8);
            var gen = new WordGenerator("unicode.txt");
            Assert.AreEqual("слово", gen.getSym());
        }

        [TestMethod]
        public void WordGen_Test12()
        {
            new WordGenerator("empty_file.txt");
        }
    }
}