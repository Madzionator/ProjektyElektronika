using System.Collections.Generic;
using System.Threading.Tasks;
using ProjektyElektronika.Client.Models;

namespace ProjektyElektronika.Client.Data
{
    public interface IDataProvider
    {
        Task<List<Project>> GetProjectList();
        Task DownloadProject(Project project);
        Task OpenProject(Project project);
    }
}