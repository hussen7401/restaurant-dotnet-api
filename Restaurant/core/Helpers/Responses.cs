using core.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace core.Helpers
{
    public class Responses
    {
        public ActionResult ResponseSuccess<T>(string message, T data)
        {
            var responseSuccess = new ResponseDto<T>
            {
                Status = "Success",
                Code = 200,
                Message = message,
                Data = data
            };
            return new ObjectResult(responseSuccess) { StatusCode = 200 };
        } 
        public ActionResult ResponseSuccess(string message)
        {
            var responseSuccess = new ResponseDto<string>
            {
                Status = "Success",
                Code = 200,
                Message = message,
            };
            return new ObjectResult(responseSuccess) { StatusCode = 200 };
        }
        public ActionResult ResponseNotFound(string message)
        {
            var responseNotFound = new ResponseDto<string>
            {
                Status = "Not Found",
                Code = 404,
                Message = message,
                Data = null
            };
            return new ObjectResult(responseNotFound) { StatusCode = 404 };
        }
        public ActionResult ResponseBadRequest(string message)
        {
            var responseBadRequest = new ResponseDto<string>
            {
                Status = "Bad Request",
                Code = 400,
                Message = message,
                Data = null
            };
            return new ObjectResult(responseBadRequest) { StatusCode = 400 };
        }
        public ActionResult ResponseConflict(string message)
        {
            var responseConflict = new ResponseDto<string>
            {
                Status = "Conflict",
                Code = 409,
                Message = message,
                Data = null
            };
            return new ObjectResult(responseConflict) { StatusCode = 409 };
        }
        public ActionResult ResponseUnauthorized(string message)
        {
            var responseUnauthorized = new ResponseDto<string>
            {
                Status = "Unauthorized",
                Code = 401,
                Message = message,
                Data = null
            };
            return new ObjectResult(responseUnauthorized) { StatusCode = 401 };
        }
        public ActionResult ResponseUnavailable(string message)
        {
            var ResponseUnavailable = new ResponseDto<string>
            {
                Status = "Unavailable",
                Code = 503,
                Message = message,
                Data = null
            };
            return new ObjectResult(ResponseUnavailable) { StatusCode = 503 };
        }
        public ActionResult ResponseForbidden(string message)
        {
            var responseForbidden = new ResponseDto<string>
            {
                Status = "Forbidden",
                Code = 403,
                Message = message,
                Data = null
            };
            return new ObjectResult(responseForbidden) { StatusCode = 403 };
        }
        public ActionResult HandleException(Exception ex)
        {
            if (ex is DbUpdateException)
            {
                Console.WriteLine($"Database update exception: {ex.Message}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
            }
            else
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
            }
            var errorResponse = new ResponseDto<string>
            {
                Status = "Error",
                Code = 500,
                Message = "حدث خطأ ما!",
                Data = ex.Message
            };

            return new ObjectResult(errorResponse) { StatusCode = 500 };
        }
    }
}

