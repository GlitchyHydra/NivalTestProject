using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Contracts;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    public class LeaderboardController : Controller
    {

        private bool isSorted = false;

        private static List<LeaderboardNote> Leaderboard =
            new List<LeaderboardNote>();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, Route("Post")]
        public void Post([FromBody] LeaderboardNote leaderboardNote)
        {
            if (Leaderboard.Count < 10)
            {
                Leaderboard.Add(leaderboardNote);
                isSorted = false;
            }
            else
            {
                int answersCount = leaderboardNote.score;
                int indexOfAdd = Leaderboard.FindIndex(note => note.score < answersCount);
                if (indexOfAdd != -1) {
                    Leaderboard.Insert(indexOfAdd, leaderboardNote);
                    Leaderboard.RemoveAt(10);
                }
            }
        }

        [HttpGet]
        public JsonResult Get()
        {
            if (isSorted)
                return Json(Leaderboard);
            else {
                Leaderboard = Leaderboard.OrderByDescending(ldNote => ldNote.score).ToList();
                isSorted = true;
                return Json(Leaderboard);
            } 
        }
    }
}