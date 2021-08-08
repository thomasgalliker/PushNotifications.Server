
DEPLOYMENT
----------

dotnet publish .\Externals\PushNotifications.Console -r win10-x64 --self-contained=false /p:Configuration=Release /p:PublishSingleFile=true /p:PublishDir=bin\Release\netcoreapp3.1\publish\


APP SETTINGS
------------

Copy appsettings.json file to .exe and use it as base configuration.