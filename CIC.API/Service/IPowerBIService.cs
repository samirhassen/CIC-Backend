using CIC.API.DTO;
using Microsoft.PowerBI.Api.Models;

namespace CIC.API.Service
{
    public interface IPowerBIService
    {
        Task<EmbeddedReportConfig> GetEmbedReportConfig(Guid reportId, string roleName, string emailaddress1);
    }
}
