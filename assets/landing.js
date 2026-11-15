/* Arquipoza landing — scroll interactions (vanilla, no dependencies) */
(function () {
    const root = document.querySelector('.lp');
    if (!root) return;
    const reduce = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
    const clamp = (v, a = 0, b = 1) => Math.min(b, Math.max(a, v));

    /* split manifesto into words */
    document.querySelectorAll('.lp-words').forEach(p => {
        const gold = (p.dataset.gold || '').toLowerCase().split(',').map(s => s.trim());
        p.innerHTML = p.textContent.trim().split(/\s+/).map(w => {
            const clean = w.toLowerCase().replace(/[^a-záéíóúñ]/g, '');
            return `<span class="${gold.includes(clean) ? 'gold' : ''}">${w}</span> `;
        }).join('');
    });

    /* reveal on enter + counters */
    const counters = [...root.querySelectorAll('[data-count]')];
    const runCount = el => {
        const end = +el.dataset.count, suffix = el.dataset.suffix || '', start = +(el.dataset.from || 0);
        if (reduce) { el.textContent = end + suffix; return; }
        const t0 = performance.now(), dur = 1800;
        const tick = t => {
            const k = clamp((t - t0) / dur), e = 1 - Math.pow(1 - k, 4);
            el.textContent = Math.round(start + (end - start) * e) + suffix;
            if (k < 1) requestAnimationFrame(tick);
        };
        requestAnimationFrame(tick);
    };
    const io = new IntersectionObserver(entries => entries.forEach(en => {
        if (!en.isIntersecting) return;
        en.target.classList.add('in');
        if (en.target.hasAttribute('data-count')) runCount(en.target);
        io.unobserve(en.target);
    }), { threshold: 0.2, rootMargin: '0px 0px -8% 0px' });
    root.querySelectorAll('.reveal, .lp-value, [data-count]').forEach(el => io.observe(el));

    if (reduce) {
        document.querySelectorAll('.lp-words span').forEach(s => s.classList.add('on'));
        return;
    }

    /* scroll-linked effects */
    const progress = document.querySelector('.lp-progress');
    const hero = root.querySelector('.lp-hero');
    const words = [...root.querySelectorAll('.lp-words span')];
    const manifesto = root.querySelector('.lp-manifesto');
    const works = root.querySelector('.lp-works');
    const track = root.querySelector('.lp-track');
    const bar = root.querySelector('.lp-works__bar');
    const current = root.querySelector('.lp-works__current');
    const cards = track ? [...track.querySelectorAll('.lp-card:not(.lp-card--more)')] : [];
    const sketch = root.querySelector('.lp-sketch__stage');
    const cta = root.querySelector('.lp-cta');

    let travel = 0;
    const layout = () => {
        if (!works || !track) return;
        travel = Math.max(0, track.scrollWidth - window.innerWidth);
        works.style.setProperty('--track-h', (window.innerHeight + travel) + 'px');
    };

    const update = () => {
        const vh = window.innerHeight, y = window.scrollY;
        const docH = document.documentElement.scrollHeight - vh;
        if (progress) progress.style.setProperty('--p', docH > 0 ? clamp(y / docH) : 0);

        if (hero) hero.style.setProperty('--h', clamp(y / hero.offsetHeight));

        if (manifesto && words.length) {
            const r = manifesto.getBoundingClientRect();
            const k = clamp((vh * 0.85 - r.top) / (r.height + vh * 0.1));
            const n = Math.floor(k * words.length * 1.15);
            words.forEach((w, i) => w.classList.toggle('on', i < n));
        }

        if (works && track) {
            const r = works.getBoundingClientRect();
            const k = travel > 0 ? clamp(-r.top / travel) : 0;
            track.style.transform = `translate3d(${-k * travel}px,0,0)`;
            if (bar) bar.style.setProperty('--w', k);
            if (current && cards.length) current.textContent = String(Math.min(cards.length, 1 + Math.floor(k * cards.length * 0.999))).padStart(2, '0');
        }

        if (sketch) {
            const r = sketch.getBoundingClientRect();
            sketch.style.setProperty('--s', (vh / 2 - (r.top + r.height / 2)) * -1);
        }

        if (cta) {
            const r = cta.getBoundingClientRect();
            cta.style.setProperty('--c', clamp((vh - r.top) / (vh + r.height)) - 0.5);
        }
    };

    let ticking = false;
    const onScroll = () => {
        if (ticking) return;
        ticking = true;
        requestAnimationFrame(() => { update(); ticking = false; });
    };
    window.addEventListener('scroll', onScroll, { passive: true });
    window.addEventListener('resize', () => { layout(); update(); });
    window.addEventListener('load', () => { layout(); update(); });
    layout(); update();
})();
