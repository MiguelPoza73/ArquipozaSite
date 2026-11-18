# Arquipoza

Web de un estudio de arquitectura con portfolio de proyectos, galería de bocetos, seguimiento de obras e información de contacto.

🌐 **[Visitar la web](https://miguelpoza73.github.io/ArquipozaSite/)** — Acceso público desde cualquier navegador.

Sitio estático (HTML, CSS, Bootstrap y JavaScript) con imágenes optimizadas en WebP, generado con **Eleventy** y publicado con **GitHub Pages**: cada cambio que llega a `main` (incluidos los hechos desde el panel) se publica automáticamente en 1-2 minutos.

Incluye búsqueda y filtros de proyectos, fichas de detalle con galería, carruseles y un panel de contenidos en `/admin`. Los formularios abren la aplicación de correo del visitante.

## Cómo añadir o cambiar proyectos (sin tocar código)

El panel lo usa solo el mantenedor del repositorio. Aunque la página `/admin` es pública, nadie puede ver borradores ni guardar cambios sin un token con permiso de escritura en este repositorio. Por eso no hay que dar acceso a nadie más.

1. Entra en **https://miguelpoza73.github.io/ArquipozaSite/admin/**.
2. Pulsa **«Iniciar sesión con token de acceso»** (*Sign In Using Access Token*) y pega tu token (ver abajo). Solo hace falta la primera vez en cada navegador.
3. En **Proyectos** pulsa **Nuevo proyecto**, o abre uno existente para editarlo.
   - Rellena título, tipo, ubicación, año, superficie y la **foto de portada**.
   - En **Galería** añade más fotos; se ordenan arrastrándolas.
   - **Destacado en la portada**: lo muestra en la página de inicio (máximo 6).
   - **Orden**: números bajos salen primero; los nuevos tienen 0 y aparecen al principio.
   - **Borrador**: guárdalo sin publicarlo todavía.
4. Pulsa **Guardar**. La web se actualiza sola en 1-2 minutos.
5. En **Bocetos** puedes añadir, quitar y reordenar los dibujos de la página «Bocetos».

Las fotos se convierten automáticamente a WebP y se reducen a 2400 px, así que se pueden subir directamente desde la cámara o el móvil. Para borrar un proyecto, ábrelo y usa **Borrar**; su dirección antigua llevará al listado de proyectos.

### Obtener el token de acceso (una sola vez)
Con la cuenta propietaria del repositorio:
1. GitHub → *Settings* → *Developer settings* → *Personal access tokens* → **Fine-grained tokens** → *Generate new token*.
2. *Repository access*: **Only select repositories** → `ArquipozaSite`.
3. *Permissions* → *Repository permissions* → **Contents: Read and write**.
4. Pon una fecha de caducidad (por ejemplo, 1 año), genera el token, cópialo y pégalo en el panel. Guárdalo en el gestor de contraseñas. Cuando caduque, crea otro igual.

Si el estudio te envía material, basta con que te mande las fotos y los datos (por correo o en una carpeta compartida) y los subes tú desde el panel.

## Estructura

- `content/proyectos/*.md` — un archivo por proyecto (lo edita el panel)
- `_data/bocetos.json` — lista de bocetos (lo edita el panel)
- `_data/redirecciones.json` — direcciones antiguas de proyectos → nuevas
- `index.njk`, `portfolio/*.njk`, `sketchbook/index.njk` — plantillas de las páginas con contenido
- `_includes/` — piezas reutilizables (tarjeta de proyecto)
- `about/`, `contact/`, `quote/` — páginas estáticas que se copian tal cual
- `assets/` — estilos y scripts · `media/` — imágenes (`media/uploads/` para lo subido desde el panel)
- `admin/` — panel de contenidos ([Sveltia CMS](https://github.com/sveltia/sveltia-cms))
- `.github/workflows/deploy.yml` — genera y publica la web

## Desarrollo local

```bash
npm install
npm start          # http://localhost:8080 con recarga automática
npm run build      # genera la web en _site/
```

El panel también funciona en local: abre `http://localhost:8080/admin/` y elige **Trabajar con repositorio local** (*Work with Local Repository*) para editar los archivos sin pasar por GitHub.

La versión original en .NET 8 + Orchard Core se conserva en la etiqueta `dotnet-original`.
