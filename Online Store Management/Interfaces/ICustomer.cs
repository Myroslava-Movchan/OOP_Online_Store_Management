﻿using Online_Store_Management.Models;
using Online_Store_Management.DataAccess;

namespace Online_Store_Management.Interfaces
{
    public interface ICustomer
    {
        void LogAction(string message);
        void SetCustomerLogFileStream(FileStream customerLogFileStream);
        Task<Discount> GetNewCustomerAsync(CancellationToken cancellationToken);
        Task<Discount> GetRegularCustomerAsync(CancellationToken cancellationToken);
        Task<CustomerDbModel> GetCustomerByIdAsync(int id, CancellationToken cancellationToken);
        Task AddCustomerAsync(CustomerDbModel customer, CancellationToken cancellationToken);
        Task UpdateAsync(CustomerDbModel customer, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
