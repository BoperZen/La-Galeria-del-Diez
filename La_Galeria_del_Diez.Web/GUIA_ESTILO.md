# ?? Guía de Estilo - La Galería del Diez

## ?? Filosofía de Diseño

El diseño de La Galería del Diez se basa en el tema **Vault-Tec**, con una paleta oscura elegante y acentos dorados que evocan lujo y sofisticación.

---

## ?? Principios de Diseño

### 1. Consistencia
- Usa siempre las mismas clases para elementos similares
- Mantén el mismo espaciado en toda la aplicación
- Reutiliza componentes de `COMPONENTES_TAILWIND.html`

### 2. Jerarquía Visual
- **Títulos**: `text-3xl font-bold text-foreground`
- **Subtítulos**: `text-xl font-semibold text-foreground`
- **Texto normal**: `text-sm text-foreground`
- **Texto secundario**: `text-sm text-muted-foreground`

### 3. Espaciado
```html
<!-- Contenedores principales -->
<div class="container mx-auto px-4 py-8">

<!-- Cards -->
<div class="p-6">

<!-- Elementos pequeños -->
<div class="p-2">

<!-- Entre elementos -->
<div class="space-y-4">  <!-- Vertical -->
<div class="space-x-4">  <!-- Horizontal -->
```

---

## ??? Uso de Colores

### Colores Primarios

```html
<!-- Dorado Vault-Tec - Para acciones importantes -->
<button class="bg-primary text-primary-foreground">Guardar</button>

<!-- Azul Accent - Para links y elementos interactivos -->
<a class="text-accent hover:text-accent/80">Ver más</a>

<!-- Destructivo - Para eliminar -->
<button class="bg-destructive/10 text-red-600">Eliminar</button>
```

### Fondos

```html
<!-- Fondo principal -->
<div class="bg-background">

<!-- Cards -->
<div class="bg-card">

<!-- Elementos sutiles -->
<div class="bg-muted">
```

### Texto

```html
<!-- Texto principal -->
<p class="text-foreground">

<!-- Texto secundario -->
<p class="text-muted-foreground">

<!-- Sobre fondo dorado -->
<span class="text-primary-foreground">
```

---

## ?? Botones

### Botón Primary (Acción Principal)
```html
<button class="px-6 py-2.5 bg-primary text-primary-foreground rounded-md hover:bg-secondary transition-all font-medium shadow-sm hover:shadow-md">
    Guardar
</button>
```

### Botón Secondary (Acción Secundaria)
```html
<button class="px-6 py-2.5 bg-muted text-foreground rounded-md hover:bg-muted/80 transition-all font-medium">
    Cancelar
</button>
```

### Botón Outline
```html
<button class="px-6 py-2.5 border-2 border-primary text-primary rounded-md hover:bg-primary hover:text-primary-foreground transition-all font-medium">
    Ver Detalles
</button>
```

### Botón Destructivo
```html
<button class="px-6 py-2.5 bg-red-600 text-white rounded-md hover:bg-red-700 transition-all font-medium">
    Eliminar
</button>
```

---

## ?? Cards

### Card Básica
```html
<div class="bg-card rounded-lg shadow-lg p-6 border border-border">
    <h2 class="text-xl font-bold text-foreground mb-4">Título</h2>
    <p class="text-muted-foreground">Contenido...</p>
</div>
```

### Card con Header
```html
<div class="bg-card rounded-xl shadow-2xl border border-border overflow-hidden">
    <div class="bg-muted px-6 py-4 border-b border-border">
        <h2 class="text-xl font-bold text-foreground">Título</h2>
    </div>
    <div class="p-6">
        <p class="text-foreground">Contenido...</p>
    </div>
</div>
```

---

## ?? Formularios

### Input Text
```html
<div class="mb-4">
    <label class="block text-sm font-medium text-foreground mb-2">
        Nombre
    </label>
    <input type="text" 
           class="w-full px-4 py-2 bg-muted border border-border rounded-md text-foreground placeholder-placeholder focus:outline-none focus:ring-2 focus:ring-ring focus:border-transparent"
           placeholder="Ingresa tu nombre">
</div>
```

### Select
```html
<select class="w-full px-4 py-2 bg-muted border border-border rounded-md text-foreground focus:outline-none focus:ring-2 focus:ring-ring">
    <option>Selecciona una opción</option>
    <option>Opción 1</option>
</select>
```

---

## ??? Badges

### Badge Status
```html
<!-- Activo -->
<span class="inline-flex items-center px-3 py-1 rounded-full text-xs font-medium bg-green-500/20 text-green-400 border border-green-500/30">
    Activo
</span>

<!-- Inactivo -->
<span class="inline-flex items-center px-3 py-1 rounded-full text-xs font-medium bg-red-500/20 text-red-400 border border-red-500/30">
    Inactivo
</span>

<!-- Pendiente -->
<span class="inline-flex items-center px-3 py-1 rounded-full text-xs font-medium bg-yellow-500/20 text-yellow-400 border border-yellow-500/30">
    Pendiente
</span>
```

---

## ?? Tablas

```html
<div class="overflow-x-auto rounded-md border border-border">
    <table class="w-full">
        <thead class="bg-muted border-b border-border">
            <tr>
                <th class="px-6 py-4 text-left text-sm font-bold text-foreground">
                    Columna
                </th>
            </tr>
        </thead>
        <tbody class="divide-y divide-border">
            <tr class="hover:bg-muted/50 transition-colors">
                <td class="px-6 py-4 text-sm text-foreground">
                    Dato
                </td>
            </tr>
        </tbody>
    </table>
</div>
```

---

## ?? Transiciones y Animaciones

```html
<!-- Hover suave -->
<div class="transition-colors duration-200 hover:bg-muted">

<!-- Hover con múltiples propiedades -->
<div class="transition-all duration-200 hover:shadow-lg hover:scale-105">

<!-- Fade in -->
<div class="opacity-0 animate-fade-in">
```

---

## ?? Espaciado Estándar

| Tamaño | Clase | Uso |
|--------|-------|-----|
| Muy pequeño | `p-2` `gap-2` | Elementos muy compactos |
| Pequeño | `p-4` `gap-4` | Elementos normales |
| Mediano | `p-6` `gap-6` | Cards, secciones |
| Grande | `p-8` `gap-8` | Contenedores principales |

---

## ?? Iconos

Usamos **Bootstrap Icons**:

```html
<!-- Icono simple -->
<i class="bi bi-search"></i>

<!-- Icono con tamaño -->
<i class="bi bi-heart text-xl text-destructive"></i>

<!-- SVG personalizado -->
<svg class="w-6 h-6 text-primary" fill="none" stroke="currentColor" viewBox="0 0 24 24">
    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="..."></path>
</svg>
```

---

## ? Checklist de Diseño

Antes de implementar un componente, verifica:

- [ ] ¿Usa colores de la paleta Vault-Tec?
- [ ] ¿Tiene estados hover/focus adecuados?
- [ ] ¿Es responsive (se ve bien en móvil)?
- [ ] ¿Sigue el espaciado estándar?
- [ ] ¿Usa las fuentes correctas?
- [ ] ¿Tiene transiciones suaves?
- [ ] ¿Es accesible (contraste, alt text)?

---

## ?? Anti-patrones (Evitar)

```html
<!-- ? NO: Colores hardcodeados -->
<div class="bg-[#C89E2F]">

<!-- ? SÍ: Usa variables semánticas -->
<div class="bg-primary">

<!-- ? NO: Espaciado inconsistente -->
<div class="p-3">

<!-- ? SÍ: Usa valores estándar -->
<div class="p-4">

<!-- ? NO: Demasiadas clases inline -->
<div class="bg-card p-6 rounded-lg shadow-lg border border-border hover:shadow-xl transition-all duration-200 ...">

<!-- ? SÍ: Extrae a componente reutilizable -->
@await Html.PartialAsync("_CardComponent")
```

---

## ?? Responsive Design

```html
<!-- Ocultar en móvil, mostrar en desktop -->
<div class="hidden md:block">

<!-- Grid responsive -->
<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">

<!-- Padding responsive -->
<div class="px-4 md:px-8 lg:px-12">

<!-- Texto responsive -->
<h1 class="text-2xl md:text-3xl lg:text-4xl">
```

---

**Mantén la consistencia, usa los componentes existentes, y crea experiencias visuales increíbles! ??**
