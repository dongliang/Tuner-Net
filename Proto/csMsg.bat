set PATH=ProtoGen;%PATH%

echo ~~~start~~~~


ProtoGen.exe -i:np_client.proto -o:TunerMsg.cs
copy /Y TunerMsg.cs ..\DotNet\Client
copy /Y TunerMsg.cs ..\DotNet\Server

echo ~~~end

pause
