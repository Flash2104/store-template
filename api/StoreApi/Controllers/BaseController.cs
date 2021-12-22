using System.ComponentModel.DataAnnotations;
using Store.Service.Common;
using Store.Service.Exceptions;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Models;

namespace StoreApi.Controllers
{
    public class RootController : ControllerBase
    {
        private readonly ILogger _logger;

        public RootController(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected async Task<ServerResponseDto<TResponseDto>> HandleRequest<TRequestDto, TServiceRequest, TServiceResponse, TResponseDto>(
            Func<TServiceRequest, Task<TServiceResponse>> serviceFunction,
            TRequestDto requestDto,
            Func<TRequestDto, TServiceRequest> requestMap,
            Func<TServiceResponse, TResponseDto> responseMap,
            string logPath)// where TRequestDto : IValidatableObject
        {
            _logger.Log(LogLevel.Trace, $"{logPath} started.");
            try
            {
                //var validationResults = new List<ValidationResult>();
                //if (!Validator.TryValidateObject(requestDto, new ValidationContext(requestDto), validationResults) || validationResults.Count > 0)
                //{
                //    var message = "";
                //    if (validationResults.Count > 0)
                //    {
                //        message = string.Join("\r\n", validationResults.Select(vr => vr.ErrorMessage));
                //    }
                //    throw new AirSoftBaseException(ErrorCodes.InvalidParameters, "Invalid Parameters. \r\n" + message);
                //}
                var serviceRequest = requestMap(requestDto);
                var serviceResponse = await serviceFunction(serviceRequest);
                var responseDto = responseMap(serviceResponse);
                _logger.Log(LogLevel.Trace, $"{logPath} ended.");
                return new ServerResponseDto<TResponseDto>(responseDto);
            }
            catch (AirSoftBaseException baseEx)
            {
                _logger.LogError(baseEx, $"{baseEx.LogPath}.");
                return new ServerResponseDto<TResponseDto>(new ErrorDto(baseEx.Code, baseEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{logPath} Common Exception.");
                return new ServerResponseDto<TResponseDto>(new ErrorDto(ErrorCodes.CommonError, ex.Message));
            }
        }

        protected async Task<ServerResponseDto<TResponseDto>> HandleGetRequest<TServiceRequest, TServiceResponse, TResponseDto>(
            Func<TServiceRequest, Task<TServiceResponse>> serviceFunction,
            Func<TServiceRequest> requestMap,
            Func<TServiceResponse, TResponseDto> responseMap,
            string logPath)
        {
            _logger.Log(LogLevel.Trace, $"{logPath} started.");
            try
            {
                var serviceRequest = requestMap();
                var serviceResponse = await serviceFunction(serviceRequest);
                var responseDto = responseMap(serviceResponse);
                _logger.Log(LogLevel.Trace, $"{logPath} ended.");
                return new ServerResponseDto<TResponseDto>(responseDto);
            }
            catch (AirSoftBaseException baseEx)
            {
                _logger.LogError(baseEx, $"{baseEx.LogPath}.");
                return new ServerResponseDto<TResponseDto>(new ErrorDto(baseEx.Code, baseEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{logPath} Common Exception.");
                return new ServerResponseDto<TResponseDto>(new ErrorDto(ErrorCodes.CommonError, ex.Message));
            }
        }

        protected async Task<ServerResponseDto<TResponseDto>> HandleRequest<TServiceResponse, TResponseDto>(
            Func<Task<TServiceResponse>> serviceFunction,
            Func<TServiceResponse, TResponseDto> responseMap,
            string logPath)
        {
            _logger.Log(LogLevel.Trace, $"{logPath} started.");
            try
            {
                var serviceResponse = await serviceFunction();
                var responseDto = responseMap(serviceResponse);
                _logger.Log(LogLevel.Trace, $"{logPath} ended.");
                return new ServerResponseDto<TResponseDto>(responseDto);
            }
            catch (AirSoftBaseException baseEx)
            {
                _logger.LogError(baseEx, $"{baseEx.LogPath}.");
                return new ServerResponseDto<TResponseDto>(new ErrorDto(baseEx.Code, baseEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{logPath} Common Exception.");
                return new ServerResponseDto<TResponseDto>(new ErrorDto(ErrorCodes.CommonError, ex.Message));
            }
        }
    }
}
