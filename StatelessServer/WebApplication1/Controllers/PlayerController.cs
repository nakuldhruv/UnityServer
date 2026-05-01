using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PlayerController : Controller
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
            // 在 MongoDB 中查找 Username 匹配的第一条数据
            var player = await _playerCollection.Find(x => x.Name == username).FirstOrDefaultAsync();

            if (player == null)
            {
                return NotFound(new { message = "玩家不存在" });
            }

            return Ok(player);
        }

        // POST: api/player/save
        [HttpPost("save")]
        public async Task<IActionResult> Save(PlayerProfile incomingPlayer)
        {
            // 使用 Upsert (Update or Insert) 逻辑
            // 过滤器：按用户名查找
            var filter = Builders<PlayerProfile>.Filter.Eq(x => x.Name, incomingPlayer.Name);

            // 如果不存在就插入，存在就替换整个文档
            var options = new ReplaceOptions { IsUpsert = true };

            await _playerCollection.ReplaceOneAsync(filter, incomingPlayer, options);

            return Ok(new { status = "Success", message = "存档已同步" });
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
