# Script para configurar Tailwind CSS en La Galeria del Diez

Write-Host "==================================" -ForegroundColor Cyan
Write-Host "Tailwind CSS Setup" -ForegroundColor Cyan
Write-Host "La Galeria del Diez" -ForegroundColor Cyan
Write-Host "==================================" -ForegroundColor Cyan
Write-Host ""

# Verificar si Node.js está instalado
Write-Host "Verificando Node.js..." -ForegroundColor Yellow
try {
    $nodeVersion = node --version
    Write-Host "? Node.js encontrado: $nodeVersion" -ForegroundColor Green
} catch {
    Write-Host "? Node.js NO encontrado" -ForegroundColor Red
    Write-Host "Por favor, instala Node.js desde: https://nodejs.org/" -ForegroundColor Yellow
    exit 1
}

# Verificar si npm está instalado
Write-Host "Verificando npm..." -ForegroundColor Yellow
try {
    $npmVersion = npm --version
    Write-Host "? npm encontrado: v$npmVersion" -ForegroundColor Green
} catch {
    Write-Host "? npm NO encontrado" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Instalando dependencias..." -ForegroundColor Yellow
npm install

if ($LASTEXITCODE -eq 0) {
    Write-Host "? Dependencias instaladas correctamente" -ForegroundColor Green
} else {
    Write-Host "? Error al instalar dependencias" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Compilando Tailwind CSS..." -ForegroundColor Yellow
npm run build

if ($LASTEXITCODE -eq 0) {
    Write-Host "? CSS compilado correctamente" -ForegroundColor Green
} else {
    Write-Host "? Error al compilar CSS" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "==================================" -ForegroundColor Cyan
Write-Host "¡Configuración completada!" -ForegroundColor Green
Write-Host "==================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Comandos disponibles:" -ForegroundColor Yellow
Write-Host "  npm run dev   - Compilar CSS en modo desarrollo (con watch)" -ForegroundColor White
Write-Host "  npm run build - Compilar CSS para producción (minificado)" -ForegroundColor White
Write-Host ""
Write-Host "Durante el desarrollo, ejecuta:" -ForegroundColor Yellow
Write-Host "  npm run dev" -ForegroundColor Cyan
Write-Host ""
Write-Host "Lee TAILWIND_README.md para más información" -ForegroundColor Yellow
