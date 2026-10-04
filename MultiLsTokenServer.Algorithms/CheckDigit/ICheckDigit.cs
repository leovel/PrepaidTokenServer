namespace MultiLsTokenServer.Algorithms.CheckDigit;

public interface ICheckDigit
{
    string Calculate(string code);
    bool IsValid(string code);
}
