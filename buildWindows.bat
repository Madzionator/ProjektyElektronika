dotnet publish -o ./build -r win-x64 --self-contained false -c Release -p:PublishSingleFile=true  -p:IncludeAllContentForSelfExtract=true /p:DebugType=None /p:DebugSymbols=false -o