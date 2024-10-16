﻿namespace Infrastructure.Utilities
{
    using System; // This is required for Guid.TryParse method.
    using global::Application.Result;

    /// <summary>
    /// Provides utility functions to validate GUIDs (Globally Unique Identifiers).
    /// </summary>
    public class GuidValidator
    {
        /// <summary>
        /// Validates if the provided string is a valid GUID.
        /// </summary>
        /// <param name="id">The string representation of a GUID to validate.</param>
        /// <returns>
        /// An <see cref="OperationResult{T}"/> which indicates the result of the validation.
        /// If the validation is successful, it contains the valid GUID as a string.
        /// If the validation fails, it contains an error message.
        /// </returns>
        public static OperationResult<string> HasGuid(string id)
        {
            // Try to parse the provided string to check if it's a valid GUID.
            var resultConversion = Guid.TryParse(id, out _);

            // If it's not a valid GUID, return a failure result.
            if (!resultConversion)
            {
                return OperationBuilder<string>.FailureBusinessValidation(Resource.FailedIntegerConversion); // This error message seems mismatched. Shouldn't it be related to a GUID conversion failure?
            }

            // If it's a valid GUID, return a success result.
            return OperationResult<string>.Success(id, Resource.GlobalOkMessage);
        }
    }
}
