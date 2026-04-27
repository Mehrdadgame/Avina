// Avina - Star Map Background Animation
// "نقشه ستارگان و مسیر رشد" - الهام‌بخش، جذاب اما بدون حواس‌پرتی
// هر ستاره = یک مهارت / هر خط = یک اتصال در مسیر

(function () {
    const CANVAS_ID = 'avina-stars-bg';
    let canvas, ctx, stars = [], shootingStars = [], rafId = null;
    let w = 0, h = 0, dpr = 1;
    let lastTs = 0;
    let mouseX = -9999, mouseY = -9999;
    let lastShootingStar = 0;

    const CFG = {
        starCount: 140,
        starCountMobile: 70,
        connectDistance: 150,
        baseOpacity: 0.85,
        lineOpacity: 0.18,
        twinkleSpeed: 0.0009,
        driftSpeed: 0.018,
        // پالت آبی-بنفش-طلایی: گرم، الهام‌بخش، شب‌گونه
        colors: [
            { r: 170, g: 199, b: 255 },   // آبی روشن
            { r: 175, g: 199, b: 247 },   // آبی نرم
            { r: 145, g: 151, b: 255 },   // آبی-بنفش
            { r: 180, g: 150, b: 255 },   // بنفش روشن
            { r: 200, g: 175, b: 255 },   // یاسی
            { r: 255, g: 215, b: 140 },   // طلایی روشن (دستاورد)
            { r: 255, g: 182, b: 140 },   // طلایی-نارنجی
            { r: 230, g: 200, b: 255 }    // بنفش-سفید
        ],
        shootingStarMinInterval: 6000,    // هر ۶ تا ۱۲ ثانیه یک شهاب
        shootingStarMaxInterval: 12000
    };

    function isMobile() { return window.innerWidth < 768; }

    function resize() {
        if (!canvas) return;
        dpr = Math.min(window.devicePixelRatio || 1, 2);
        w = window.innerWidth;
        h = window.innerHeight;
        canvas.width = Math.floor(w * dpr);
        canvas.height = Math.floor(h * dpr);
        canvas.style.width = w + 'px';
        canvas.style.height = h + 'px';
        ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
    }

    function rand(min, max) { return Math.random() * (max - min) + min; }
    function pickColor() { return CFG.colors[Math.floor(Math.random() * CFG.colors.length)]; }

    function makeStar() {
        const c = pickColor();
        const isGolden = c.r > 220 && c.g > 180;
        return {
            x: rand(0, w),
            y: rand(0, h),
            vx: rand(-CFG.driftSpeed, CFG.driftSpeed),
            vy: rand(-CFG.driftSpeed, CFG.driftSpeed),
            // ستاره‌های طلایی بزرگ‌تر و برجسته‌تر
            r: isGolden ? rand(1.4, 2.4) : (Math.random() < 0.8 ? rand(0.8, 1.6) : rand(1.6, 2.2)),
            phase: rand(0, Math.PI * 2),
            speed: rand(0.6, 1.4),
            color: c,
            glow: isGolden ? 1.7 : 1.0,
            isGolden
        };
    }

    function spawnShootingStar() {
        // شهاب از یک گوشه به سمت گوشه مقابل
        const fromTop = Math.random() < 0.5;
        const startX = rand(0, w * 0.4);
        const startY = fromTop ? rand(-30, h * 0.2) : rand(h * 0.5, h * 0.7);
        const angle = rand(Math.PI * 0.15, Math.PI * 0.35); // پایین-راست
        const speed = rand(0.6, 1.0);

        shootingStars.push({
            x: startX,
            y: startY,
            vx: Math.cos(angle) * speed,
            vy: Math.sin(angle) * speed,
            life: 0,
            maxLife: rand(1100, 1700),
            length: rand(80, 140),
            color: Math.random() < 0.5
                ? { r: 255, g: 215, b: 150 }   // طلایی
                : { r: 200, g: 200, b: 255 }    // آبی-سفید
        });
    }

    function init() {
        const existing = document.getElementById(CANVAS_ID);
        if (existing) existing.remove();

        canvas = document.createElement('canvas');
        canvas.id = CANVAS_ID;
        canvas.setAttribute('aria-hidden', 'true');
        document.body.insertBefore(canvas, document.body.firstChild);

        ctx = canvas.getContext('2d', { alpha: true });
        if (!ctx) return;

        resize();

        const count = isMobile() ? CFG.starCountMobile : CFG.starCount;
        stars = [];
        shootingStars = [];
        for (let i = 0; i < count; i++) stars.push(makeStar());

        window.addEventListener('resize', onResize, { passive: true });
        window.addEventListener('mousemove', onMouseMove, { passive: true });

        if (rafId) cancelAnimationFrame(rafId);
        lastTs = performance.now();
        lastShootingStar = lastTs + rand(2000, 5000);
        rafId = requestAnimationFrame(loop);
    }

    let resizeTimer = null;
    function onResize() {
        if (resizeTimer) clearTimeout(resizeTimer);
        resizeTimer = setTimeout(() => {
            resize();
            const count = isMobile() ? CFG.starCountMobile : CFG.starCount;
            if (Math.abs(stars.length - count) > 5) {
                stars = [];
                for (let i = 0; i < count; i++) stars.push(makeStar());
            }
        }, 150);
    }

    function onMouseMove(e) { mouseX = e.clientX; mouseY = e.clientY; }

    function step(ts) {
        const dt = Math.min(ts - lastTs, 50);
        lastTs = ts;

        for (let i = 0; i < stars.length; i++) {
            const s = stars[i];
            s.x += s.vx * dt;
            s.y += s.vy * dt;
            if (s.x < -10) s.x = w + 10;
            else if (s.x > w + 10) s.x = -10;
            if (s.y < -10) s.y = h + 10;
            else if (s.y > h + 10) s.y = -10;
            s.phase += CFG.twinkleSpeed * s.speed * dt;
        }

        // شهاب‌سنگ
        for (let i = shootingStars.length - 1; i >= 0; i--) {
            const sh = shootingStars[i];
            sh.x += sh.vx * dt;
            sh.y += sh.vy * dt;
            sh.life += dt;
            if (sh.life > sh.maxLife || sh.x > w + 100 || sh.y > h + 100) {
                shootingStars.splice(i, 1);
            }
        }

        if (ts > lastShootingStar) {
            spawnShootingStar();
            lastShootingStar = ts + rand(CFG.shootingStarMinInterval, CFG.shootingStarMaxInterval);
        }
    }

    function draw() {
        ctx.clearRect(0, 0, w, h);

        // 1) خطوط اتصال
        const dMax = CFG.connectDistance;
        const dMaxSq = dMax * dMax;
        ctx.lineWidth = 0.7;

        for (let i = 0; i < stars.length; i++) {
            const a = stars[i];
            for (let j = i + 1; j < stars.length; j++) {
                const b = stars[j];
                const dx = a.x - b.x;
                const dy = a.y - b.y;
                const dsq = dx * dx + dy * dy;
                if (dsq < dMaxSq) {
                    const d = Math.sqrt(dsq);
                    const t = 1 - d / dMax;
                    const alpha = CFG.lineOpacity * t * t;
                    const r = (a.color.r + b.color.r) * 0.5;
                    const g = (a.color.g + b.color.g) * 0.5;
                    const bl = (a.color.b + b.color.b) * 0.5;
                    ctx.strokeStyle = `rgba(${r | 0}, ${g | 0}, ${bl | 0}, ${alpha})`;
                    ctx.beginPath();
                    ctx.moveTo(a.x, a.y);
                    ctx.lineTo(b.x, b.y);
                    ctx.stroke();
                }
            }
        }

        // 2) ستاره‌ها با چشمک‌زدن
        for (let i = 0; i < stars.length; i++) {
            const s = stars[i];
            const tw = 0.6 + 0.4 * Math.sin(s.phase);
            const alpha = CFG.baseOpacity * tw;

            const dxm = s.x - mouseX;
            const dym = s.y - mouseY;
            const dm = Math.sqrt(dxm * dxm + dym * dym);
            const mouseBoost = dm < 130 ? (1 - dm / 130) * 0.35 : 0;
            const finalAlpha = Math.min(1, alpha + mouseBoost);

            // halo (glow) واضح
            const glowR = s.r * 5 * s.glow;
            const grad = ctx.createRadialGradient(s.x, s.y, 0, s.x, s.y, glowR);
            grad.addColorStop(0, `rgba(${s.color.r}, ${s.color.g}, ${s.color.b}, ${finalAlpha * 0.55})`);
            grad.addColorStop(0.4, `rgba(${s.color.r}, ${s.color.g}, ${s.color.b}, ${finalAlpha * 0.20})`);
            grad.addColorStop(1, `rgba(${s.color.r}, ${s.color.g}, ${s.color.b}, 0)`);
            ctx.fillStyle = grad;
            ctx.beginPath();
            ctx.arc(s.x, s.y, glowR, 0, Math.PI * 2);
            ctx.fill();

            // هسته
            ctx.fillStyle = `rgba(${s.color.r}, ${s.color.g}, ${s.color.b}, ${finalAlpha})`;
            ctx.beginPath();
            ctx.arc(s.x, s.y, s.r, 0, Math.PI * 2);
            ctx.fill();

            // ستاره طلایی: یک پلاس کوچک (نماد دستاورد)
            if (s.isGolden && finalAlpha > 0.5) {
                ctx.strokeStyle = `rgba(${s.color.r}, ${s.color.g}, ${s.color.b}, ${finalAlpha * 0.6})`;
                ctx.lineWidth = 0.5;
                ctx.beginPath();
                ctx.moveTo(s.x - s.r * 2.5, s.y);
                ctx.lineTo(s.x + s.r * 2.5, s.y);
                ctx.moveTo(s.x, s.y - s.r * 2.5);
                ctx.lineTo(s.x, s.y + s.r * 2.5);
                ctx.stroke();
            }
        }

        // 3) شهاب‌سنگ‌ها (نماد دستاورد ناگهانی)
        for (let i = 0; i < shootingStars.length; i++) {
            const sh = shootingStars[i];
            const lifeT = sh.life / sh.maxLife;
            const alpha = lifeT < 0.2 ? lifeT * 5 : (1 - lifeT) * 1.25;
            const a = Math.max(0, Math.min(1, alpha));

            const tailX = sh.x - sh.vx * sh.length;
            const tailY = sh.y - sh.vy * sh.length;
            const grad = ctx.createLinearGradient(sh.x, sh.y, tailX, tailY);
            grad.addColorStop(0, `rgba(${sh.color.r}, ${sh.color.g}, ${sh.color.b}, ${a})`);
            grad.addColorStop(1, `rgba(${sh.color.r}, ${sh.color.g}, ${sh.color.b}, 0)`);
            ctx.strokeStyle = grad;
            ctx.lineWidth = 1.8;
            ctx.lineCap = 'round';
            ctx.beginPath();
            ctx.moveTo(tailX, tailY);
            ctx.lineTo(sh.x, sh.y);
            ctx.stroke();

            // سر شهاب
            const headGrad = ctx.createRadialGradient(sh.x, sh.y, 0, sh.x, sh.y, 6);
            headGrad.addColorStop(0, `rgba(${sh.color.r}, ${sh.color.g}, ${sh.color.b}, ${a})`);
            headGrad.addColorStop(1, `rgba(${sh.color.r}, ${sh.color.g}, ${sh.color.b}, 0)`);
            ctx.fillStyle = headGrad;
            ctx.beginPath();
            ctx.arc(sh.x, sh.y, 6, 0, Math.PI * 2);
            ctx.fill();
        }
    }

    function loop(ts) { step(ts); draw(); rafId = requestAnimationFrame(loop); }

    function destroy() {
        if (rafId) cancelAnimationFrame(rafId);
        rafId = null;
        window.removeEventListener('resize', onResize);
        window.removeEventListener('mousemove', onMouseMove);
        const el = document.getElementById(CANVAS_ID);
        if (el) el.remove();
    }

    function prefersReducedMotion() {
        return window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches;
    }

    function start() {
        if (prefersReducedMotion()) {
            CFG.driftSpeed = 0;
            CFG.twinkleSpeed = 0;
        }
        if (!('HTMLCanvasElement' in window)) return;
        init();
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', start);
    } else {
        start();
    }

    document.addEventListener('blazor:navigated', () => {
        if (!document.getElementById(CANVAS_ID)) start();
    });

    window.AvinaStarsBG = { start, destroy };
})();
