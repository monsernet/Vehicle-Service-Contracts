using GSP.Models.Domain;

namespace GSP.Models.ViewModels
{
    public class ServiceContractPrintViewModel
    {
        public ServiceContract ServiceContract { get; set; }
        public List<ServiceContractPart> ServiceContractParts { get; set; }
    }
}
