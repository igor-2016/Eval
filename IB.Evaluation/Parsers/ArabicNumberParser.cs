using IB.Evaluation.Calculators.Settings;
using IB.Evaluation.Parsers.Base;
using IB.Evaluation.Parsers.Nodes.Arabic;
using IB.Evaluation.Parsers.Nodes.Base;
using IB.Evaluation.Tokenizers.Enums;

namespace IB.Evaluation.Parsers
{
    public class ArabicNumberParser : Parser<double>
    {
        public ArabicNumberParser(IEnumerable<(TokenTypes Type, double Number)> tokens) : base(tokens)
        {
        }

        protected override Node<double> CreateNumberNode(double number)
        {
            return new ArabicNumberNode(number);
        }

        protected override Node<double> CreateZeroNumberNode()
        {
            return new ArabicZeroNumberNode();
        }

        protected override Node<double> CreateUnaryNode(Node<double> rightNode)
        {
            return new ArabicUnaryNode(rightNode, (a) => -a);
        }

        protected override Node<double> CreateBinaryNode(Node<double> leftNode,
            Node<double> rightNode, TokenTypes type)
        {
            Func<double, double, double> operation;

            if (type == TokenTypes.Addition)
                operation = (a, b) => a + b;
            else if (type == TokenTypes.Substraction)
                operation = (a, b) => a - b;
            else if (type == TokenTypes.Multiplication)
                operation = (a, b) => a * b;
            else if (type == TokenTypes.Division)
                operation = (a, b) => a / b;
            else
                throw new InvalidOperationException(type.ToString());

            return new ArabicBinaryNode(leftNode, rightNode, operation);
        }
    }

}
