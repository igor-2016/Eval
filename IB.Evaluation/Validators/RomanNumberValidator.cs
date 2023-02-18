using System.Text.RegularExpressions;

namespace IB.Evaluation.Validators
{
    public static class RomanNumberValidator
    {
        public static Dictionary<char, int> RomanToArabic = new Dictionary<char, int>
            { {'I', 1 }, {'V', 5}, {'X', 10}, { 'L', 50}, { 'C', 100}, { 'D', 500}, { 'M', 1000} };

        private static Regex _validator = new Regex(@"^[" + string.Join("", RomanToArabic.Keys) + "]*$");


        private static void Fix(ref string romanNumber)
        {
            romanNumber = romanNumber.Replace(" ", "").ToUpper();
        }

        public static bool IsValid(ref string romanNumber)
        {
            Fix(ref romanNumber);
            return _validator.IsMatch(romanNumber);
        }
    }
}
