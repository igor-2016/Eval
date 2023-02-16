namespace IB.Evaluation.Parsers.Nodes.Base
{
    public abstract class Node<TNumber> : INode
    {
        public abstract TNumber Eval();
    }

}
