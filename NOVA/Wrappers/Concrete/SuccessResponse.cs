using NOVA.Wrappers.Abstract;

namespace NOVA.Wrappers.Concrete
{
    public class SuccessResponse<T> : IResponse where T : class
    {
        public T Value { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; } = false;

        public SuccessResponse(T Value)
        {
            this.Value = Value;
        }

        public SuccessResponse(T Value, string Message) 
        {
            this.Value = Value;
            this.Message = Message;
        }
    }
}