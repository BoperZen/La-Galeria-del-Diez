# ?? La Galería del Diez

Sistema de gestión de librería desarrollado con **ASP.NET Core 9** y **Tailwind CSS** con tema Vault-Tec.

## ?? Características

- ? **Gestión de Usuarios** - Sistema completo de administración de usuarios
- ? **Catálogo de Libros** - Búsqueda y navegación de libros
- ? **Gestión de Autores** - Administración de autores y sus obras
- ? **Sistema de Órdenes** - Procesamiento de pedidos
- ? **Reportes** - Gráficos y estadísticas de ventas
- ? **Tema Vault-Tec** - Diseño oscuro elegante con Tailwind CSS

## ??? Tecnologías

### Backend
- **ASP.NET Core 9.0**
- **Entity Framework Core**
- **C# 13.0**
- **Razor Pages / MVC**

### Frontend
- **Tailwind CSS v4.2.1**
- **Alpine.js** (para interactividad)
- **Bootstrap Icons**
- **SweetAlert2** (notificaciones)

### Herramientas
- **Node.js 18+** (para Tailwind)
- **npm** (gestor de paquetes)

## ?? Instalación

### Requisitos Previos
- .NET 9 SDK
- Node.js 18+
- SQL Server (o tu base de datos preferida)

### Paso 1: Clonar el repositorio
```bash
git clone https://github.com/BoperZen/La-Galeria-del-Diez.git
cd La-Galeria-del-Diez
```

### Paso 2: Configurar la base de datos
```bash
# Actualiza la cadena de conexión en appsettings.json
# Luego ejecuta las migraciones
dotnet ef database update
```

### Paso 3: Instalar dependencias de Tailwind
```bash
cd La_Galeria_del_Diez.Web
npm install
npm run build
```

### Paso 4: Ejecutar la aplicación
```bash
dotnet run
```

La aplicación estará disponible en `https://localhost:5001`

## ?? Estructura del Proyecto

```
La-Galeria-del-Diez/
??? La_Galeria_del_Diez.Application/    # Lógica de negocio y DTOs
??? La_Galeria_del_Diez.Infraestructure/ # Acceso a datos y repositorios
??? La_Galeria_del_Diez.Web/            # Capa de presentación
    ??? Controllers/                     # Controladores MVC
    ??? Views/                           # Vistas Razor
    ?   ??? Shared/
    ?   ?   ??? _Layout.cshtml
    ?   ?   ??? _Menu.cshtml
    ?   ?   ??? _Footer.cshtml
    ?   ??? User/
    ?       ??? Index.cshtml
    ??? wwwroot/
    ?   ??? css/
    ?   ?   ??? app.css                 # CSS fuente (editar este)
    ?   ?   ??? output.css              # CSS compilado (generado)
    ?   ??? images/
    ??? package.json                     # Dependencias NPM
    ??? tailwind.config.js              # Configuración de Tailwind
    ??? TAILWIND_README.md              # Documentación de Tailwind
    ??? GUIA_ESTILO.md                  # Guía de estilos
    ??? COMPONENTES_TAILWIND.html      # Librería de componentes
```

## ?? Sistema de Diseño

El proyecto usa un tema personalizado **Vault-Tec** con:

- **Fondo oscuro**: `#0A0E11`
- **Dorado primario**: `#C89E2F` (Vault Gold)
- **Azul accent**: `#4A90E2` (Vault Blue)
- **Cards**: `#1A1E23`
- **Texto claro**: `#E8E9EA`

Ver la [Guía de Estilo](La_Galeria_del_Diez.Web/GUIA_ESTILO.md) para más detalles.

## ?? Desarrollo

### Compilar Tailwind en modo watch
```bash
cd La_Galeria_del_Diez.Web
npm run dev
```

### Ejecutar la aplicación
```bash
dotnet run --project La_Galeria_del_Diez.Web
```

### Compilar para producción
```bash
npm run build
dotnet publish -c Release
```

## ?? Documentación

- [Guía de Tailwind CSS](La_Galeria_del_Diez.Web/TAILWIND_README.md)
- [Guía de Estilo](La_Galeria_del_Diez.Web/GUIA_ESTILO.md)
- [Componentes Reutilizables](La_Galeria_del_Diez.Web/COMPONENTES_TAILWIND.html)

## ?? Scripts Útiles

| Comando | Descripción |
|---------|-------------|
| `npm run dev` | Compila Tailwind en modo watch |
| `npm run build` | Compila Tailwind para producción |
| `dotnet run` | Ejecuta la aplicación |
| `dotnet build` | Compila el proyecto .NET |
| `dotnet ef database update` | Actualiza la base de datos |

## ?? Características del Diseño

### Navbar Moderno
- Logo a la izquierda
- Barra de búsqueda central
- Iconos de notificaciones y configuración
- Menú de usuario con dropdown
- Navegación secundaria con links principales

### Footer Elaborado
- 4 columnas de información
- Links a redes sociales
- Información de contacto
- Enlaces rápidos

### Componentes Reutilizables
- Botones (Primary, Secondary, Outline, Destructive)
- Cards (básicas, con header, con footer)
- Formularios estilizados
- Tablas responsivas
- Badges y pills
- Alerts

## ?? Solución de Problemas

### CSS no se actualiza
```bash
npm run build
# Forzar recarga en navegador: Ctrl + F5
```

### Error de compilación de Tailwind
```bash
rm -rf node_modules
npm install
npm run build
```

### Error de base de datos
```bash
dotnet ef database drop
dotnet ef database update
```

## ?? Contribuir

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## ?? Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## ?? Equipo

- **Desarrollo**: La Galería del Diez Team
- **Diseño**: Tema Vault-Tec

## ?? Agradecimientos

- [Tailwind CSS](https://tailwindcss.com/)
- [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet)
- [Bootstrap Icons](https://icons.getbootstrap.com/)
- [Alpine.js](https://alpinejs.dev/)
- [SweetAlert2](https://sweetalert2.github.io/)

---

**¡Hecho con ?? usando ASP.NET Core y Tailwind CSS!**
