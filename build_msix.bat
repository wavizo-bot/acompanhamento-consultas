@echo off
REM Build do MSIX - Acompanhamento de Consultas
REM Pre-requisito: Windows SDK (MakeAppx.exe + SignTool.exe)
REM O PFX de TESTE fica em msix\wavizo-test.pfx (senha: wavizo-test-2026).
REM Para a Store, o Partner Center assina com o certificado oficial.
setlocal
set KIT=C:\Program Files (x86)\Windows Kits\10\bin\10.0.26100.0\x64
set ROOT=C:\APPS\AcompanhamentoConsultas

REM 1. Espelha o PWA na pasta do pacote (fonte da verdade = raiz do projeto)
xcopy "%ROOT%\index.html" "%ROOT%\msix\package\wwwroot\" /Y
xcopy "%ROOT%\manifest.json" "%ROOT%\msix\package\wwwroot\" /Y
xcopy "%ROOT%\sw.js" "%ROOT%\msix\package\wwwroot\" /Y
xcopy "%ROOT%\icons\*.png" "%ROOT%\msix\package\wwwroot\icons\" /Y

REM 2. Empacota
"%KIT%\makeappx.exe" pack /d "%ROOT%\msix\package" /p "%ROOT%\AcompanhamentoConsultas.msix" /v
if errorlevel 1 exit /b 1

REM 3. Assina (certificado de TESTE auto-assinado, CN igual ao Publisher oficial)
"%KIT%\signtool.exe" sign /fd SHA256 /f "%ROOT%\msix\wavizo-test.pfx" /p wavizo-test-2026 "%ROOT%\AcompanhamentoConsultas.msix"
if errorlevel 1 exit /b 1

REM 4. Copia de backup dentro de msix\
copy /Y "%ROOT%\AcompanhamentoConsultas.msix" "%ROOT%\msix\AcompanhamentoConsultas.msix"
echo BUILD OK: %ROOT%\AcompanhamentoConsultas.msix
