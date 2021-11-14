using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjektyElektronika.Api.DateBase;
using ProjektyElektronika.Api.Models.DTO;

namespace ProjektyElektronika.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly DataContext _context;

        public ProjectController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<ProjectDto> Get()
        {
            return _context.Projects
                .Include(x => x.Authors)
                .Select(x => new ProjectDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    DateCreated = x.DateCreated,
                    Authors = x.Authors.Select(y => new AuthorDto
                    {
                        Index = y.Index,
                        Name = y.Name
                    }).ToList()
                })
                .ToList();
        }

        [HttpGet("download-request/{projectId}")]
        public IActionResult DownloadRequest([FromRoute] int projectId)
        {
            var project = _context.Projects.Find(projectId);
            if (project == null)
                return NotFound();

            var filename = project.Address.Split(new []{'/', '\\'})[^1];
            var extension = filename.Split('.')[^1];

            var file = new FileContentResult(System.IO.File.ReadAllBytes(project.Address), GetMimeType(extension))
                {
                    FileDownloadName = filename
                };
            Response.Headers.Add("filename", filename);
            return file;
        }

        private string GetMimeType(string extension) => extension switch
        {
            "pdf" => "application/pdf",
            "rar" => "application/x-rar-compressed",
            "zip" => "application/zip",
            _ => throw new NotImplementedException()
        };
    }
}
