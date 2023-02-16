using IB.Evaluation.Parsers.Nodes.Base;

namespace IB.Evaluation.Parsers.Nodes.Arabic
{
    public class ArabicBinaryNode : BinaryNode<double>
    {
        public ArabicBinaryNode(Node<double> leftNode, Node<double> rightNode, Func<double, double, double> operation)
            : base(leftNode, rightNode, operation)
        {
        }
    }

}
