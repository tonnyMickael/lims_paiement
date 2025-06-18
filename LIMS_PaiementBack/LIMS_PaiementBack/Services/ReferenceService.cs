using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LIMS_PaiementBack.Entities;

namespace LIMS_PaiementBack.Services
{
    public interface IReferenceService
    {
        Task<int> GetNextReferenceAsync(DateTime date);
    }

    public class ReferenceService : IReferenceService
    {
        private readonly DbContextEntity _dbContext;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public ReferenceService(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetNextReferenceAsync(DateTime date)
        {
            await _semaphore.WaitAsync();
            try
            {
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var lastDepart = await _dbContext.Depart
                            .OrderByDescending(d => d.reference)
                            .FirstOrDefaultAsync();

                        int newReference = 1;

                        if (lastDepart != null)
                        {
                            if (lastDepart.DateDepart.Year < date.Year)
                            {
                                newReference = 1;
                            }
                            else
                            {
                                newReference = lastDepart.reference + 1;
                            }
                        }

                        await transaction.CommitAsync();
                        return newReference;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
} 