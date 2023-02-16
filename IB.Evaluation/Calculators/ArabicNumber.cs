using IB.Evaluation.Parsers.Exceptions;
using System.Text;

namespace IB.Evaluation.Calculators
{
    public struct ArabicNumber
    {
        double _value = 0;
        public double Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = (int)value;
                }
            }
        }

        Dictionary<int, string> arabicToRoman = new Dictionary<int, string> 
            { { 1, "I"}, {4, "IV"}, {5, "V"}, {9, "IX"}, {10, "X"}, { 40, "XL"}, { 50, "L"}, { 90, "XC"}, 
            { 100, "C"}, { 400, "CD" },{ 500, "D" }, { 900, "CM"}, { 1000, "M"} };

        public ArabicNumber(double value) : this()
        {
            Value = value;
        }

        public ArabicNumber()
        {
        }

        public string ToRoman()
        {
            if (Value == 0)
                return "";

            if (Value < 0)
                throw new InvalidNumberException("Roman number can not be negative");

            if (Value > 4000)
                throw new InvalidNumberException("Roman number more 4000 is not supported");


            var value = (int)Value;
            
            var result = new StringBuilder();
            while (value > 0)
            {
                foreach (var key in arabicToRoman.Keys.Reverse())
                {
                    if (value >= key)
                    {
                        value -= key;
                        result.Append(arabicToRoman[key]);
                        break;
                    }
                }
            }
            return result.ToString();
        }
    }
}
