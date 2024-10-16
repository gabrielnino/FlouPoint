﻿namespace Application.UseCases.Repository.CRUD.Validation
{
    using Application.Result;

    /// <summary>
    /// Defines the contract for validating the existence of an entity in the repository.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public interface IEntityExistenceValidator<T>
    {
        /// <summary>
        /// Checks if an entity with the given ID exists in the repository.
        /// </summary>
        /// <param name="id">The ID of the entity to check.</param>
        /// <returns>An <see cref="OperationResult{T}"/> containing the entity if it exists, or null if it does not.</returns>
        Task<OperationResult<T>> HasEntity(string id);
    }
}
