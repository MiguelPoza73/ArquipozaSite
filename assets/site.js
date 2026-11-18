
for(const form of document.querySelectorAll('[data-email-form]')) form.addEventListener('submit',e=>{e.preventDefault();const L={name:'Nombre',email:'Email',phone:'Teléfono',message:'Mensaje',projectType:'Tipo de proyecto',budget:'Presupuesto',timeline:'Plazo',description:'Descripción'};const body=Array.from(new FormData(form)).map(([k,v])=>{const el=form.elements[k];const val=el&&el.tagName==='SELECT'&&el.selectedIndex>0?el.options[el.selectedIndex].text:v;return (L[k]||k)+': '+val}).join('\n');location.href='mailto:arquipoza@hotmail.com?subject='+encodeURIComponent(form.dataset.emailForm==='Quote'?'Solicitud de presupuesto':'Contacto desde la web')+'&body='+encodeURIComponent(body)});
// ===== Portfolio: buscador, filtros por tipo y paginación =====
(() => {
    const grid = document.querySelector('#projectGrid');
    if (!grid) return;

    const PER_PAGE = 6;
    const params = new URLSearchParams(location.search);
    let type = params.get('type') || '';
    let query = params.get('search') || '';
    let page = 1;

    const cards = [...grid.querySelectorAll('.project-card')];
    const tools = document.querySelector('.pf-tools');
    const form = document.querySelector('#searchForm');
    const input = document.querySelector('#search');
    const clear = document.querySelector('.pf-clear');
    const filters = [...document.querySelectorAll('.pf-filters .filter')];
    const count = document.querySelector('#projectCount');
    const pager = document.querySelector('#pagination');
    const empty = document.querySelector('#pfEmpty');
    const reduceMotion = matchMedia('(prefers-reduced-motion: reduce)');

    // Búsqueda sin distinguir mayúsculas ni tildes, sobre los datos y el texto de cada ficha
    const norm = s => s.normalize('NFD').replace(/[̀-ͯ]/g, '').toLowerCase();
    const haystack = new Map(cards.map(c => [c, norm(`${c.dataset.search} ${c.textContent}`)]));
    const matches = c => {
        const words = norm(query).split(/\s+/).filter(Boolean);
        return words.every(w => haystack.get(c).includes(w));
    };

    function syncUrl() {
        const p = new URLSearchParams();
        if (type) p.set('type', type);
        if (query.trim()) p.set('search', query.trim());
        history.replaceState(null, '', p.toString() ? `?${p}` : location.pathname);
    }

    function render(scroll) {
        const bySearch = cards.filter(matches);
        const shown = bySearch.filter(c => !type || c.dataset.type === type);
        const pages = Math.max(1, Math.ceil(shown.length / PER_PAGE));
        page = Math.min(page, pages);

        cards.forEach(c => { c.hidden = true; });
        shown.slice((page - 1) * PER_PAGE, page * PER_PAGE).forEach(c => { c.hidden = false; });

        count.textContent = `${shown.length} ${shown.length === 1 ? 'proyecto' : 'proyectos'}`;
        filters.forEach(b => {
            const n = b.dataset.filter ? bySearch.filter(c => c.dataset.type === b.dataset.filter).length : bySearch.length;
            b.querySelector('sup').textContent = n;
            b.toggleAttribute('data-empty', n === 0);
            b.setAttribute('aria-pressed', String(b.dataset.filter === type));
        });
        empty.hidden = shown.length > 0;
        clear.hidden = !input.value;

        pager.replaceChildren();
        if (pages > 1) {
            for (let i = 1; i <= pages; i++) {
                const b = document.createElement('button');
                b.type = 'button';
                b.className = 'pf-page';
                b.textContent = i;
                b.setAttribute('aria-label', `Página ${i}`);
                if (i === page) b.setAttribute('aria-current', 'page');
                b.addEventListener('click', () => { if (i !== page) { page = i; render(true); } });
                pager.append(b);
            }
        }

        syncUrl();
        if (scroll) tools.scrollIntoView({ behavior: reduceMotion.matches ? 'auto' : 'smooth', block: 'start' });
    }

    input.value = query;

    let timer;
    input.addEventListener('input', () => {
        clearTimeout(timer);
        timer = setTimeout(() => { query = input.value; page = 1; render(); }, 150);
    });
    form.addEventListener('submit', e => {
        e.preventDefault();
        clearTimeout(timer);
        query = input.value; page = 1; render();
        input.blur(); // cierra el teclado en móvil
    });
    clear.addEventListener('click', () => {
        input.value = ''; query = ''; page = 1; render();
        input.focus();
    });
    filters.forEach(b => b.addEventListener('click', () => {
        type = b.dataset.filter; page = 1; render();
        b.scrollIntoView({ behavior: reduceMotion.matches ? 'auto' : 'smooth', block: 'nearest', inline: 'nearest' });
    }));
    empty.querySelector('.pf-reset').addEventListener('click', () => {
        input.value = ''; query = ''; type = ''; page = 1; render();
    });

    render();
    const active = filters.find(b => b.dataset.filter === type);
    if (active && active.parentElement.scrollWidth > active.parentElement.clientWidth) {
        active.parentElement.scrollLeft = active.offsetLeft - active.parentElement.offsetLeft;
    }
    requestAnimationFrame(() => requestAnimationFrame(() => tools.classList.add('is-ready')));
})();
document.addEventListener('keydown',e=>{if(e.key==='Escape'&&typeof closeLightbox==='function')closeLightbox()});
