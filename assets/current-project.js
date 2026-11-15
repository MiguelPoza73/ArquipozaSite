/* Portfolio · Current Project — gallery, phase timeline and lightbox */
(function () {
    const root = document.querySelector('.cp');
    if (!root) return;
    root.classList.add('js');
    const slides = [...root.querySelectorAll('.cp-slide')];
    const phases = [...root.querySelectorAll('.cp-phase')];
    const viewer = root.querySelector('.cp-viewer');
    const current = root.querySelector('.cp-counter b');
    const lb = document.getElementById('lightbox');
    const lbImg = lb.querySelector('img');
    const lbCap = lb.querySelector('p');
    const total = slides.length;
    const pad = n => String(n).padStart(2, '0');
    let idx = 0, visible = false;

    function go(n) {
        idx = (n + total) % total;
        /* load the active photo and its neighbours right away */
        [idx - 1, idx, idx + 1].forEach(k => {
            const im = slides[(k + total) % total].querySelector('img');
            if (im.loading !== 'eager') im.loading = 'eager';
        });
        slides.forEach((s, i) => {
            s.classList.toggle('is-active', i === idx);
            s.setAttribute('aria-hidden', i === idx ? 'false' : 'true');
        });
        phases.forEach((p, i) => {
            p.classList.toggle('is-done', i < idx);
            if (i === idx) p.setAttribute('aria-current', 'step'); else p.removeAttribute('aria-current');
        });
        root.style.setProperty('--line', total > 1 ? (idx / (total - 1) * 100) + '%' : '0%');
        if (current) current.textContent = pad(idx + 1);
        if (lb.classList.contains('open')) fillLightbox();
        const active = phases[idx];
        const strip = root.querySelector('.cp-timeline');
        if (active && strip && strip.scrollWidth > strip.clientWidth) {
            strip.scrollTo({ left: active.parentElement.offsetLeft - 8, behavior: 'smooth' });
        }
    }

    function fillLightbox() {
        const s = slides[idx], img = s.querySelector('img');
        lbImg.src = img.currentSrc || img.src;
        lbImg.alt = img.alt;
        lbCap.innerHTML = s.dataset.phase + '<span>' + s.dataset.desc + ' · ' + pad(idx + 1) + ' / ' + pad(total) + '</span>';
    }

    function openLightbox() {
        fillLightbox();
        lb.classList.add('open');
        lb.setAttribute('aria-hidden', 'false');
        document.body.style.overflow = 'hidden';
        lb.querySelector('.close').focus();
    }

    function closeLightbox() {
        if (!lb.classList.contains('open')) return;
        lb.classList.remove('open');
        lb.setAttribute('aria-hidden', 'true');
        document.body.style.overflow = '';
        viewer.focus({ preventScroll: true });
    }

    /* public API (site.js calls closeLightbox on Escape) */
    window.obraGoTo = go;
    window.obraSlide = d => go(idx + d);
    window.openLightbox = openLightbox;
    window.closeLightbox = closeLightbox;

    root.querySelector('.cp-arrow.prev').addEventListener('click', e => { e.stopPropagation(); go(idx - 1); });
    root.querySelector('.cp-arrow.next').addEventListener('click', e => { e.stopPropagation(); go(idx + 1); });
    phases.forEach((p, i) => p.addEventListener('click', () => go(i)));
    viewer.addEventListener('click', openLightbox);
    viewer.addEventListener('keydown', e => { if (e.key === 'Enter' || e.key === ' ') { e.preventDefault(); openLightbox(); } });

    lb.addEventListener('click', e => { if (e.target === lb) closeLightbox(); });
    lb.querySelector('.close').addEventListener('click', closeLightbox);
    lb.querySelector('.cp-arrow.prev').addEventListener('click', () => go(idx - 1));
    lb.querySelector('.cp-arrow.next').addEventListener('click', () => go(idx + 1));

    document.addEventListener('keydown', e => {
        const open = lb.classList.contains('open');
        if (!open && !visible) return;
        if (e.target.closest && e.target.closest('input, textarea, select')) return;
        if (e.key === 'ArrowLeft') { go(idx - 1); e.preventDefault(); }
        if (e.key === 'ArrowRight') { go(idx + 1); e.preventDefault(); }
        if (e.key === 'Escape') closeLightbox();
    });

    /* swipe */
    const swipe = el => {
        let x0 = null, y0 = 0;
        el.addEventListener('touchstart', e => { x0 = e.touches[0].clientX; y0 = e.touches[0].clientY; }, { passive: true });
        el.addEventListener('touchend', e => {
            if (x0 === null) return;
            const dx = e.changedTouches[0].clientX - x0, dy = e.changedTouches[0].clientY - y0;
            if (Math.abs(dx) > 45 && Math.abs(dx) > Math.abs(dy)) { go(idx + (dx < 0 ? 1 : -1)); el.dataset.swiped = '1'; setTimeout(() => delete el.dataset.swiped, 400); }
            x0 = null;
        });
        el.addEventListener('click', e => { if (el.dataset.swiped) e.stopImmediatePropagation(); }, true);
    };
    swipe(viewer); swipe(lbImg);

    /* entrance + visibility for keyboard */
    if ('IntersectionObserver' in window) {
        new IntersectionObserver(es => es.forEach(en => {
            visible = en.isIntersecting;
            if (en.isIntersecting) root.classList.add('in');
        }), { threshold: 0.15 }).observe(root);
    } else {
        root.classList.add('in');
    }

    go(0);
})();
