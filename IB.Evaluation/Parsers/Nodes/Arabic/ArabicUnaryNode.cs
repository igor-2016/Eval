using IB.Evaluation.Parsers.Nodes.Base;

namespace IB.Evaluation.Parsers.Nodes.Arabic
{
    public class ArabicUnaryNode : UnaryNode<double>
    {
        public ArabicUnaryNode(Node<double> rightNode, Func<double, double> operation) : base(rightNode, operation)
        {
        }
    }

}
