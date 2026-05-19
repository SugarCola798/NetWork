@echo off
setlocal EnableExtensions EnableDelayedExpansion

rem Proto -> C# generator for Unity (Google.Protobuf)
rem Run from: Tools\GenProto.bat
rem protoc  : Tools\protoc.exe  (+ optional Tools\include)
rem Input   : Assets\Proto
rem Output  : Assets\Scripts\Gen
rem Add to .proto: option csharp_namespace = "ProtoMsg";

set "TOOLS=%~dp0"
cd /d "%TOOLS%.."
set "ROOT=%CD%"

set "PROTO_DIR=%ROOT%\Assets\Proto"
set "OUT_DIR=%ROOT%\Assets\Scripts\Gen"
set "PROTOC=%TOOLS%protoc.exe"
set "PROTO_INCLUDE=%TOOLS%include"

if not exist "%PROTOC%" (
    echo [ERROR] protoc.exe not found: %PROTOC%
    echo Put protoc.exe into Tools folder.
    echo Download: https://github.com/protocolbuffers/protobuf/releases
    pause
    exit /b 1
)

if not exist "%PROTO_DIR%" mkdir "%PROTO_DIR%"
if not exist "%OUT_DIR%" mkdir "%OUT_DIR%"

set "PROTO_ARGS="
for /r "%PROTO_DIR%" %%f in (*.proto) do (
    set "PROTO_ARGS=!PROTO_ARGS! "%%f""
)

if not defined PROTO_ARGS (
    echo [WARN] No .proto files in %PROTO_DIR%
    pause
    exit /b 0
)

echo.
echo ===== Proto to C# =====
echo Input : %PROTO_DIR%
echo Output: %OUT_DIR%
echo.

if exist "%PROTO_INCLUDE%" (
    "%PROTOC%" --proto_path="%PROTO_DIR%" --proto_path="%PROTO_INCLUDE%" --csharp_out="%OUT_DIR%" !PROTO_ARGS!
) else (
    "%PROTOC%" --proto_path="%PROTO_DIR%" --csharp_out="%OUT_DIR%" !PROTO_ARGS!
)

if errorlevel 1 (
    echo.
    echo [ERROR] protoc failed
    pause
    exit /b 1
)

echo.
echo [OK] Generated C# files in %OUT_DIR%
echo       Runtime: Assets\Plugins\Google.Protobuf (3.21.x)
echo       protoc : Tools\protoc.exe (3.21.x recommended)
pause
exit /b 0
