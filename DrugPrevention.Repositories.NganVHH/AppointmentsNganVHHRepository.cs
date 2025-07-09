using DrugPrevention.Repositories.NganVHH.Basic;
using DrugPrevention.Repositories.NganVHH.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugPrevention.Repositories.NganVHH
{
    public class AppointmentsNganVHHRepository : GenericRepository<AppointmentsNganVHH>
    {
        public AppointmentsNganVHHRepository() { }

        public AppointmentsNganVHHRepository(SU25_PRN222_SE1709_G2_DrugPreventionSystemContext context) => _context = context;
        public override async Task<List<AppointmentsNganVHH>> GetAllAsync()
        {
            return await _context.AppointmentsNganVHHs
                .Include(a => a.Consultant)
                    .ThenInclude(c => c.User)
                .Include(a => a.User)
                .ToListAsync() ?? new List<AppointmentsNganVHH>();
        }

        public override async Task<AppointmentsNganVHH> GetByIdAsync(int code) //(Guid code) 
        {
            var item = await _context.AppointmentsNganVHHs
                .Include(i => i.Consultant)
                    .ThenInclude(c => c.User)
                .Include(i => i.User)
                .FirstOrDefaultAsync(o => o.AppointmentNganVHHID == code);

            return item ?? new AppointmentsNganVHH();
        }

        //Search 3 trường thông tin
        public async Task<List<AppointmentsNganVHH>> SearchAsync(string primaryIssue, int? duration, string specialization)
        {
            var list = await _context.AppointmentsNganVHHs
                .Include(i => i.Consultant)
                    .ThenInclude(c => c.User)
                .Include(i => i.User)
                .Where(i =>
                    (string.IsNullOrEmpty(primaryIssue) || (i.PrimaryIssue != null && i.PrimaryIssue.Contains(primaryIssue)))
                    && (duration == null || i.Duration == duration)
                    && (string.IsNullOrEmpty(specialization) || (i.Consultant != null && i.Consultant.Specialization != null && i.Consultant.Specialization.Contains(specialization)))
                    ).ToListAsync();

            return list ?? new List<AppointmentsNganVHH>();
        }
    }
}
