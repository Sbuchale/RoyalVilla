using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using System.Collections;

namespace RoyalVilla_API.Controllers
{
    //[Route("api/[controller]")]
    // [Route("/Villa")]
    [Route("api/Villa")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        #region FRomRout/fromQuery/fromheader


        //[HttpGet]
        //[Route("/villas")]
        // [Route("/GetVilla")]

        //[HttpGet]
        //public string GetVillas()
        //{
        //    return "Get all Villas";
        //}

        //[HttpGet("{Id:int}")]
        //public string GetVillaById(int id)
        //{
        //    return "Get Villa:" + id;
        //}


        //[HttpGet("{Id:int}/{name:string}")]
        //[HttpGet("{id:int}/{name}")]
        // public string GetVillaByIdAndName([FromRoute] int id, string name)
        // public string GetVillaByIdAndName([FromQuery] int id, [FromQuery] string name)
        //[HttpGet()]
        //public string GetVillaByIdAndName([FromQuery] int id, [FromHeader] string name)
        //{
        //    return "Get Villa:" + id + ":" + name;
        //}

        #endregion

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
