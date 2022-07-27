VERSION=0.4.3
dotnet publish PRUNner -c Release -o Release/PRUNner-$VERSION-linux-64 -f net5.0 -p:PublishReadyToRun=true -p:PublishSingleFile=true -p:PublishTrimmed=true -p:Version=$VERSION --self-contained true -r linux-x64 
dotnet publish PRUNner -c Release -o Release/PRUNner-$VERSION-win-x64 -f net5.0 -p:PublishReadyToRun=false -p:PublishSingleFile=true -p:PublishTrimmed=true -p:Version=$VERSION --self-contained true -r win-x64
dotnet publish PRUNner -c Release -o Release/PRUNner-$VERSION-osx-x64 -f net5.0 -p:PublishReadyToRun=false -p:PublishSingleFile=true -p:PublishTrimmed=true -p:Version=$VERSION --self-contained true -r osx-x64

cp -r FIOCache Release/PRUNner-$VERSION-linux-64/FIOCache
cp -r FIOCache Release/PRUNner-$VERSION-win-x64/FIOCache
cp -r FIOCache Release/PRUNner-$VERSION-osx-x64/FIOCache

cd Release || exit
zip PRUNner-$VERSION-linux-64.zip -r PRUNner-$VERSION-linux-64
zip PRUNner-$VERSION-win-x64.zip -r PRUNner-$VERSION-win-x64
zip PRUNner-$VERSION-osx-x64.zip -r PRUNner-$VERSION-osx-x64

echo "DONE! :)"