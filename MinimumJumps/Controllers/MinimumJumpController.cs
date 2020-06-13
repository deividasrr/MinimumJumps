using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MinimumJumps.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MinimumJumpController : ControllerBase
    {

        private MinimumJumpsContext db;
        private readonly ILogger<MinimumJumpController> _logger;

        public MinimumJumpController(ILogger<MinimumJumpController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public int GetMinimumJumpShortestPath(string source)
        {

            // check if input was already calculated and stored in history
            var existingShortestPath = CheckMinJumpHistoryForMatch(source);
            if (existingShortestPath != int.MinValue)
            {
                return existingShortestPath;
            }

            // TODO: reimplement DP algorithm for finding shortest path
            var shortestPath = 10;
            UpdateMinJumpHistory(source, shortestPath);
            return shortestPath;
        }

        private int CheckMinJumpHistoryForMatch(string source)
        {
            var match = db.MinimumJumpHistories.Where(x => x.entries == source).FirstOrDefault();
            return match != null ? match.shortestPath : int.MinValue;
        }

        private void UpdateMinJumpHistory(string entries, int shortestPath)
        {
            db.MinimumJumpHistories.Add(new MinimumJumpHistory { entries = entries, shortestPath = shortestPath });
            db.SaveChanges();
        }
    }
}
