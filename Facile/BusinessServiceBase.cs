﻿using Facile.Core;
using Facile.Exception;
using Facile.Extensions;
using Facile.Rules;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Facile
{
    /// <summary>
    /// Base class of all business services
    /// </summary>
    public abstract class BusinessServiceBase<T, TKey> : ServiceBase<T, TKey> where T : IDomainObject<TKey>, new()
    {
        public BusinessServiceBase(IServiceDataProxy<T, TKey> dataProxy) : base(dataProxy)
        {
        }

        /// <summary>
        /// Supplies validation results to DeleteCommand()
        /// </summary>
        protected override IEnumerable<ValidationResult> GetValidationResultsForDelete(TKey id)
        {
            var rule = id.CreateValueRequiredRule("id").Validate();
            if (!rule.IsValid)
                yield return new ValidationResult(rule.ErrorMessage, new string[] { typeof(T).Name });
        }
        
        /// <summary>
        /// Supplies validation results to GetByIDCommand()
        /// </summary>
        protected override IEnumerable<ValidationResult> GetValidationResultsForGetByID(TKey id)
        {
            var rule = id.CreateValueRequiredRule("id").Validate();
            if (!rule.IsValid)
                yield return new ValidationResult(rule.ErrorMessage, new string[] { typeof(T).Name });
        }

        /// <summary>
        /// Invoked by UpdateCommand() if validation and business rules execute successfully
        /// </summary>
        /// <exception cref="Facile.Exception.DomainObjectNotFoundException" />
        /// <exception cref="Facile.Exception.ConcurrencyException" />
        protected override T Update(T entity)
        {
            // only perform this if we're not latency prone - keep it close to the server, no need to do this more than once
            if (!this.IsLatencyProne)
            {
                T current = GetByID(entity.ID);
                if (current == null)
                {
                    throw new DomainObjectNotFoundException(BuildNotFoundError(entity.ID));
                }
                if (current is IVersionContainer)
                {
                    var rule = new ConcurrencyCheckRule(current as IVersionContainer, entity as IVersionContainer).Validate();
                    if (!rule.IsValid)
                        throw new ConcurrencyException(rule.ErrorMessage);

                }
                entity.RevertNonEditableValues(current);
                entity.RevertForeignKeysFromZeroToNull();
            }
            return _dataProxy.Update(entity);
        }

        /// <summary>
        /// Invoked by UpdateCommand() if validation and business rules execute successfully
        /// </summary>
        protected override async Task<T> UpdateAsync(T entity)
        {
            // only perform this if we're not latency prone - keep it close to the server, no need to do this more than once
            if (!this.IsLatencyProne)
            {
                T current = await GetByIDAsync(entity.ID);
                if (current == null)
                {
                    throw new DomainObjectNotFoundException(BuildNotFoundError(entity.ID));
                }
                if (current is IVersionContainer)
                {
                    var rule = new ConcurrencyCheckRule(current as IVersionContainer, entity as IVersionContainer).Validate();
                    if (!rule.IsValid)
                        throw new ConcurrencyException(rule.ErrorMessage);

                }
                entity.RevertNonEditableValues(current);
                entity.RevertForeignKeysFromZeroToNull();
            }

            return await _dataProxy.UpdateAsync(entity);
        }

        public bool SupportsTransactions
        {
            get { return (_dataProxy as IServiceDataProxy<T, TKey>).SupportsTransactions; }
        }

        public bool IsLatencyProne
        {
            get { return (_dataProxy as IServiceDataProxy<T, TKey>).IsLatencyProne; }
        }

        public string BuildNotFoundError(TKey id)
        {            
            var message = string.Format("{0} ID {1} could not be found.", new T().ClassName(), id.ToString());
            return message;
        }
    }
}
