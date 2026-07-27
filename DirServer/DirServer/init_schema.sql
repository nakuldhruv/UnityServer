-- 创建服务器列表表
CREATE TABLE IF NOT EXISTS game_servers (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    ip VARCHAR(50) NOT NULL,
    port INTEGER NOT NULL,
    status VARCHAR(20) NOT NULL
);

-- 插入初始配置
INSERT INTO game_servers (name, ip, port, status) 
VALUES ('一区: 盘古开天', '127.0.0.1', 9001, '流畅')
ON CONFLICT DO NOTHING; -- 避免重复插入