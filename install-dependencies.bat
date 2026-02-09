@echo off
REM Script para instalar dependências do projeto Vue

echo Instalando dependências do frontend...
cd WorkforceHub.Client
npm install

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ? Dependências instaladas com sucesso!
    echo.
    echo Para rodar o servidor de desenvolvimento, use:
    echo   npm run dev
) else (
    echo.
    echo ? Erro ao instalar dependências
    exit /b 1
)
