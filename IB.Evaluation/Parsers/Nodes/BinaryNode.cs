using IB.Evaluation.Parsers.Nodes.Base;

namespace IB.Evaluation.Parsers.Nodes
{
    public class BinaryNode<TNumber> : Node<TNumber>
    {
        Node<TNumber> _leftNode;
        Node<TNumber> _rightNode;
        Func<TNumber, TNumber, TNumber> _operation;

        public BinaryNode(Node<TNumber> leftNode, Node<TNumber> rightNode, Func<TNumber, TNumber, TNumber> operation)
        {
            _leftNode = leftNode;
            _rightNode = rightNode;
            _operation = operation;
        }

        public override TNumber Eval()
        {
            var leftVal = _leftNode.Eval();
            var rightVal = _rightNode.Eval();

            var result = _operation(leftVal, rightVal);
            return result;
        }
    }

}
