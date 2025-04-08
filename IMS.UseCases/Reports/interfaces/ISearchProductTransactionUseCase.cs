﻿using IMS.CoreBusiness;

namespace IMS.UseCases.Reports.interfaces
{
    public interface ISearchProductTransactionUseCase
    {
        Task<IEnumerable<ProductTransaction>> ExecuteAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? transactionType);
    }
}