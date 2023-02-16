using IB.Evaluation.Tokenizers.Enums;
using IB.Evaluation.Tokenizers.Exceptions;
using IB.Evaluation.Tokenizers.Selectors.Base;

namespace IB.Evaluation.Tokenizers.Base
{
    public abstract class Tokenizer<TNumber> : ITokenizer
    {
        public TokenTypes CurrentToken { get; protected set; }

        public TNumber? Number { get; set; }

        protected TextReader Reader { get; }

        char _currentChar;

        protected ITokenSelector Selectors { get; set; }

        public IList<(TokenTypes Type, TNumber? Number)> Tokens { get; } = 
            new List<(TokenTypes Type, TNumber? Number)>();


        protected Tokenizer(TextReader reader, ITokenSelector selectors)
        {
            Selectors = selectors;
            Reader = reader;
            MoveNextChar();
            MoveNextToken();
        }

        public virtual char MoveNextChar()
        {
            int ch = Reader.Read();
            return _currentChar = ch < 0 ? '\0' : (char)ch;
        }

        public virtual void MoveNextToken()
        {
            var token = Selectors.Select(this, _currentChar);

            if (token == null)
                throw new UnknownCharException($"Unknown character: {_currentChar}");

            CurrentToken = token.Value;

            Tokens.Add((CurrentToken, token.Value == TokenTypes.Number ?  Number : default));
        }
    }

}
