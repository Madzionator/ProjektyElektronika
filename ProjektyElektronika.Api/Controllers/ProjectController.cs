using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjektyElektronika.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjektyElektronika.Api.DateBase;
using ProjektyElektronika.Api.Models;
using ProjektyElektronika.Shared.DTO;

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
    }
}
