using DrugPrevention.Repositories.NganVHH.Models;
using DrugPrevention.Repositories.NganVHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugPrevention.Services.NganVHH
{
    public class AppointmentsNganVHHService : IAppointmentsNganVHHService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public AppointmentsNganVHHService() => _unitOfWork ??= new UnitOfWork();


        public async Task<List<AppointmentsNganVHH>> GetAllAsync()
        {
            return await _unitOfWork.AppointmentsNganVHHRepository.GetAllAsync();
        }

        public async Task<AppointmentsNganVHH> GetByIdAsync(int code)
        {
            return await _unitOfWork.AppointmentsNganVHHRepository.GetByIdAsync(code);
        }

        public async Task<List<AppointmentsNganVHH>> SearchAsync(string primaryIssue, int? duration, string specialization)
        {
            return await _unitOfWork.AppointmentsNganVHHRepository.SearchAsync(primaryIssue, duration, specialization);
        }

        public async Task<List<AppointmentsNganVHH>> SearchAsync(string primaryIssue, int? duration, string specialization, int pageNumber, int pageSize)
        {
            return await _unitOfWork.AppointmentsNganVHHRepository.SearchAsync(primaryIssue, duration, specialization, pageNumber, pageSize);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _unitOfWork.AppointmentsNganVHHRepository.GetTotalCountAsync();
        }

        public async Task<int> GetSearchCountAsync(string primaryIssue, int? duration, string specialization)
        {
            return await _unitOfWork.AppointmentsNganVHHRepository.GetSearchCountAsync(primaryIssue, duration, specialization);
        }

        public async Task<int> CreateAsync(AppointmentsNganVHH item)
        {
            await _unitOfWork.ClearTracking();
            return await _unitOfWork.AppointmentsNganVHHRepository.CreateAsync(item);
        }

        public async Task<int> UpdateAsync(AppointmentsNganVHH item)
        {
            Console.WriteLine($"{item.AppointmentNganVHHID} - {item.AppointmentDateTime}");
            var result = await _unitOfWork.AppointmentsNganVHHRepository.UpdateAsync(item);
            await _unitOfWork.AppointmentsNganVHHRepository.SaveAsync();
            return result;

        }

        public async Task<bool> DeleteAsync(int code)
        {
            await _unitOfWork.ClearTracking(); // Clear tracking before deletion
            var item = await _unitOfWork.AppointmentsNganVHHRepository.GetByIdAsync(code);

            if (item == null)
            {
                return false;
            }

            return await _unitOfWork.AppointmentsNganVHHRepository.RemoveAsync(item);
        }
    }
}
