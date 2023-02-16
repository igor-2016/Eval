using IB.Evaluation.Parsers.Nodes.Base;

namespace IB.Evaluation.Parsers.Nodes
{
    public abstract class NumberNode<TNumber> : Node<TNumber>
    {
        protected TNumber Number;

        public NumberNode(TNumber number)
        {
            Number = number;
        }

        public override TNumber Eval()
        {
            return Number;
        }
    }

}
