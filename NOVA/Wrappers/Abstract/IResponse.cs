namespace NOVA.Wrappers.Abstract
{
    public interface IResponse
    {
        string Message { get; }
        bool IsSuccess { get; }
    }
}
