using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IMongoCollection<PlayerProfile> _playerCollection;

        public PlayerController(IMongoClient mongoClient, IConfiguration config)
        {
            var databaseName = config.GetValue<string>("PlayerDbSettings:DatabaseName");
            var collectionName = config.GetValue<string>("PlayerDbSettings:CollectionName");

            var database = mongoClient.GetDatabase(databaseName);
            _playerCollection = database.GetCollection<PlayerProfile>(collectionName);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<PlayerProfile>> Get(string username)
        {
            var player = await _playerCollection.Find(x => x.Name == username).FirstOrDefaultAsync();

            if (player == null)
            {
                return NotFound(new { message = "玩家不存在" });
            }

            return Ok(player);
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save(PlayerProfile player)
        {
            if (string.IsNullOrEmpty(player.Name))
            {
                return BadRequest(new { message = "用户名不能为空" });
            }

            var filter = Builders<PlayerProfile>.Filter.Eq(x => x.Name, player.Name);

            var options = new ReplaceOptions { IsUpsert = true };

            await _playerCollection.ReplaceOneAsync(filter, player, options);

            return Ok(new { status = "Success", message = "数据同步成功" });
        }
    }
}