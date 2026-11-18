/* Ficha de proyecto — visor a pantalla completa de la portada y la galería */
(() => {
    const shots = [...document.querySelectorAll('.pf-shot')];
    const lb = document.getElementById('lightbox');
    if (!shots.length || !lb) return;

    const img = lb.querySelector('img');
    const cap = lb.querySelector('p');
    const prev = lb.querySelector('.cp-arrow.prev');
    const next = lb.querySelector('.cp-arrow.next');
    const total = shots.length;
    const pad = n => String(n).padStart(2, '0');
    let idx = 0;
    let opener = null;

    prev.hidden = next.hidden = total < 2;

    function show(n) {
        idx = (n + total) % total;
        const src = shots[idx].querySelector('img');
        img.src = src.currentSrc || src.src;
        img.alt = src.alt;
        cap.replaceChildren(document.createTextNode(`${pad(idx + 1)} / ${pad(total)}`));
        if (src.dataset.caption) {
            const s = document.createElement('span');
            s.textContent = src.dataset.caption;
            cap.append(s);
        }
        [idx - 1, idx + 1].forEach(k => { new Image().src = shots[(k + total) % total].querySelector('img').src; });
    }

    function open(n) {
        opener = document.activeElement;
        show(n);
        lb.classList.add('open');
        lb.setAttribute('aria-hidden', 'false');
        document.body.style.overflow = 'hidden';
        lb.querySelector('.close').focus();
    }

    function close() {
        if (!lb.classList.contains('open')) return;
        lb.classList.remove('open');
        lb.setAttribute('aria-hidden', 'true');
        document.body.style.overflow = '';
        if (opener) opener.focus();
    }

    shots.forEach((s, i) => s.addEventListener('click', () => open(i)));
    lb.addEventListener('click', e => { if (e.target === lb) close(); });
    lb.querySelector('.close').addEventListener('click', close);
    prev.addEventListener('click', () => show(idx - 1));
    next.addEventListener('click', () => show(idx + 1));
    document.addEventListener('keydown', e => {
        if (!lb.classList.contains('open')) return;
        if (e.key === 'ArrowLeft') show(idx - 1);
        if (e.key === 'ArrowRight') show(idx + 1);
    });

    /* gesto de deslizar en móvil */
    let x0 = null;
    lb.addEventListener('touchstart', e => { x0 = e.touches[0].clientX; }, { passive: true });
    lb.addEventListener('touchend', e => {
        if (x0 === null) return;
        const dx = e.changedTouches[0].clientX - x0;
        if (Math.abs(dx) > 50) show(idx + (dx < 0 ? 1 : -1));
        x0 = null;
    });

    window.closeLightbox = close; /* site.js cierra con Escape */
})();
