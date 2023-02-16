using IB.Evaluation.Parsers.Nodes.Base;

namespace IB.Evaluation.Parsers.Nodes.Roman
{
    public class RomanUnaryNode : UnaryNode<string>
    {
        public RomanUnaryNode(Node<string> rightNode, Func<string, string> operation) : base(rightNode, operation)
        {
        }
    }

}
