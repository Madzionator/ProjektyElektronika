using System.Collections.Generic;
using System.Threading.Tasks;
using ProjektyElektronika.Shared.DTO;

namespace ProjektyElektronika.Client.Data
{
    public interface IDataProvider
    {
        Task<List<ProjectDto>> GetProjectList();
        Task DownloadProject(int projectId);
    }
}