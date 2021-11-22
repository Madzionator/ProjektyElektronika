using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjektyElektronika.Api.DateBase;
using ProjektyElektronika.Api.Models;
using ProjektyElektronika.Api.Models.DTO;

namespace ProjektyElektronika.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(DataContext context, ILogger<ProjectController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public List<ProjectDto> Get()
        {
            _logger.LogInformation("Reading project list");

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
            _logger.LogInformation($"downloading project {projectId}");

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

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Consumes("multipart/form-data")]
        public ActionResult Add(
            [FromForm] IFormFile file,
            [FromForm] string json)
        {
            var projectDto = JsonConvert.DeserializeObject<ProjectDto>(json);
            var project = new Project
            {
                Title = projectDto.Title,
                DateCreated = projectDto.DateCreated,
                Authors = projectDto.Authors.Select(x => new Author
                {
                    Name = x.Name,
                    Index = x.Index,
                }).ToList()
            };

            _logger.LogInformation($"Adding project {project.Title}");

            //using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Projects.Add(project);
                _context.SaveChanges();

                Directory.CreateDirectory($"/projects/{project.Id}");
                var filePath = $"/projects/{project.Id}/{file.FileName}";
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                    project.Address = filePath;
                }

                _context.SaveChanges();
            //    transaction.Commit();
            }

            return NoContent();
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
