#!/bin/bash
# Script para instalar dependências do projeto Vue

echo "Instalando dependências do frontend..."
npm install

if [ $? -eq 0 ]; then
    echo "? Dependências instaladas com sucesso!"
    echo ""
    echo "Para rodar o servidor de desenvolvimento, use:"
    echo "  npm run dev"
else
    echo "? Erro ao instalar dependências"
    exit 1
fi
