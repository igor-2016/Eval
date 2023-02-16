using IB.Evaluation.Parsers.Exceptions;
using System.Text.RegularExpressions;

namespace IB.Evaluation.Calculators
{
    public struct RomanNumber
    {
        string _value = "";
        public string Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    value = value.Replace(" ", "");
                    if (!validator.IsMatch(value))
                        throw new InvalidNumberException($"Incorrect roman number {value}");

                    //.. TODO other validations

                    _value = value;
                }
            }
        }

        public static Dictionary<char, int> RomanToArabic = new Dictionary<char, int> 
            { {'I', 1 }, {'V', 5}, {'X', 10}, { 'L', 50}, { 'C', 100}, { 'D', 500}, { 'M', 1000} };

        Regex validator;

        public RomanNumber(string value) : this()
        {
            Value = value;
        }

        public RomanNumber()
        {
            validator = new Regex(@"^[" + string.Join("", RomanToArabic.Keys) + "]*$");
        }

        public double ToArabic()
        {
            if (string.IsNullOrWhiteSpace(Value))
                return 0.0;

            int result = 0;
            int prevValue = 0;

            foreach (var r in Value)
            {
                var currentValue = RomanToArabic[r];
                result += currentValue;
                if (prevValue != 0 && prevValue < currentValue)
                {
                    if (prevValue == 1 && (currentValue == 5 || currentValue == 10)
                        || prevValue == 10 && (currentValue == 50 || currentValue == 100)
                        || prevValue == 100 && (currentValue == 500 || currentValue == 1000))
                        result -= 2 * prevValue;
                    else
                        throw new InvalidNumberException($"Invalid roman number: {Value}");
                }
                prevValue = currentValue;
            }

            return result;
        }
    }
}
