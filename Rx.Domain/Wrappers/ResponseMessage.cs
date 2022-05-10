namespace Rx.Domain.Wrappers;

public class ResponseMessage<T>: ResponseMessage{
    public T? Payload { get; set; }= default;
    
    public static ResponseMessage<T> Success(T payload){
        return new ResponseMessage<T>{
            Succeeded = true,
            Payload = payload
        };
    }
    public static ResponseMessage<T> Success(string message,T payload){
        return new ResponseMessage<T>{
            Succeeded = true,
            Message = message,
            Payload = payload
        };
    }
}

public class ResponseMessage
{
    public bool Succeeded { get; set; }
    public string Message { get; set; } = string.Empty;

    public static ResponseMessage Fail(string message)
    {
        return new ResponseMessage
        {
            Succeeded = false,
            Message = message
        };
    }
    public static ResponseMessage Success()
    {
        return new ResponseMessage
        {
            Succeeded = true
        };

    }
    
}