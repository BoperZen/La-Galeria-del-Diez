/** @type {import('tailwindcss').Config} */
module.exports = {
  // Rutas donde Tailwind buscará clases
  content: [
    "./Views/**/*.cshtml",
    "./Pages/**/*.cshtml",
    "./wwwroot/**/*.html",
    "./wwwroot/**/*.js"
  ],
  
  // Modo oscuro basado en clase
  darkMode: 'class',
  
  theme: {
    extend: {
      // Colores personalizados Vault-Tec
      colors: {
        // Colores base
        background: 'var(--color-background)',
        foreground: 'var(--color-foreground)',
        
        // Cards y popovers
        card: {
          DEFAULT: 'var(--color-card)',
          foreground: 'var(--color-card-foreground)',
        },
        popover: {
          DEFAULT: 'var(--color-popover)',
          foreground: 'var(--color-popover-foreground)',
        },
        
        // Colores de marca
        primary: {
          DEFAULT: 'var(--color-primary)',      // Dorado Vault-Tec
          foreground: 'var(--color-primary-foreground)',
        },
        secondary: {
          DEFAULT: 'var(--color-secondary)',    // Dorado claro
          foreground: 'var(--color-secondary-foreground)',
        },
        
        // Colores de UI
        muted: {
          DEFAULT: 'var(--color-muted)',
          foreground: 'var(--color-muted-foreground)',
        },
        accent: {
          DEFAULT: 'var(--color-accent)',       // Azul Vault-Tec
          foreground: 'var(--color-accent-foreground)',
        },
        destructive: {
          DEFAULT: 'var(--color-destructive)',  // Rojo para eliminar
        },
        
        // Elementos de formulario
        border: 'var(--color-border)',
        input: 'var(--color-input)',
        ring: 'var(--color-ring)',
        
        // Colores para gráficos
        chart: {
          1: 'var(--color-chart-1)',
          2: 'var(--color-chart-2)',
          3: 'var(--color-chart-3)',
          4: 'var(--color-chart-4)',
          5: 'var(--color-chart-5)',
        },
        
        // Sidebar (si se usa)
        sidebar: {
          DEFAULT: 'var(--color-sidebar)',
          foreground: 'var(--color-sidebar-foreground)',
          primary: 'var(--color-sidebar-primary)',
          'primary-foreground': 'var(--color-sidebar-primary-foreground)',
          accent: 'var(--color-sidebar-accent)',
          'accent-foreground': 'var(--color-sidebar-accent-foreground)',
          border: 'var(--color-sidebar-border)',
          ring: 'var(--color-sidebar-ring)',
        },
      },
      
      // Border radius personalizado
      borderRadius: {
        lg: 'var(--radius-lg)',
        md: 'var(--radius-md)',
        sm: 'var(--radius-sm)',
      },
      
      // Fuentes personalizadas
      fontFamily: {
        sans: 'var(--font-sans)',
        serif: 'var(--font-serif)',
        mono: 'var(--font-mono)',
      },
      
      // Sombras personalizadas
      boxShadow: {
        '2xs': 'var(--shadow-2xs)',
        xs: 'var(--shadow-xs)',
        sm: 'var(--shadow-sm)',
        DEFAULT: 'var(--shadow)',
        md: 'var(--shadow-md)',
        lg: 'var(--shadow-lg)',
        xl: 'var(--shadow-xl)',
        '2xl': 'var(--shadow-2xl)',
      },
    },
  },
  
  // Plugins de Tailwind
  plugins: [
    require('tailwindcss-animate'),      // Animaciones
    require('@tailwindcss/typography'),  // Tipografía mejorada
  ],
}
