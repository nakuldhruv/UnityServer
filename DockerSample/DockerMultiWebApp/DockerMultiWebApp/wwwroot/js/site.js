// 首页动效：笑脸表情定时轮换，每次切换有一个“弹出”动画
(function () {
    const emojis = ['😂', '🤣', '😄', '😆', '😜'];
    const el = document.querySelector('.emoji');
    if (!el) {
        return;
    }

    let index = 0;
    setInterval(() => {
        index = (index + 1) % emojis.length;
        el.textContent = emojis[index];
        el.classList.remove('emoji-swap');
        void el.offsetWidth; // 强制重绘以重新触发动画
        el.classList.add('emoji-swap');
    }, 3000);
})();
