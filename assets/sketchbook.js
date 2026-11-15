/* Sketchbook — reveal on scroll, cursor follower and full-screen viewer */
(function () {
    const root = document.querySelector('.sk');
    if (!root) return;
    root.classList.add('js');
    const reduce = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
    const items = [...root.querySelectorAll('.sk-item')];
    const total = items.length;
    const pad = n => String(n).padStart(2, '0');

    /* reveal */
    const io = new IntersectionObserver(es => es.forEach(en => {
        if (!en.isIntersecting) return;
        en.target.classList.add('in');
        io.unobserve(en.target);
    }), { threshold: 0.12, rootMargin: '0px 0px -6% 0px' });
    items.forEach(el => io.observe(el));
    const end = root.querySelector('.sk-end');
    if (end) io.observe(end);

    /* hero scroll fade + spiral rotation */
    const hero = root.querySelector('.sk-hero');
    if (hero && !reduce) {
        let t = false;
        const upd = () => { hero.style.setProperty('--h', Math.min(1, Math.max(0, scrollY / hero.offsetHeight))); t = false; };
        addEventListener('scroll', () => { if (!t) { t = true; requestAnimationFrame(upd); } }, { passive: true });
        upd();
    }

    /* cursor follower (desktop only) */
    const cursor = root.querySelector('.sk-cursor');
    if (cursor && matchMedia('(hover: hover) and (pointer: fine)').matches && !reduce) {
        addEventListener('pointermove', e => {
            cursor.style.setProperty('--x', e.clientX + 'px');
            cursor.style.setProperty('--y', e.clientY + 'px');
        }, { passive: true });
        items.forEach(it => {
            it.addEventListener('pointerenter', () => cursor.classList.add('on'));
            it.addEventListener('pointerleave', () => cursor.classList.remove('on'));
        });
    }

    /* viewer */
    const v = document.getElementById('skViewer');
    const vImg = v.querySelector('.sk-stage img');
    const vCount = v.querySelector('.count b');
    const strip = v.querySelector('.sk-strip');
    let idx = 0, lastFocus = null;

    strip.innerHTML = items.map((it, i) =>
        `<button type="button" aria-label="Sketch ${i + 1}"><img src="${it.querySelector('img').getAttribute('src')}" alt="" loading="lazy" decoding="async"></button>`).join('');
    const thumbs = [...strip.children];

    function show(n) {
        idx = (n + total) % total;
        const src = items[idx].querySelector('img');
        vImg.style.animation = 'none'; void vImg.offsetWidth; vImg.style.animation = '';
        vImg.src = src.getAttribute('src');
        vImg.alt = src.alt;
        vCount.textContent = pad(idx + 1);
        thumbs.forEach((t, i) => t.setAttribute('aria-current', i === idx ? 'true' : 'false'));
        thumbs[idx].scrollIntoView({ block: 'nearest', inline: 'center', behavior: reduce ? 'auto' : 'smooth' });
        [idx - 1, idx + 1].forEach(k => { const im = new Image(); im.src = items[(k + total) % total].querySelector('img').getAttribute('src'); });
    }
    function open(n) {
        lastFocus = document.activeElement;
        v.classList.add('open');
        v.setAttribute('aria-hidden', 'false');
        document.body.style.overflow = 'hidden';
        if (cursor) cursor.classList.remove('on');
        show(n);
        v.querySelector('.close').focus();
    }
    function close() {
        if (!v.classList.contains('open')) return;
        v.classList.remove('open');
        v.setAttribute('aria-hidden', 'true');
        document.body.style.overflow = '';
        if (lastFocus) lastFocus.focus({ preventScroll: true });
    }
    window.closeLightbox = close; /* site.js closes on Escape */

    items.forEach((it, i) => it.addEventListener('click', () => open(i)));
    thumbs.forEach((t, i) => t.addEventListener('click', () => show(i)));
    v.querySelector('.close').addEventListener('click', close);
    v.querySelector('.sk-nav.prev').addEventListener('click', () => show(idx - 1));
    v.querySelector('.sk-nav.next').addEventListener('click', () => show(idx + 1));
    v.querySelector('.sk-stage').addEventListener('click', e => { if (e.target.classList.contains('sk-stage')) close(); });

    document.addEventListener('keydown', e => {
        if (!v.classList.contains('open')) return;
        if (e.key === 'ArrowLeft') show(idx - 1);
        else if (e.key === 'ArrowRight') show(idx + 1);
        else if (e.key === 'Escape') close();
    });

    let x0 = null, y0 = 0;
    v.addEventListener('touchstart', e => { x0 = e.touches[0].clientX; y0 = e.touches[0].clientY; }, { passive: true });
    v.addEventListener('touchend', e => {
        if (x0 === null) return;
        const dx = e.changedTouches[0].clientX - x0, dy = e.changedTouches[0].clientY - y0;
        if (Math.abs(dx) > 45 && Math.abs(dx) > Math.abs(dy)) show(idx + (dx < 0 ? 1 : -1));
        x0 = null;
    });
})();
