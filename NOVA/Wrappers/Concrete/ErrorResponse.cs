using NOVA.Wrappers.Abstract;
using System;

namespace NOVA.Wrappers.Concrete
{
    public class ErrorResponse : IResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; } = false;

        public ErrorResponse(string Message)
        {
            this.Message = Message;
        }

        public ErrorResponse(Exception Ex)
        {
            Message = Ex.Message;
        }
    }
}