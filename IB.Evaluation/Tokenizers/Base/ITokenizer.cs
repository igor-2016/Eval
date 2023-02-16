namespace IB.Evaluation.Tokenizers.Base
{
    public interface ITokenizer
    {
        char MoveNextChar();

        void MoveNextToken();
    }
}
