namespace IB.Evaluation.Calculators.Settings
{
    public class ArabicCalcSettings
    {
        public char DecimalSeparator { get; }
        
        public char NumberGroupSeparator { get; }

        public ArabicCalcSettings(char decimalSeparator = '.', char numberGroupSeparator = ',')
        {
            DecimalSeparator = decimalSeparator;
            NumberGroupSeparator = numberGroupSeparator;
        }
    }
}
