using IB.Evaluation.Parsers.Exceptions;

namespace IB.Evaluation.Validators
{
    public static class ArabicNumberValidator
    {
        public static Dictionary<int, string> ArabicToRoman = new Dictionary<int, string>
            {{ 1000, "M"}, { 900, "CM"}, { 500, "D" }, { 400, "CD" }, { 100, "C"}, { 90, "XC"}, { 50, "L"}, { 40, "XL"}, {10, "X"}, 
            {9, "IX"},  {5, "V"}, {4, "IV"}, { 1, "I"}};

        public static void Fix(ref double arabicNumber)
        {
            if (arabicNumber < 0)
                throw new InvalidNumberException("Roman number can not be negative");

            if (arabicNumber > 4000)
                throw new InvalidNumberException("Roman number more 4000 is not supported");


            arabicNumber = (int)arabicNumber;
        }

       
    }
}
