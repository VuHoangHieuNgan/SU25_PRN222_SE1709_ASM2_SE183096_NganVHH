using DrugPrevention.Repositories.NganVHH.Models;
using DrugPrevention.Repositories.NganVHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugPrevention.Services.NganVHH
{
    public class SystemUserAccountService : ISystemUserAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public SystemUserAccountService()
        {
            _unitOfWork = new UnitOfWork();
        }
        
        public SystemUserAccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<System_UserAccount> GetUserAccountAsync(string userName, string password)
        {
            return await _unitOfWork.SystemUserAccountRepository.GetUserAccount(userName, password);
        }

        public async Task<System_UserAccount?> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.SystemUserAccountRepository.GetUserByEmailAsync(email);
        }

        public async Task<int> CreateUserAccountAsync(System_UserAccount userAccount)
        {
            return await _unitOfWork.SystemUserAccountRepository.CreateUserAccountAsync(userAccount);
        }
    }
}
