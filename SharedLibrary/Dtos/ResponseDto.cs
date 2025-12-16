using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos
{
    public class ResponseDto<T> where T : class
    {
        public T Data { get;private set; }
        public int StatusCode { get; private set; }
        public string Message { get; private set; }
        [JsonIgnore]
        public bool IsSuccessful { get; private set; }
        public ErrorDto Error { get; private set; }
        public static ResponseDto<T> Success(T data, int statusCode, string? message = null)
        {
          return new ResponseDto<T> { Data = data, StatusCode = statusCode,IsSuccessful = true, Message = message };
        }
        public static ResponseDto<T> Success(int statusCode)
        { 
            return new ResponseDto<T> { Data = default, IsSuccessful = true, StatusCode = statusCode };
        }
        public static ResponseDto<T> Fail(ErrorDto error, int statusCode, string? message = null)
        {
            return new ResponseDto<T> { Error = error, StatusCode = statusCode,IsSuccessful = false, Message = message };
        }

        public static ResponseDto<T> Fail(string error,bool isShow, int statusCode, string? message = null)
        {
            return new ResponseDto<T> { Error = new ErrorDto(error,isShow), StatusCode = statusCode, IsSuccessful = false, Message = message };
        }
    }
}
