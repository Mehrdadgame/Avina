// Avina - Subtle Star Map Background Animation
// "نقشه ستارگان و مسیر رشد" - ظریف، آرام، الهام‌بخش
// آموزشی است، نباید حواس‌پرت‌کن باشد

(function () {
    const CANVAS_ID = 'avina-stars-bg';
    let canvas, ctx, stars = [], paths = [], rafId = null;
    let w = 0, h = 0, dpr = 1;
    let lastTs = 0;
    let mouseX = -9999, mouseY = -9999;

    // Configuration - tuned for subtlety
    const CFG = {
        starCount: 90,            // تعداد ستاره (با عرض اسکرین scale می‌شه)
        starCountMobile: 45,
        connectDistance: 140,     // حداکثر فاصله برای اتصال
        baseOpacity: 0.55,        // شفافیت پایه ستاره
        lineOpacity: 0.10,        // شفافیت خطوط (خیلی کم)
        twinkleSpeed: 0.0006,     // سرعت چشمک‌زدن
        driftSpeed: 0.012,        // سرعت حرکت ستاره
        // پالت رنگی: آبی-بنفش عمیق + طلایی
        colors: [
            { r: 170, g: 199, b: 255 },   // primary blue (#aac7ff)
            { r: 175, g: 199, b: 247 },   // secondary blue (#afc7f7)
            { r: 145, g: 151, b: 255 },   // deep purple-blue (#9197ff)
            { r: 255, g: 182, b: 140 },   // golden tertiary (#ffb68c)
            { r: 218, g: 197, b: 240 }    // soft purple
        ]
    };

    function isMobile() {
        return window.innerWidth < 768;
    }

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

    function rand(min, max) {
        return Math.random() * (max - min) + min;
    }

    function pickColor() {
        return CFG.colors[Math.floor(Math.random() * CFG.colors.length)];
    }

    function makeStar() {
        const c = pickColor();
        // اکثراً آبی-بنفش، گاهی طلایی (طلایی برجسته‌تر)
        return {
            x: rand(0, w),
            y: rand(0, h),
            // سرعت drift خیلی کم
            vx: rand(-CFG.driftSpeed, CFG.driftSpeed),
            vy: rand(-CFG.driftSpeed, CFG.driftSpeed),
            // اندازه: بیشتر کوچک، چندتا متوسط
            r: Math.random() < 0.85 ? rand(0.6, 1.4) : rand(1.4, 2.2),
            // فاز چشمک‌زدن
            phase: rand(0, Math.PI * 2),
            speed: rand(0.7, 1.3),
            color: c,
            // ستاره‌های طلایی برجسته‌تر باشن (اشاره به هدف، دستاورد)
            glow: c.r > 200 && c.g > 150 && c.b < 200 ? 1.4 : 1.0
        };
    }

    function init() {
        // Remove existing canvas if any (در صورت navigation)
        const existing = document.getElementById(CANVAS_ID);
        if (existing) existing.remove();

        canvas = document.createElement('canvas');
        canvas.id = CANVAS_ID;
        canvas.setAttribute('aria-hidden', 'true');
        // اول از همه body
        document.body.insertBefore(canvas, document.body.firstChild);

        ctx = canvas.getContext('2d', { alpha: true });
        if (!ctx) return;

        resize();

        const count = isMobile() ? CFG.starCountMobile : CFG.starCount;
        stars = [];
        for (let i = 0; i < count; i++) stars.push(makeStar());

        // Listeners
        window.addEventListener('resize', onResize, { passive: true });
        window.addEventListener('mousemove', onMouseMove, { passive: true });

        if (rafId) cancelAnimationFrame(rafId);
        lastTs = performance.now();
        rafId = requestAnimationFrame(loop);
    }

    let resizeTimer = null;
    function onResize() {
        if (resizeTimer) clearTimeout(resizeTimer);
        resizeTimer = setTimeout(() => {
            resize();
            // re-seed برای پر شدن یکنواخت بعد از resize
            const count = isMobile() ? CFG.starCountMobile : CFG.starCount;
            if (stars.length !== count) {
                stars = [];
                for (let i = 0; i < count; i++) stars.push(makeStar());
            }
        }, 150);
    }

    function onMouseMove(e) {
        mouseX = e.clientX;
        mouseY = e.clientY;
    }

    function step(ts) {
        const dt = Math.min(ts - lastTs, 50); // cap to avoid jumps
        lastTs = ts;

        for (let i = 0; i < stars.length; i++) {
            const s = stars[i];
            s.x += s.vx * dt;
            s.y += s.vy * dt;

            // wrap edges آرام
            if (s.x < -10) s.x = w + 10;
            else if (s.x > w + 10) s.x = -10;
            if (s.y < -10) s.y = h + 10;
            else if (s.y > h + 10) s.y = -10;

            s.phase += CFG.twinkleSpeed * s.speed * dt;
        }
    }

    function draw() {
        ctx.clearRect(0, 0, w, h);

        // 1) خطوط اتصال بین ستاره‌های نزدیک (نماد "اتصال مهارت‌ها / مسیر")
        // فقط برای ستاره‌های نزدیک، خیلی کم‌رنگ
        const dMax = CFG.connectDistance;
        const dMaxSq = dMax * dMax;
        ctx.lineWidth = 0.6;

        for (let i = 0; i < stars.length; i++) {
            const a = stars[i];
            for (let j = i + 1; j < stars.length; j++) {
                const b = stars[j];
                const dx = a.x - b.x;
                const dy = a.y - b.y;
                const dsq = dx * dx + dy * dy;
                if (dsq < dMaxSq) {
                    const d = Math.sqrt(dsq);
                    // alpha بر اساس فاصله: نزدیک‌تر = پررنگ‌تر
                    const t = 1 - d / dMax;
                    const alpha = CFG.lineOpacity * t * t;

                    // رنگ خط: ترکیب رنگ دو ستاره
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

        // 2) ستاره‌ها (با چشمک‌زدن آرام)
        for (let i = 0; i < stars.length; i++) {
            const s = stars[i];
            // چشمک‌زدن نرم: opacity بین 0.4 تا 1 از baseOpacity
            const tw = 0.7 + 0.3 * Math.sin(s.phase);
            const alpha = CFG.baseOpacity * tw;

            // فاصله از موس برای effect ظریف (اختیاری - خیلی کم)
            const dxm = s.x - mouseX;
            const dym = s.y - mouseY;
            const dm = Math.sqrt(dxm * dxm + dym * dym);
            const mouseBoost = dm < 120 ? (1 - dm / 120) * 0.25 : 0;

            const finalAlpha = Math.min(1, alpha + mouseBoost);

            // halo (glow) ظریف
            const glowR = s.r * 4 * s.glow;
            const grad = ctx.createRadialGradient(s.x, s.y, 0, s.x, s.y, glowR);
            grad.addColorStop(0, `rgba(${s.color.r}, ${s.color.g}, ${s.color.b}, ${finalAlpha * 0.45})`);
            grad.addColorStop(0.5, `rgba(${s.color.r}, ${s.color.g}, ${s.color.b}, ${finalAlpha * 0.15})`);
            grad.addColorStop(1, `rgba(${s.color.r}, ${s.color.g}, ${s.color.b}, 0)`);
            ctx.fillStyle = grad;
            ctx.beginPath();
            ctx.arc(s.x, s.y, glowR, 0, Math.PI * 2);
            ctx.fill();

            // هسته ستاره
            ctx.fillStyle = `rgba(${s.color.r}, ${s.color.g}, ${s.color.b}, ${finalAlpha})`;
            ctx.beginPath();
            ctx.arc(s.x, s.y, s.r, 0, Math.PI * 2);
            ctx.fill();
        }
    }

    function loop(ts) {
        step(ts);
        draw();
        rafId = requestAnimationFrame(loop);
    }

    function destroy() {
        if (rafId) cancelAnimationFrame(rafId);
        rafId = null;
        window.removeEventListener('resize', onResize);
        window.removeEventListener('mousemove', onMouseMove);
        const el = document.getElementById(CANVAS_ID);
        if (el) el.remove();
    }

    // Respect reduced motion preference
    function prefersReducedMotion() {
        return window.matchMedia &&
            window.matchMedia('(prefers-reduced-motion: reduce)').matches;
    }

    function start() {
        if (prefersReducedMotion()) {
            // فقط ستاره‌های ثابت بدون انیمیشن
            CFG.driftSpeed = 0;
            CFG.twinkleSpeed = 0;
        }
        // فقط در مرورگرهایی که canvas دارن
        if (!('HTMLCanvasElement' in window)) return;
        init();
    }

    // اول از document load، سپس هر بار blazor navigate شد بررسی کنیم
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', start);
    } else {
        start();
    }

    // Blazor enhanced navigation: canvas از قبل هست، نیاز به re-init نیست
    document.addEventListener('blazor:navigated', () => {
        if (!document.getElementById(CANVAS_ID)) {
            start();
        }
    });

    // Expose برای debugging
    window.AvinaStarsBG = { start, destroy, getStars: () => stars };
})();
