using IB.Evaluation.Parsers.Nodes.Base;

namespace IB.Evaluation.Parsers.Nodes
{
    public class UnaryNode<TNumber> : Node<TNumber>
    {
        Node<TNumber> _rightNode;

        Func<TNumber, TNumber> _operation;

        public UnaryNode(Node<TNumber> rightNode, Func<TNumber, TNumber> operation)
        {
            _rightNode = rightNode;
            _operation = operation;
        }

        public override TNumber Eval()
        {
            var rigthVal = _rightNode.Eval();
            var result = _operation(rigthVal);
            return result;
        }
    }

}
