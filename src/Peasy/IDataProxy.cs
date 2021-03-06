﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peasy
{
    /// <summary>
    /// Represents a data abstraction for consumption by services, commands, and rules.
    /// </summary>
    public interface IDataProxy<T, TKey> :
        ISupportGetAll<T>,
        ISupportGetByID<T, TKey>,
        ISupportInsert<T>,
        ISupportUpdate<T>,
        ISupportDelete<TKey>
    {
    }

    /// <summary>
    /// Represents a data abstraction that retrieves all domain objects or resources from a source.
    /// </summary>
    public interface ISupportGetAll<T>
    {
        /// <summary>
        /// Retrieves all domain objects or resources from a source.
        /// </summary>
        /// <returns>An awaitable list of resources.</returns>
        Task<IEnumerable<T>> GetAllAsync();
    }

    /// <summary>
    /// Represents a data abstraction that retrieves a domain object or resource from a source specified by an identifier.
    /// </summary>
    public interface ISupportGetByID<T, TKey>
    {
        /// <summary>
        /// Retrieves a domain object or resource from a source specified by an identifier.
        /// </summary>
        /// <param name="id">The id of the resource to retrieve.</param>
        /// <returns>An awaitable resource.</returns>
        Task<T> GetByIDAsync(TKey id);
    }

    /// <summary>
    /// Represents a data abstraction that creates a new domain object or resource.
    /// </summary>
    public interface ISupportInsert<T>
    {
        /// <summary>
        /// Creates a new domain object or resource.
        /// </summary>
        /// <param name="resource">The resource to insert.</param>
        /// <returns>
        /// An awaitable updated representation of the created domain object or resource.
        /// </returns>
        Task<T> InsertAsync(T resource);
    }

    /// <summary>
    /// Represents a data abstraction that updates an existing domain object or resource.
    /// </summary>
    public interface ISupportUpdate<T>
    {
        /// <summary>
        /// Updates an existing domain object or resource.
        /// </summary>
        /// <param name="resource">The resource to update.</param>
        /// <returns>
        /// An awaitable updated representation of the updated domain object or resource.
        /// </returns>
        Task<T> UpdateAsync(T resource);
    }

    /// <summary>
    /// Represents a data abstraction that deletes a domain object or resource from a source specified by an identifier.
    /// </summary>
    public interface ISupportDelete<TKey>
    {
        /// <summary>
        /// Deletes a domain object or resource from a source specified by an identifier.
        /// </summary>
        /// <param name="id">The id of the resource to delete.</param>
        /// <returns>An awaitable task.</returns>
        Task DeleteAsync(TKey id);
    }
}
