using System.Collections.Generic;
using System.Linq;
using PicNic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PicNic.Controllers
{
    public class APIController : Controller
    {
        private PicNicContext _picNicContext;
        public APIController(PicNicContext db) => _picNicContext = db;
        [HttpGet, Route("api/equipment")]
        public IEnumerable<Equipment> Get() => _picNicContext.Equipment.OrderBy(p => p.EquipmentName);
    }
}