::dotnet tool install --global dotnet-ef
::dotnet tool update --global dotnet-ef

:loop
set /p id="Migration className: "
cd ./CodeNotion.Academy.OrderScheduling
dotnet-ef migrations add %id% --context DatabaseContext
PAUSE
goto :loop