@echo off
REM Build do MSIX - SlSS Acompanhamento de consultas
REM Pre-requisitos: .NET 8 SDK + Windows SDK (MakeAppx.exe + SignTool.exe)
REM O PFX de TESTE fica em msix\wavizo-test.pfx (senha: wavizo-test-2026).
REM Para a Store, o Partner Center assina com o certificado oficial.
setlocal
set KIT=C:\Program Files (x86)\Windows Kits\10\bin\10.0.26100.0\x64
set ROOT=C:\APPS\AcompanhamentoConsultas

REM 1. Publica o host WebView2 SELF-CONTAINED x64 (roda sem .NET instalado)
dotnet publish "%ROOT%\src\AcompanhamentoConsultas.csproj" -c Release
if errorlevel 1 exit /b 1

REM 2. Copia os binarios para o pacote (wwwroot eh espelhado no passo 3)
xcopy "%ROOT%\src\bin\Release\net8.0-windows\win-x64\publish\*" "%ROOT%\msix\package\" /E /Y /EXCLUDE:nul
if errorlevel 1 exit /b 1

REM 3. Espelha o PWA na pasta do pacote (fonte da verdade = raiz do projeto)
xcopy "%ROOT%\index.html" "%ROOT%\msix\package\wwwroot\" /Y
xcopy "%ROOT%\manifest.json" "%ROOT%\msix\package\wwwroot\" /Y
xcopy "%ROOT%\sw.js" "%ROOT%\msix\package\wwwroot\" /Y
xcopy "%ROOT%\icons\*.png" "%ROOT%\msix\package\wwwroot\icons\" /Y

REM 4. Empacota
"%KIT%\makeappx.exe" pack /d "%ROOT%\msix\package" /p "%ROOT%\AcompanhamentoConsultas.msix"
if errorlevel 1 exit /b 1

REM 5. Assina (certificado de TESTE auto-assinado, CN igual ao Publisher oficial)
"%KIT%\signtool.exe" sign /fd SHA256 /f "%ROOT%\msix\wavizo-test.pfx" /p wavizo-test-2026 "%ROOT%\AcompanhamentoConsultas.msix"
if errorlevel 1 exit /b 1

REM 6. Copia de backup dentro de msix\
copy /Y "%ROOT%\AcompanhamentoConsultas.msix" "%ROOT%\msix\AcompanhamentoConsultas.msix"
echo BUILD OK: %ROOT%\AcompanhamentoConsultas.msix
