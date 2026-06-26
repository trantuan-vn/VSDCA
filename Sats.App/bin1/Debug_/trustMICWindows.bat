ver | find "5.1"
if %ERRORLEVEL% == 0 goto ver_winxp
certutil.exe -addstore -f "Root" "MIC.cer"
certutil.exe -addstore -f "CA" "BkavCA.cer"
certutil.exe -addstore -f "CA" "BkavCA2.cer"
goto end
:ver_winxp
certutilwinxp.exe -addstore -f "Root" "MIC.cer"
certutilwinxp.exe -addstore -f "CA" "BkavCA.cer"
certutilwinxp.exe -addstore -f "CA" "BkavCA2.cer"
:end