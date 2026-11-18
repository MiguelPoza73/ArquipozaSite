// Genera la web a partir de los proyectos y bocetos que el arquitecto edita en /admin.
// Las páginas usan rutas relativas (variable `root`), como el resto del sitio.

// Tipos de proyecto: etiqueta en el panel -> valor de data-type / ?type= (no cambiar los valores)
const TIPOS = [
    { es: 'Residencial', en: 'Residential' },
    { es: 'Comercial', en: 'Commercial' },
    { es: 'Cultural', en: 'Cultural' },
    { es: 'Industrial', en: 'Industrial' },
    { es: 'Urbano', en: 'Urban' },
];

export default function (eleventyConfig) {
    // Ficheros que se copian tal cual
    for (const dir of ['assets', 'media', 'admin', 'about', 'contact', 'quote']) {
        eleventyConfig.addPassthroughCopy(dir);
    }
    eleventyConfig.ignores.add('README.md');
    eleventyConfig.ignores.add('admin/**');

    eleventyConfig.addGlobalData('tipos', TIPOS);

    // Proyectos publicados: primero `orden` (menor = antes), después los más recientes
    eleventyConfig.addCollection('proyectos', api =>
        api.getFilteredByGlob('content/proyectos/*.md')
            .filter(p => !p.data.borrador)
            .sort((a, b) => (a.data.orden ?? 0) - (b.data.orden ?? 0) || (b.data.anio ?? 0) - (a.data.anio ?? 0)));

    // Direcciones antiguas (/portfolio/<id>/) de _data/redirecciones.json -> ficha actual.
    // Si el proyecto se borra o pasa a borrador, se redirige al listado.
    eleventyConfig.addCollection('redirecciones', api => {
        const vivos = new Set(api.getFilteredByGlob('content/proyectos/*.md')
            .filter(p => !p.data.borrador).map(p => p.page.fileSlug));
        const mapa = api.getAll()[0]?.data.redirecciones || {};
        return Object.entries(mapa).map(([id, slug]) => ({ id, destino: vivos.has(slug) ? `../${slug}/` : '../' }));
    });

    // "/media/x.webp" -> "../media/x.webp" según la profundidad de la página
    eleventyConfig.addFilter('rel', (src, root) => {
        if (!src) return '';
        if (/^(https?:)?\/\//.test(src)) return src;
        return root + String(src).replace(/^\/+/, '');
    });
    eleventyConfig.addFilter('tipoEn', es => (TIPOS.find(t => t.es === es) || {}).en || es);
    eleventyConfig.addFilter('ciudad', s => String(s || '').split(',')[0].trim());
    eleventyConfig.addFilter('pad', n => String(n).padStart(2, '0'));
    eleventyConfig.addFilter('contarTipo', (proyectos, es) => proyectos.filter(p => p.data.tipo === es).length);
    eleventyConfig.addFilter('destacados', proyectos => {
        const marcados = proyectos.filter(p => p.data.destacado);
        return (marcados.length ? marcados : proyectos).slice(0, 6);
    });
    eleventyConfig.addFilter('miles', n => Number(n).toLocaleString('es-ES'));

    return {
        dir: { input: '.', includes: '_includes', data: '_data', output: '_site' },
        templateFormats: ['njk', 'md'],
        markdownTemplateEngine: false,
    };
}
