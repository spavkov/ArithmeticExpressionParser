using System;
using NUnit.Framework;

namespace ArithmeticExpressionParser.Tests
{
    [TestFixture]
    public class ParserTests
    {
        [TestCase("0", ExpectedResult = 0)]
        [TestCase("9", ExpectedResult = 9)]
        [TestCase("1.9", ExpectedResult = 1.9)]
        [TestCase(".9", ExpectedResult = .9)]
        [TestCase("-.9", ExpectedResult = -.9)]
        [TestCase("-8", ExpectedResult = -8)]
        public double SingleNumber(string expression)
        {
            var parser = new Parser(expression);
            return parser.Parse();
        }

        [Test]
        [TestCase("(1+2)", ExpectedResult = 3)]
        [TestCase("(((1+2)))", ExpectedResult = 3)]
        [TestCase("(1+2", ExpectedException = typeof(Exception))]
        [TestCase("(7.5+7.5)", ExpectedResult = 15)]
        [TestCase("(7.5-0)", ExpectedResult = 7.5)]
        [TestCase("(1-1)", ExpectedResult = 0)]
        [TestCase("(9-9)", ExpectedResult = 0)]
        [TestCase("(9-5)", ExpectedResult = 4)]
        [TestCase("2*3*2-1", ExpectedResult = 11)]
        [TestCase("2*3*2-1+(4*2*3)", ExpectedResult = 35)]
        [TestCase("100-100+1", ExpectedResult = 1)]
        [TestCase("((9-5)+(2+3))", ExpectedResult = 9)]
        [TestCase("((9-5)+(2+3)+(1+1))", ExpectedResult = 11)]
        [TestCase("(((34-17)*8)+(2*7))", ExpectedResult = 150)]
        [TestCase("(33/3-9)*2", ExpectedResult = 4)]
        [TestCase("20-3*8/2+(28*7)", ExpectedResult = 196)]
        [TestCase("-1", ExpectedResult = -1)]
        [TestCase("-(2+3)", ExpectedResult = -5)]
        [TestCase("-(5-10)", ExpectedResult = 5)]
        [TestCase("(5-10)", ExpectedResult = -5)]
        [TestCase("(", ExpectedException = typeof(Exception))]
        public double MultipleNumbersWithParenthesis(string expression)
        {
            var parser = new Parser(expression);
            return parser.Parse();
        }
    }
}

