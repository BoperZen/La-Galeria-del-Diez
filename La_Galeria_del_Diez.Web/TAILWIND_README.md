# ?? Tailwind CSS - Tema Vault-Tec

## ?? Tabla de Contenidos

- [Instalación](#instalación)
- [Configuración](#configuración)
- [Uso](#uso)
- [Colores Vault-Tec](#colores-vault-tec)
- [Componentes](#componentes)
- [Scripts NPM](#scripts-npm)
- [Modo Oscuro](#modo-oscuro)

---

## ?? Instalación

### Requisitos Previos
- Node.js 18+ instalado ([Descargar](https://nodejs.org/))
- .NET 9 SDK

### Instalación Rápida

```powershell
# Usando el script de instalación automática
.\setup-tailwind.ps1
```

### Instalación Manual

```powershell
# 1. Navegar al directorio del proyecto
cd La_Galeria_del_Diez.Web

# 2. Instalar dependencias
npm install

# 3. Compilar Tailwind CSS
npm run build
```

---

## ?? Configuración

### Archivos de Configuración

```
La_Galeria_del_Diez.Web/
??? package.json              # Dependencias NPM
??? tailwind.config.js        # Configuración de Tailwind
??? wwwroot/
?   ??? css/
?       ??? app.css          # Estilos fuente (EDITAR ESTE)
?       ??? output.css       # Compilado (NO EDITAR)
??? Views/
    ??? Shared/
        ??? _Layout.cshtml   # Layout principal
        ??? _Menu.cshtml     # Navbar
        ??? _Footer.cshtml   # Footer
```

### Variables CSS Personalizadas

Todas las variables de color se definen en `wwwroot/css/app.css`:

```css
@theme {
  --color-background: #0A0E11;    /* Fondo oscuro */
  --color-foreground: #E8E9EA;    /* Texto claro */
  --color-primary: #C89E2F;       /* Dorado Vault-Tec */
  --color-accent: #4A90E2;        /* Azul Vault-Tec */
  /* ... más colores */
}
```

---

## ?? Uso

### En tus vistas Razor (.cshtml)

```razor
<div class="bg-card rounded-lg shadow-lg p-6 border border-border">
    <h1 class="text-3xl font-bold text-primary mb-4">Título</h1>
    <p class="text-muted-foreground">Contenido...</p>
</div>
```

### Layout de Página Típico

```razor
@{
    ViewData["Title"] = "Mi Página";
}

<div class="min-h-screen bg-background py-8">
    <div class="container mx-auto px-4">
        <div class="bg-card rounded-xl shadow-2xl p-6 border border-border">
            <!-- Tu contenido aquí -->
        </div>
    </div>
</div>
```

---

## ?? Colores Vault-Tec

### Paleta Principal

| Color | Variable | Valor | Uso |
|-------|----------|-------|-----|
| ?? **Primary** | `bg-primary` | `#C89E2F` | Botones principales, acentos dorados |
| ?? **Accent** | `bg-accent` | `#4A90E2` | Enlaces, elementos interactivos |
| ? **Background** | `bg-background` | `#0A0E11` | Fondo principal |
| ? **Card** | `bg-card` | `#1A1E23` | Tarjetas, contenedores |
| ? **Foreground** | `text-foreground` | `#E8E9EA` | Texto principal |
| ??? **Muted** | `bg-muted` | `#2A2E33` | Fondos sutiles |
| ?? **Destructive** | `bg-destructive` | `#DC2626` | Acciones de eliminar |

### Ejemplos de Uso

```html
<!-- Botón Primary -->
<button class="px-6 py-2.5 bg-primary text-primary-foreground rounded-md hover:bg-secondary">
    Guardar
</button>

<!-- Badge Accent -->
<span class="px-3 py-1 rounded-full bg-accent/10 text-accent border border-accent/30">
    Activo
</span>

<!-- Card con borde -->
<div class="bg-card border border-border rounded-lg p-6">
    Contenido
</div>
```

---

## ?? Componentes

Ver el archivo `COMPONENTES_TAILWIND.html` para una librería completa de componentes reutilizables:

- ? Botones (Primary, Secondary, Outline, Ghost)
- ? Badges y Pills
- ? Cards (básicas, con header, con footer)
- ? Alerts (Info, Success, Warning, Error)
- ? Formularios (Inputs, Textarea, Select, Checkbox, Radio)
- ? Tablas estilizadas
- ? Loading Spinners
- ? Breadcrumbs
- ? Paginación

### Ejemplo: Botón Primary

```html
<button class="inline-flex items-center px-6 py-2.5 bg-primary text-primary-foreground rounded-md hover:opacity-90 transition-all duration-200 font-medium shadow-sm hover:shadow-md">
    <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path>
    </svg>
    Crear Nuevo
</button>
```

---

## ?? Scripts NPM

| Comando | Descripción |
|---------|-------------|
| `npm install` | Instala todas las dependencias |
| `npm run dev` | Compilación en modo desarrollo con watch (recompila automáticamente) |
| `npm run build` | Compilación optimizada y minificada para producción |

### Workflow de Desarrollo

```powershell
# Terminal 1: Mantén Tailwind compilando en vivo
npm run dev

# Terminal 2: Ejecuta tu aplicación ASP.NET
dotnet run
```

---

## ?? Modo Oscuro

El tema Vault-Tec ya está en modo oscuro por defecto. Si necesitas un modo claro:

### Activar Modo Claro (opcional)

En `_Layout.cshtml`:

```html
<!-- Remover o comentar esto en app.css -->
```

---

## ??? Personalización

### Agregar Nuevos Colores

1. Edita `wwwroot/css/app.css`:

```css
@theme {
  --color-success: #10B981;
  --color-warning: #F59E0B;
}
```

2. Recompila:

```powershell
npm run build
```

3. Usa en tus vistas:

```html
<div class="bg-success text-white">Éxito!</div>
```

### Cambiar Colores Existentes

Edita los valores en `@theme` dentro de `app.css` y recompila.

---

## ?? Dependencias

- **Tailwind CSS v4.2.1** - Framework CSS utility-first
- **@tailwindcss/cli** - CLI para compilar Tailwind v4
- **@tailwindcss/typography** - Plugin para contenido tipográfico
- **Alpine.js** - Framework JS ligero (para dropdowns en navbar)

---

## ?? Solución de Problemas

### El CSS no se actualiza

```powershell
# 1. Limpia y recompila
npm run build

# 2. Fuerza recarga en el navegador
Ctrl + F5
```

### Clases de Tailwind no funcionan

1. Verifica que `output.css` esté en `wwwroot/css/`
2. Asegúrate de que `_Layout.cshtml` incluya: `<link rel="stylesheet" href="~/css/output.css" />`
3. Ejecuta `npm run build`

### Errores de compilación

```powershell
# Reinstala dependencias
rm -rf node_modules
npm install
npm run build
```

---

## ?? Notas Importantes

- ?? **NO edites `output.css`** - Este archivo se genera automáticamente
- ?? **Edita solo `app.css`** - Este es tu archivo fuente
- ?? **Ejecuta `npm run build`** antes de hacer commit
- ?? **Agrega `output.css` al `.gitignore`** si no quieres versionarlo

---

## ?? Recursos Adicionales

- [Documentación de Tailwind CSS](https://tailwindcss.com/docs)
- [Tailwind v4 Alpha Docs](https://tailwindcss.com/docs/v4-beta)
- [Alpine.js Documentation](https://alpinejs.dev/)
- [Bootstrap Icons](https://icons.getbootstrap.com/)

---

## ????? Desarrollo

### Estructura Recomendada

```
Views/
??? Shared/
?   ??? _Layout.cshtml      # Layout principal
?   ??? _Menu.cshtml        # Navbar
?   ??? _Footer.cshtml      # Footer
??? User/
?   ??? Index.cshtml        # Ejemplo de vista con Tailwind
??? [OtrasVistas]/
    ??? *.cshtml            # Tus vistas personalizadas
```

### Convenciones de Clases

- Usa nombres semánticos: `bg-card`, `text-foreground`
- Prefiere utilidades de Tailwind sobre CSS personalizado
- Agrupa clases relacionadas: `"flex items-center space-x-4"`

---

**¡Listo para crear interfaces increíbles con Vault-Tec! ??**
