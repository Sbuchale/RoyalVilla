using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using System.Collections;

namespace RoyalVilla_API.Controllers
{

    [Route("api/Villa")]
    [ApiController]
    public class VillaController : ControllerBase
    {

        private readonly ApplicationDbContext _db;
        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }
         
        [HttpGet]
        public IEnumerable<Villa> GetVillas()
        {
            return _db.Villa.ToList();
        }

        [HttpGet("{Id:int}")]
        public string GetVillaById(int id)
        {
            return "Get Villa:" + id;
        }
    }
}
