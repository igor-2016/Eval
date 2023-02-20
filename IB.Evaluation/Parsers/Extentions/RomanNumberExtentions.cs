using IB.Evaluation.Parsers.Exceptions;

namespace IB.Evaluation.Parsers.Extentions
{
    public static class RomanNumberExtentions
    {
        public static CalcState Calc(this CalcState state, string pattern, CalcGroup group, double value)
        {
            var patternLength = pattern != null ? pattern.Length : 0;
            var restLength = state.source.Length - state.currentPosition;
            var enought = restLength >= patternLength;

            if (enought && pattern == state.source[state.currentPosition..(state.currentPosition + patternLength)])//  state.source.Substring(state.currentPosition, patternLength))
            {
                if (state.currentGroup == group)
                {
                    throw new InvalidNumberException($"Invalid symbol '{state.source.Substring(state.currentPosition, patternLength)}' in the same group '{group}'");
                }

                return new CalcState(state.source, state.currentPosition + patternLength, group, state.output + value);

            }
            return state;
        }

        public static CalcState Final(this CalcState state)
        {
            if (state.currentPosition < state.source.Length)
                throw new InvalidNumberException($"Invalid symbol(s) '{state.source.Substring(state.currentPosition, state.source.Length - state.currentPosition)}' in the end");

            return state;
        }

        public static double Calc(string romanNumber)
        {
            var start = new CalcState(romanNumber, 0, CalcGroup.Unknown, 0)
                //.Calc("MMMMMMMMMM", CalcGroup.Thousands, 10000)
                //.Calc("MMMMMMMMM", CalcGroup.Thousands, 9000)
                //.Calc("MMMMMMMM", CalcGroup.Thousands, 8000)
                //.Calc("MMMMMMM", CalcGroup.Thousands, 7000)
                //.Calc("MMMMMM", CalcGroup.Thousands, 6000)
                //.Calc("MMMMM", CalcGroup.Thousands, 5000)
                //.Calc("MMMM", CalcGroup.Thousands, 4000)
                .Calc("MMM", CalcGroup.Thousands, 3000)
                .Calc("MM", CalcGroup.Thousands, 2000)
                .Calc("M", CalcGroup.Thousands, 1000)

                .Calc("CM", CalcGroup.Hundreds, 900)
                .Calc("DCCC", CalcGroup.Hundreds, 800)
                .Calc("DCC", CalcGroup.Hundreds, 700)
                .Calc("DC", CalcGroup.Hundreds, 600)
                .Calc("D", CalcGroup.Hundreds, 500)
                .Calc("CD", CalcGroup.Hundreds, 400)
                .Calc("CCC", CalcGroup.Hundreds, 300)
                .Calc("CC", CalcGroup.Hundreds, 200)
                .Calc("C", CalcGroup.Hundreds, 100)

                 .Calc("XC", CalcGroup.Tens, 90)
                 .Calc("LXXX", CalcGroup.Tens, 80)
                 .Calc("LXX", CalcGroup.Tens, 70)
                 .Calc("LX", CalcGroup.Tens, 60)
                 .Calc("L", CalcGroup.Tens, 50)
                 .Calc("XL", CalcGroup.Tens, 40)
                 .Calc("XXX", CalcGroup.Tens, 30)
                 .Calc("XX", CalcGroup.Tens, 20)
                 .Calc("X", CalcGroup.Tens, 10)

                 .Calc("IX", CalcGroup.Ones, 9)
                 .Calc("VIII", CalcGroup.Ones, 8)
                 .Calc("VII", CalcGroup.Ones, 7)
                 .Calc("VI", CalcGroup.Ones, 6)
                 .Calc("V", CalcGroup.Ones, 5)
                 .Calc("IV", CalcGroup.Ones, 4)
                 .Calc("III", CalcGroup.Ones, 3)
                 .Calc("II", CalcGroup.Ones, 2)
                 .Calc("I", CalcGroup.Ones, 1)
                 .Final()

                ;
            return start.output;
        }
    }
}
