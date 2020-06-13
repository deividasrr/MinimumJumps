using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MinimumJumps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MinimumJumpController : ControllerBase
    {

        private readonly MinimumJumpsContext db = new MinimumJumpsContext();
        private readonly ILogger<MinimumJumpController> _logger;

        public MinimumJumpController(ILogger<MinimumJumpController> logger, MinimumJumpsContext db)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("getpath")]
        public JsonResult GetMinimumJumpShortestPath(string entries)
        {
            try
            {

                int shortestPath = int.MinValue;
                int temp;
                if (entries == null || int.TryParse(entries, out temp))
                {
                    return new JsonResult("Invalid input. Please provide the entries  GET parameter as a comma-delimited integer list.");
                }
                entries = Regex.Replace(entries, @"\s", ""); // remove any whitespace

                // check if input was already calculated and stored in history
                var existingShortestPath = CheckMinJumpHistoryForMatch(entries);
                if (existingShortestPath != int.MinValue)
                {
                    return new JsonResult(existingShortestPath);
                }

                var sourceList = ParseIntStringToList(entries);
                //shortestPath = GetShortestPath(sourceList);
                shortestPath = Jump(sourceList);
                UpdateMinJumpHistory(entries, shortestPath);
                return new JsonResult(shortestPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new JsonResult("An error was encountered during the getpath operation.");
            }
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

        // TODO: check validity of input
        private List<int> ParseIntStringToList(string ints)
        {
            var intList = new List<int>();
            foreach (var number in ints.Split(','))
            {
                intList.Add(int.Parse(number));
            }
            return intList;
        }

        // NOTE: GREEDY ALGORITHM, WILL NOT ALWAYS YIELD BEST PATH
        public int GetShortestPath(List<int> nums)
        {
            if (nums == null || nums.Count < 2)
            {
                return 0;
            }
            int jumpCursor = nums[0];
            int bestJumpRange = nums[0];
            int jump = 1;
            for (int i = 1; i < nums.Count; i++)
            {
                // reached the end of the current cell's jump range,
                // jump becomes necessary. the greatest jump option is chosen
                if (jumpCursor < i)
                {
                    jumpCursor = bestJumpRange;
                    jump++;
                }
                // update bestJumpRange if current cell value + index is greater
                bestJumpRange = Math.Max(bestJumpRange, i + nums[i]);
            }
            return jump;
        }

        // GREEDY ALGORITHM
        public int Jump(List<int> nums)
        {
            if (nums == null || nums.Count < 2)
            {
                return 0;
            }
            var rangeStart = 0;
            var rangeEnd = 0;
            int jump = 0;
            while (true)
            {
                int bestJumpRange = 0;
                // check the current cell's possible destinations' jump ranges.
                for (int i = rangeStart; i <= rangeEnd; i++)
                {
                    bestJumpRange = Math.Max(bestJumpRange, nums[i] + i); // update bestJumpRange if current cell value + index is greater
                }
                // if next cell is unreachable, return 0
                if (bestJumpRange <= rangeEnd)
                {
                    return 0;
                }
                // if current cell can reach the last cell, jump and return jump count
                if (bestJumpRange >= nums.Count - 1)
                {
                    return ++jump;
                } 
                // else continue the loop
                rangeStart = rangeEnd + 1;
                rangeEnd = bestJumpRange;
                jump++;
            }
        }
    }
}
