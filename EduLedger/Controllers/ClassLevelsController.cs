using Azure;
using EduLedger.Data;
using EduLedger.Data.DTOs.ClassLevelDTOs;
using EduLedger.Entitites.DTOs.ClassLevelDTOs;
using EduLedger.Entitites.Models;
using EduLedger.Repository;
using EduLedger.Repository.IRepository;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduLedger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassLevelsController : ControllerBase
    {
        private readonly EduLedgerDBContext _context;
        private readonly IClassLevelRepository _classLevelRepository;

        public ClassLevelsController(EduLedgerDBContext context, IClassLevelRepository classLevelRepository)
        {
            _context = context;
            _classLevelRepository = classLevelRepository;
        }

        // GET: api/ClassLevels
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassLevel>>> GetAllClassLevels()
        {
            var classLevels = await _context.ClassLevels.Where(x => x.IsActive).ToListAsync();
            return classLevels;
        }

        // GET: api/ClassLevels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassLevel>> GetClassLevel(int id)
        {
            var response = await _classLevelRepository.GetClassById(id);
            if (!response.Status) return BadRequest(response);
            return Ok(response);

        }

        // PUT: api/ClassLevels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClassLevel(int id,UpdateClassLevelDTO updateClass)
        {
            var response = await _classLevelRepository.UpdateClass(id, updateClass);
            if (!response.Status) return BadRequest(response);
            return Ok(response);
        }

        // POST: api/ClassLevels
        //Authorize(Roles = "Admin")]
        [HttpPost("create-classlevel")]
        public async Task<IActionResult> CreateClassLevel(CreateClassDTO createClass)
        {
            var response = await _classLevelRepository.CreateClass(createClass);
            if (!response.Status) return BadRequest(response);
            return Ok(response);

            
        }

        // DELETE: api/ClassLevels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassLevel(int id)
        {
            var response = await _classLevelRepository.DeleteClass(id);
            if (!response.Status) return BadRequest(response);
            return Ok(response);
        }
        [HttpPost("{classLevelId}/assign-student/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignStudentToClassLevel(
    int classLevelId,
    string userId)
        {
            var response = await _classLevelRepository.AssignStudentToClassLevel(classLevelId, userId);
            if (!response.Status) return BadRequest(response);
            return Ok("Student assigned successfully");
        }
    }
}
