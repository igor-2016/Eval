namespace IB.Evaluation.Parsers.Extentions
{
    public record CalcState(string source, int currentPosition, CalcGroup currentGroup, double output);
}
