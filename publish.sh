VERSION=0.1.0
dotnet publish PRUNner -c Release -o Release/PRUNner-$VERSION-linux-64 -f net5.0 -p:PublishReadyToRun=true -p:PublishSingleFile=true -p:PublishTrimmed=true -p:Version=$VERSION --self-contained true -r linux-x64 
dotnet publish PRUNner -c Release -o Release/PRUNner-$VERSION-win-x64 -f net5.0 -p:PublishReadyToRun=false -p:PublishSingleFile=true -p:PublishTrimmed=true -p:Version=$VERSION --self-contained true -r win-x64
dotnet publish PRUNner -c Release -o Release/PRUNner-$VERSION-osx-x64 -f net5.0 -p:PublishReadyToRun=false -p:PublishSingleFile=true -p:PublishTrimmed=true -p:Version=$VERSION --self-contained true -r osx-x64

cp -r FIOCache Release/PRUNner-$VERSION-linux-64/Cache
cp -r FIOCache Release/PRUNner-$VERSION-win-x64/Cache
cp -r FIOCache Release/PRUNner-$VERSION-osx-x64/Cache

echo "DONE! :)"