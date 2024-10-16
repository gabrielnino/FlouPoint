﻿namespace Infrastructure.Utilities
{
    using Application.Result;
    using Application.UseCases.ExternalServices;
    using Application.UseCases.Utilities;
    using Domain.DTO.Log;
    using Domain.EnumType.OperationExecute;
    using Infrastructure.Other;

    /// <summary>
    /// Provides functionality for managing images, including interacting with external services like Google Drive.
    /// </summary>
    public class ManagementImage : IManagementImage
    {
        // Constant representing the user for upload operations.
        private const string UserUpload = "AuthFlowServicesUser";

        // Service responsible for logging activities and errors.
        private readonly ILogService _logService;

        // Service responsible for image compression operations.
        private readonly IImageCompressionService _imageCompressionService;

        /// <summary>
        /// Initializes a new instance of the ManagementImage class.
        /// </summary>
        /// <param name="logService">Service responsible for logging operations.</param>
        /// <param name="imageCompressionService">Service responsible for image compression.</param>
        public ManagementImage(ILogService logService, IImageCompressionService imageCompressionService)
        {
            _logService = logService;
            _imageCompressionService = imageCompressionService;
        }

        /// <summary>
        /// Asynchronously uploads a given image, represented as a base64 string, after converting and compressing it.
        /// </summary>
        /// <param name="base64String">The base64 representation of the image.</param>
        /// <param name="filename">The name of the file being uploaded.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<OperationResult<bool>> UploadAsync(string base64String, string filename)
        {
            try
            {
                var resultStream = await ConvertBase64ToStream(base64String);
                if (!resultStream.IsSuccessful)
                {
                    return resultStream.ToResultWithBoolType();
                }

                var stream = resultStream.Data ?? new MemoryStream();
                var resultCompress = await _imageCompressionService.CompressImage(stream);
                if (!resultCompress.IsSuccessful)
                {
                    return resultCompress.ToResultWithBoolType();
                }

                Stream streamCompress = resultCompress.Data ?? new MemoryStream();
                // Upload operation commented out; possibly uploads to Google Drive or another location.
                // var result = UploadFileAsync(streamCompress);
                return OperationResult<bool>.Success(true, Resource.SuccessfullyUpload);
            }
            catch (Exception ex)
            {
                Log log = Util.GetLogError(ex, filename, OperationExecute.Validate);
                OperationResult<string> result = await _logService.CreateLog(log);
                if (!result.IsSuccessful)
                {
                    return OperationBuilder<bool>.FailureExtenalService(Resource.FailedUpload);
                }

                return OperationBuilder<bool>.FailureExtenalService(Resource.FailedUpload);
            }
        }

        /// <summary>
        /// Converts a base64 string to a memory stream.
        /// </summary>
        /// <param name="base64String">The base64 representation of the data.</param>
        /// <returns>A result containing the converted stream or an error.</returns>
        private async Task<OperationResult<Stream>> ConvertBase64ToStream(string base64String)
        {
            try
            {
                if (base64String == null)
                {
                    throw new Exception("The parameter is null.");
                }

                // Removing the base64 prefix if it exists.
                if (base64String.Contains(','))
                {
                    base64String = base64String.Split(',')[1];
                }

                byte[] bytes = Convert.FromBase64String(base64String);
                MemoryStream memoryStream = new MemoryStream(bytes);
                return OperationResult<Stream>.Success(memoryStream, Resource.GlobalOkMessage);
            }
            catch (Exception ex)
            {
                Log log = Util.GetLogError(ex, base64String, OperationExecute.Validate);
                OperationResult<string> result = await _logService.CreateLog(log);
                if (!result.IsSuccessful)
                {
                    return OperationBuilder<Stream>.FailureExtenalService(Resource.FailedUpload);
                }

                return OperationBuilder<Stream>.FailureExtenalService(Resource.FailedUpload);
            }
        }
    }
}
