using IB.Evaluation.Calculators;
using IB.Evaluation.Parsers.Base;
using IB.Evaluation.Parsers.Exceptions;
using IB.Evaluation.Parsers.Nodes.Arabic;
using IB.Evaluation.Parsers.Nodes.Base;
using IB.Evaluation.Parsers.Nodes.Roman;
using IB.Evaluation.Tokenizers.Enums;

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
                operation = (a, b) =>  new ArabicNumber( new RomanNumber(a).ToArabic() + new RomanNumber(b).ToArabic()).ToRoman();
            else if (type == TokenTypes.Substraction)
                operation = (a, b) => new ArabicNumber(new RomanNumber(a).ToArabic() - new RomanNumber(b).ToArabic()).ToRoman();
            else if (type == TokenTypes.Multiplication)
                operation = (a, b) => new ArabicNumber(new RomanNumber(a).ToArabic() * new RomanNumber(b).ToArabic()).ToRoman();
            else if (type == TokenTypes.Division)
                operation = (a, b) => new ArabicNumber(new RomanNumber(a).ToArabic() / new RomanNumber(b).ToArabic()).ToRoman();
            else
                throw new InvalidOperationException(type.ToString());

            return new RomanBinaryNode(leftNode, rightNode, operation);
        }
    }

}
