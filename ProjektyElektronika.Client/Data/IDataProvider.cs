using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjektyElektronika.Client.Models;

namespace ProjektyElektronika.Client.Data
{
    public interface IDataProvider
    {
        Task<List<Project>> GetProjectList();
        Task<List<String>> GetCategoryList();
        Task DownloadProject(Project project);
        Task OpenProject(Project project);
        Task DeleteProject(Project project);
    }
}