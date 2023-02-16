using IB.Evaluation.Parsers.Nodes.Base;

namespace IB.Evaluation.Parsers.Nodes.Roman
{
    public class RomanBinaryNode : BinaryNode<string>
    {
        public RomanBinaryNode(Node<string> leftNode, Node<string> rightNode, Func<string, string, string> operation)
            : base(leftNode, rightNode, operation)
        {
        }
    }

}
