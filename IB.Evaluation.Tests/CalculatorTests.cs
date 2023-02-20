using IB.Evaluation.Calculators;
using IB.Evaluation.Calculators.Settings;
using IB.Evaluation.Parsers;
using IB.Evaluation.Parsers.Exceptions;
using IB.Evaluation.Parsers.Nodes.Arabic;
using IB.Evaluation.Tokenizers;
using IB.Evaluation.Tokenizers.Enums;
using IB.Evaluation.Tokenizers.Selectors;
using IB.Evaluation.Validators;

namespace IB.Evaluation.Tests
{
    public class CalculatorTests
    {
        [Theory]
        [InlineData("", new TokenTypes[] { TokenTypes.EndOfFile }, new string[] { })]
        [InlineData(" ", new TokenTypes[] { TokenTypes.EndOfFile }, new string[] { })]
        [InlineData("()", new TokenTypes[] { TokenTypes.OpenParenthese, TokenTypes.CloseParenthese, TokenTypes.EndOfFile }, new string[] { })]
        [InlineData("   (  )", new TokenTypes[] { TokenTypes.OpenParenthese, TokenTypes.CloseParenthese, TokenTypes.EndOfFile }, new string[] { })]
        [InlineData("   ((  ))", new TokenTypes[] { TokenTypes.OpenParenthese, TokenTypes.OpenParenthese, TokenTypes.CloseParenthese, TokenTypes.CloseParenthese, TokenTypes.EndOfFile }, new string[] { })]
        [InlineData("(X + X)", new TokenTypes[] { TokenTypes.OpenParenthese, TokenTypes.Number, TokenTypes.Addition, TokenTypes.Number, TokenTypes.CloseParenthese, TokenTypes.EndOfFile }, new string[] { "X", "X" })]
        [InlineData("(X + X) - X", new TokenTypes[] { TokenTypes.OpenParenthese, TokenTypes.Number, TokenTypes.Addition, TokenTypes.Number, TokenTypes.CloseParenthese, TokenTypes.Substraction, TokenTypes.Number, TokenTypes.EndOfFile }, new string[] { "X", "X", "X" })]
        [InlineData("(MMMDCCXXIV - MMCCXXIX) * II", new TokenTypes[] { TokenTypes.OpenParenthese, TokenTypes.Number, TokenTypes.Substraction, TokenTypes.Number, TokenTypes.CloseParenthese, TokenTypes.Multiplication, TokenTypes.Number, TokenTypes.EndOfFile }, new string[] { "MMMDCCXXIV", "MMCCXXIX", "II" })]
        public void Can_Tokenize_Roman_Numbers(string input, TokenTypes[] tokenTypes, string[] expectedNumbers, bool hasException = false)
        {
            using (var reader = new StringReader(input))
            {
                Stack<char> parentheseChecker = new();

                var selectors = new EndFileSelector();
                selectors
                    .SetNext(new WhiteSpaceSelector())
                    .SetNext(new EndFileSelector())
                    .SetNext(new ParenthesesSelector(parentheseChecker))
                    .SetNext(new RomanNumbersSelector())
                    .SetNext(new MathOpertionsSelector())
                    .SetNext(new OpenParentheseChecker(parentheseChecker));

                var tokenizer = new RomanNumbersTokenizer(reader, selectors);

                var currentNumberIndex = 0;
                for (var i = 0; i < tokenTypes.Length; i++)
                {
                    var expectedTokenType = tokenTypes[i];
                    var currentTokenType = tokenizer.CurrentToken;

                    Assert.Equal(expectedTokenType, currentTokenType);
                    if (currentTokenType == TokenTypes.Number)
                    {
                        Assert.Equal(expectedNumbers[currentNumberIndex], tokenizer.Number);
                        currentNumberIndex++;
                    }

                    tokenizer.MoveNextToken();
                }
            }
        }

        [Theory]
        [InlineData("(10 + 20)", new TokenTypes[] { TokenTypes.OpenParenthese, TokenTypes.Number, TokenTypes.Addition, TokenTypes.Number, TokenTypes.CloseParenthese, TokenTypes.EndOfFile }, new double[] { 10, 20 })]
        public void Can_Tokenize_Arabic_Numbers(string input, TokenTypes[] tokenTypes, double[] expectedNumbers, bool hasException = false)
        {
            using (var reader = new StringReader(input))
            {
                Stack<char> parentheseChecker = new();

                var settings = new ArabicCalcSettings();

                var selectors = new EndFileSelector();
                selectors
                    .SetNext(new WhiteSpaceSelector())
                    .SetNext(new EndFileSelector())
                    .SetNext(new ParenthesesSelector(parentheseChecker))
                    .SetNext(new ArabicNumbersSelector(settings))
                    .SetNext(new MathOpertionsSelector())
                    .SetNext(new OpenParentheseChecker(parentheseChecker));

                var tokenizer = new ArabicNumbersTokenizer(reader, selectors);

                var currentNumberIndex = 0;
                for (var i = 0; i < tokenTypes.Length; i++)
                {
                    var expectedTokenType = tokenTypes[i];
                    var currentTokenType = tokenizer.CurrentToken;

                    Assert.Equal(expectedTokenType, currentTokenType);
                    if (currentTokenType == TokenTypes.Number)
                    {
                        Assert.Equal(expectedNumbers[currentNumberIndex], tokenizer.Number);
                        currentNumberIndex++;
                    }

                    tokenizer.MoveNextToken();
                }
            }
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(5.0, 15, 20)]
        [InlineData(5.0, 0, 5.0)]
        public void Can_Add_Two_ArabicNodes(double number1, double number2, double expected)
        {
            var expression = new ArabicBinaryNode(new ArabicNumberNode(number1), new ArabicNumberNode(number2), (n1, n2) => n1 + n2);
            var result = expression.Eval();
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(5.0, 15, -10)]
        [InlineData(15.4, 0.1, 15.3)]
        public void Can_Substract_Two_ArabicNodes(double number1, double number2, double expected)
        {
            var expression = new ArabicBinaryNode(new ArabicNumberNode(number1), new ArabicNumberNode(number2), (n1, n2) => n1 - n2);
            var result = expression.Eval();
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("10+20", 30)]
        [InlineData("10-30", -20)]
        [InlineData("10-0.00", 10.0)]
        [InlineData("10-1", 9)]
        [InlineData("0.00 + 0.00", 0.0)]
        [InlineData("   1   + 7  ", 8)]
        [InlineData("2 + 3 * 2", 8)]
        [InlineData("(2 + 3) * 2", 10)]
        [InlineData("4 + 2 * (2 + 3) * 3 + 2", 36)]
        [InlineData("4 +-3", 1)]
        [InlineData("4 --3", 7)]
        [InlineData("4 - 2 * (-10)", 24)]
        [InlineData("20 / 10 / 2", 1)]
        [InlineData("5 / 2 ", 2.5)]
        [InlineData("5 / 2 * 2 ", 5.0)]
        [InlineData("(2 + 1 + 1", 3, true, typeof(SyntaxException))]
        [InlineData("5 * ((2 + 5) + 1 * 4)", 55)]
        [InlineData("5 * ****** 6", 30)]
        [InlineData("10 //////// 5", 2)]
        [InlineData("10 -----+ 2", 8)]
        [InlineData("10 ------+ 2", 12)]
        [InlineData("10 + (-2)", 8)]
        [InlineData("-10", -10)]
        [InlineData("-10-30-(50-10)", -80)]
        [InlineData("", 0)]
        [InlineData("-0", 0)]
        [InlineData("()+1", 1, true, typeof(SyntaxException))]
        [InlineData("(1)+1", 2)]
        [InlineData("+1+1+1+1+1+1+1+1+1+1+1+1+1+1+1", 15)]
        //[InlineData("1,000,000.01", 1000000.01)] // TODO
        public void Can_Calculate_Arabic_Calculator(string input, double expected, bool hasException = false, Type? exceptionType = null)
        {
            double result = 0;

            if (!hasException)
            {
                result = new ArabicNumbersCalculator(new ArabicCalcSettings()).Evaluate(input);
                Assert.Equal(expected, result);
            }
            else
            {
                Assert.Throws(exceptionType, () => new ArabicNumbersCalculator(new ArabicCalcSettings()).Evaluate(input));
            }
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("X+X", "XX")]
        [InlineData("X+I", "XI")]
        [InlineData("(MMMDCCXXIV - MMCCXXIX) * II", "MMCMXC")]
        [InlineData("(MMMDCCXXIV - MMCCXXIX) * II / I", "MMCMXC")]
        public void Can_Calculate_Roman_Calculator(string input, string expected, bool hasException = false, Type? exceptionType = null)
        {
            string result = "";

            if (!hasException)
            {
                result = new RomanNumbersCalculator().Evaluate(input);
                Assert.Equal(expected, result);
            }
            else
            {
                Assert.Throws(exceptionType, () => new RomanNumbersCalculator().Evaluate(input));
            }
        }


        [Theory]
        [InlineData("X", 10)]
        [InlineData("", 0)]
        [InlineData("X2", 0, true, typeof(InvalidNumberException))]
        [InlineData("X X", 20)]
        [InlineData("IX", 9)]
        [InlineData("VX", 0, true, typeof(InvalidNumberException))]
        [InlineData("VXI", 0, true, typeof(InvalidNumberException))]
        [InlineData("LXXXIX", 89)]
        [InlineData("LXXXVIII", 88)]
        [InlineData("MMMDCCXXIV", 3724)] 
        [InlineData("MMCCXXIX", 2229)]
        [InlineData("II", 2)] 
        [InlineData("MMCMXC", 2990)]
        [InlineData("mMCMXC", 2990)]
        [InlineData("  m  MCMXC", 2990)]
        [InlineData("IXI", 10, true, typeof(InvalidNumberException))] // wrong
        [InlineData("MMDCCXXIV", 2724)]
        public void Can_Convert_Roman_To_Arabic(string input, double expected, bool hasException = false, Type? exceptionType = null)
        {
            double result = 0;

            if (!hasException)
            {
                result = RomanNumberParser.ToArabic(input);
                Assert.Equal(expected, result);
            }
            else
            {
                Assert.Throws(exceptionType, () => RomanNumberParser.ToArabic(input));
            }
        }

        [Theory]
        [InlineData(10, "X")]
        [InlineData(0, "")]
        [InlineData(20, "XX")]
        [InlineData(9, "IX")]
        [InlineData(89, "LXXXIX")]
        [InlineData(88, "LXXXVIII")]
        [InlineData(3724, "MMMDCCXXIV")]
        [InlineData(2229, "MMCCXXIX")]
        [InlineData(2, "II")]
        [InlineData(2990, "MMCMXC")]
        [InlineData(4001, "", true, typeof(InvalidNumberException))]
        [InlineData(-1, "", true, typeof(InvalidNumberException))]
        public void Can_Convert_Arabic_To_Roman(double input, string expected, bool hasException = false, Type? exceptionType = null)
        {
            string result = "";

            if (!hasException)
            {
                result = ArabicNumberParser.ToRoman(input);
                Assert.Equal(expected, result);
            }
            else
            {
                Assert.Throws(exceptionType, () => ArabicNumberParser.ToRoman(input));
            }
        }


        [Theory]
        [InlineData("X", "X")]
        [InlineData("", "")]
        [InlineData(" ", "")]
        [InlineData(" x ", "X")]
        [InlineData(" A ", "A", true)]
        public void RomanNumberIsValid(string number, string correctedNumber, bool hasException = false)
        {
            var result = RomanNumberValidator.IsValid(ref number);
            
            Assert.Equal(!hasException, result);
            if(!hasException)
                Assert.Equal(correctedNumber, number);
        }
    }
}