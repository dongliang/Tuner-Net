set PATH=ProtoGen;%PATH%

echo ~~~start~~~~


ProtoGen.exe -i:np_client.proto -o:TunerMsg.cs
copy /Y TunerMsg.cs ..\DotNet\Server
copy /Y TunerMsg.cs ..\Unity\Assets

echo ~~~end

pause
