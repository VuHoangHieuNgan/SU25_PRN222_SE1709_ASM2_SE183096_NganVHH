using DrugPrevention.Repositories.NganVHH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugPrevention.Repositories.NganVHH
{
    public interface IUnitOfWork : IDisposable
    {
        AppointmentsNganVHHRepository AppointmentsNganVHHRepository { get; }
        ConsultantsTrongLHRepository ConsultantsTrongLHRepository { get; }
        UsersTuyenTMRepository UsersTuyenTMRepository { get; }
        SystemUserAccountRepository SystemUserAccountRepository { get; }
        int SaveChangesWithTransaction();
        Task<int> SaveChangesWithTransactionAsync();
        Task ClearTracking();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SU25_PRN222_SE1709_G2_DrugPreventionSystemContext _context;
        private AppointmentsNganVHHRepository _appointmentsNganVHHRepository;
        private ConsultantsTrongLHRepository _consultantsTrongLHRepository;
        private UsersTuyenTMRepository _usersTuyenTMRepository;
        private SystemUserAccountRepository _systemUserAccountRepository;

        public UnitOfWork()
        {
            _context = new SU25_PRN222_SE1709_G2_DrugPreventionSystemContext();
        }
        
        public UnitOfWork(SU25_PRN222_SE1709_G2_DrugPreventionSystemContext context)
        {
            _context = context;
        }

        public AppointmentsNganVHHRepository AppointmentsNganVHHRepository
        {
            get { return _appointmentsNganVHHRepository ??= new AppointmentsNganVHHRepository(_context); }
        }

        public ConsultantsTrongLHRepository ConsultantsTrongLHRepository
        {
            get { return _consultantsTrongLHRepository ??= new ConsultantsTrongLHRepository(_context); }
        }

        public UsersTuyenTMRepository UsersTuyenTMRepository
        {
            get { return _usersTuyenTMRepository ??= new UsersTuyenTMRepository(_context); }
        }

        public SystemUserAccountRepository SystemUserAccountRepository
        {
            get { return _systemUserAccountRepository ??= new SystemUserAccountRepository(_context); }
        }

        public void Dispose() => _context.Dispose();

        public int SaveChangesWithTransaction()
        {
            int result = -1;

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = _context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {                    
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        public async Task<int> SaveChangesWithTransactionAsync()
        {
            int result = -1;

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {              
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        public async Task ClearTracking()
        {
            //if (_context.ChangeTracker.HasChanges())
            //{
            //    _context.ChangeTracker.Clear();
            //}
            //await Task.CompletedTask; // Ensure the method is asyn
            if (_context.ChangeTracker.Entries().Any())
            {
                _context.ChangeTracker.Clear();
            }
            await Task.CompletedTask; // Ensure the method is asynchronous
        }
    }
}
