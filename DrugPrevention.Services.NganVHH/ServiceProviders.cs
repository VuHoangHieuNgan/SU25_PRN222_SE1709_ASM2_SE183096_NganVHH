using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugPrevention.Services.NganVHH
{
    public interface IServiceProviders
    {
        ISystemUserAccountService SystemUserAccountService { get; }
        IUsersTuyenTMService UsersTuyenTMService { get; }
        IAppointmentsNganVHHService AppointmentsNganVHHService { get; }
        IConsultantsTrongLHService ConsultantsTrongLHService { get; }

    }
    public class ServiceProviders : IServiceProviders
    {
        public ISystemUserAccountService SystemUserAccountService { get; }
        public IUsersTuyenTMService UsersTuyenTMService { get; }
        public IAppointmentsNganVHHService AppointmentsNganVHHService { get; }
        public IConsultantsTrongLHService ConsultantsTrongLHService { get; }

        public ServiceProviders(
            ISystemUserAccountService systemUserAccountService,
            IUsersTuyenTMService usersTuyenTMService,
            IAppointmentsNganVHHService appointmentsNganVHHService,
            IConsultantsTrongLHService consultantsTrongLHService)
        {
            SystemUserAccountService = systemUserAccountService;
            UsersTuyenTMService = usersTuyenTMService;
            AppointmentsNganVHHService = appointmentsNganVHHService;
            ConsultantsTrongLHService = consultantsTrongLHService;
        }
    }

}
