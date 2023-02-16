using IB.Evaluation.Tokenizers.Base;
using IB.Evaluation.Tokenizers.Enums;

namespace IB.Evaluation.Tokenizers.Selectors.Base
{
    public abstract class TokenTypeSelectorBase : ITokenSelector
    {
        private ITokenSelector? _nextSelector;

        public ITokenSelector SetNext(ITokenSelector selector)
        {
            _nextSelector = selector;
            return selector;
        }

        public virtual TokenTypes? Select(ITokenizer tokenizer, char currentChar)
        {
            if (_nextSelector != null)
            {
                return _nextSelector.Select(tokenizer, currentChar);
            }
            else
            {
                return null;
            }
        }
    }

}
