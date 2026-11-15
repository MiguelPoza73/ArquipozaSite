# Arquipoza

Web de un estudio de arquitectura con portfolio de proyectos, galería de bocetos, seguimiento de obras e información de contacto.

🌐 **[Visitar la web](https://miguelpoza73.github.io/ArquipozaSite/)** — Acceso público desde cualquier navegador.

Sitio estático (HTML, CSS, Bootstrap y JavaScript) con imágenes optimizadas en WebP, publicado con **GitHub Pages**: cada cambio que se sube a `main` se publica automáticamente.

Incluye búsqueda y filtros de proyectos, fichas de detalle y carruseles. Los formularios abren la aplicación de correo del visitante.

## Estructura

- `index.html`, `about/`, `contact/`, `quote/`, `sketchbook/`, `portfolio/` — páginas
- `assets/` — estilos y scripts
- `media/` — imágenes

Las rutas son relativas, así que se puede abrir en local con cualquier servidor estático (por ejemplo `python -m http.server`).

La versión original en .NET 8 + Orchard Core se conserva en la etiqueta `dotnet-original`.
