using IB.Evaluation.Parsers.Exceptions;
using IB.Evaluation.Parsers.Nodes.Base;
using IB.Evaluation.Tokenizers.Enums;

namespace IB.Evaluation.Parsers.Base
{
    public abstract class Parser<TNumber>
    {
        protected (TokenTypes Type, TNumber? Number)[] Tokens { get; }

        protected (TokenTypes Type, TNumber? Number) CurrentToken;

        int currentPosition = -1;

        public Parser(IEnumerable<(TokenTypes Type, TNumber? Number)> tokens)
        {
            if (tokens == null)
                throw new InvalidOperationException($"{nameof(tokens)} is null");

            Tokens = tokens.ToArray();

            if (Tokens.Length > 0)
                MoveNext();
        }

        public void MoveNext()
        {
            currentPosition++;
            if (currentPosition >= Tokens.Length)
                throw new InvalidOperationException($"{nameof(currentPosition)} {currentPosition} is out of range {Tokens.Length}");

            CurrentToken = Tokens[currentPosition];
        }

        public Node<TNumber> Parse()
        {
            Node<TNumber> expression;
            if (CurrentToken.Type != TokenTypes.EndOfFile)
                expression = ParseAddAndSubtract();
            else
                expression = CreateZeroNumberNode();

            if (CurrentToken.Type != TokenTypes.EndOfFile)
                throw new SyntaxException("Unknown characters at the end of expression");

            return expression;
        }

        protected Node<TNumber> ParseLeaf()
        {

            if (CurrentToken.Type == TokenTypes.Number)
            {
                var node = CreateNumberNode(CurrentToken.Number);

                MoveNext();

                return node;
            }


            if (CurrentToken.Type == TokenTypes.OpenParenthese)
            {
                MoveNext();

                var node = ParseAddAndSubtract();

                if (CurrentToken.Type != TokenTypes.CloseParenthese)
                    throw new SyntaxException("Closed parenthese not found!");

                MoveNext();

                return node;
            }

            throw new SyntaxException($"Unknown token type: {CurrentToken.Type}");
        }

        protected Node<TNumber> ParseAddAndSubtract()
        {
            var leftNode = ParseMultiplyAndDivide();

            while (true)
            {

                if (CurrentToken.Type != TokenTypes.Addition && CurrentToken.Type != TokenTypes.Substraction)
                    return leftNode;

                var tokenType = CurrentToken.Type;

                MoveNext();

                var rightNode = ParseMultiplyAndDivide();

                leftNode = CreateBinaryNode(leftNode, rightNode, tokenType);
            }
        }

        protected Node<TNumber> ParseMultiplyAndDivide()
        {
            var leftNode = ParseUnary();

            while (true)
            {
                if (CurrentToken.Type != TokenTypes.Multiplication && CurrentToken.Type != TokenTypes.Division)
                    return leftNode;

                var tokenType = CurrentToken.Type;

                MoveNext();

                var rightNode = ParseUnary();

                leftNode = CreateBinaryNode(leftNode, rightNode, tokenType);
            }
        }

        protected Node<TNumber> ParseUnary()
        {
            if (CurrentToken.Type == TokenTypes.Addition)
            {
                MoveNext();

                return ParseUnary();
            }

            if (CurrentToken.Type == TokenTypes.Multiplication)
            {
                MoveNext();

                return ParseUnary();
            }

            if (CurrentToken.Type == TokenTypes.Division)
            {
                MoveNext();

                return ParseUnary();
            }

            if (CurrentToken.Type == TokenTypes.Substraction)
            {

                MoveNext();

                var rightNode = ParseUnary();

                return CreateUnaryNode(rightNode);
            }

            return ParseLeaf();
        }

        protected abstract Node<TNumber> CreateZeroNumberNode();

        protected abstract Node<TNumber> CreateNumberNode(TNumber? number);

        protected abstract Node<TNumber> CreateUnaryNode(Node<TNumber> rightNode);

        protected abstract Node<TNumber> CreateBinaryNode(Node<TNumber> leftNode, Node<TNumber> rightNode, TokenTypes tokenType);
    }

}
