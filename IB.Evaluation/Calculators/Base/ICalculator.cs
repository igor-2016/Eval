namespace IB.Evaluation.Calculators.Base
{
    public interface ICalculator<TNumber>
    {
        TNumber Evaluate(string input);
    }

}
