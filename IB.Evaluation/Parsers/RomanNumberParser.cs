using IB.Evaluation.Parsers.Base;
using IB.Evaluation.Parsers.Exceptions;
using IB.Evaluation.Parsers.Extentions;
using IB.Evaluation.Parsers.Nodes.Base;
using IB.Evaluation.Parsers.Nodes.Roman;
using IB.Evaluation.Tokenizers.Enums;
using IB.Evaluation.Validators;

namespace IB.Evaluation.Parsers
{
    public class RomanNumberParser : Parser<string>
    {
        public RomanNumberParser(IEnumerable<(TokenTypes Type, string? Number)> tokens) : base(tokens)
        {
        }

        protected override RomanNumberNode CreateNumberNode(string? number)
        {
            if (number == null)
                throw new InvalidNumberException("Number is null");

            return new RomanNumberNode(number);
        }

        protected override Node<string> CreateZeroNumberNode()
        {
            return new RomanZeroNumberNode();
        }

        protected override Node<string> CreateUnaryNode(Node<string> rightNode)
        {
            return new RomanUnaryNode(rightNode, (a) =>
            {
                if (a.Length > 0)
                {
                    if (a[0] == '-')
                    {
                        return a.Remove(0);
                    }
                    else
                    {
                        return '-' + a;
                    }
                }
                return a;
            });
        }

        protected override Node<string> CreateBinaryNode(Node<string> leftNode, Node<string> rightNode, TokenTypes type)
        {
            Func<string, string, string> operation;

            if (type == TokenTypes.Addition)
                operation = (a, b) => ArabicNumberParser.ToRoman(new RomanNumber(a).ToArabic() + new RomanNumber(b).ToArabic());
            else if (type == TokenTypes.Substraction)
                operation = (a, b) => ArabicNumberParser.ToRoman(new RomanNumber(a).ToArabic() - new RomanNumber(b).ToArabic());
            else if (type == TokenTypes.Multiplication)
                operation = (a, b) => ArabicNumberParser.ToRoman(new RomanNumber(a).ToArabic() * new RomanNumber(b).ToArabic());
            else if (type == TokenTypes.Division)
                operation = (a, b) => ArabicNumberParser.ToRoman(new RomanNumber(a).ToArabic() / new RomanNumber(b).ToArabic());
            else
                throw new InvalidOperationException(type.ToString());

            return new RomanBinaryNode(leftNode, rightNode, operation);
        }

        public static double ToArabic(string input)
        {
            RomanNumberValidator.IsValid(ref input);
            return new RomanNumber(input).ToArabic();
        }

        private struct RomanNumber
        {
            public string Value { get; }

            public RomanNumber(string value)
            {
                if (!RomanNumberValidator.IsValid(ref value))
                    throw new InvalidNumberException($"Incorrect roman number {value}");

                Value = value;
            }

            public double ToArabic()
            {
                if (string.IsNullOrWhiteSpace(Value))
                    return 0.0;

                return (int)RomanNumberExtentions.Calc(Value);
            }

            public double ToArabicWithoutValidation()
            {
                if (string.IsNullOrWhiteSpace(Value))
                    return 0.0;

                int result = 0;
                int prevValue = 0;

                foreach (var r in Value)
                {
                    var currentValue = RomanNumberValidator.RomanToArabic[r];
                    result += currentValue;
                    if (prevValue != 0 && prevValue < currentValue)
                    {
                        if (prevValue == 1 && (currentValue == 5 || currentValue == 10)
                            || prevValue == 10 && (currentValue == 50 || currentValue == 100)
                            || prevValue == 100 && (currentValue == 500 || currentValue == 1000))
                            result -= 2 * prevValue;
                        else
                            throw new InvalidNumberException($"Invalid roman number: {Value}");
                    }
                    prevValue = currentValue;
                }

                return result;
            }
        }

        
        

        
    }

}
