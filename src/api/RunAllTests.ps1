Write-Host "Running Domain Tests" -Foreground DarkYellow
dotnet run --project ./DomainTests/DomainTests.fsproj
Write-Host "Running Infrastructure Tests" -Foreground DarkYellow
dotnet run --project ./InfrastructureTests/InfrastructureTests.fsproj
Write-Host "Running Application Tests" -Foreground DarkYellow
dotnet run --project ./ApiTests/ApiTests.fsproj
Write-Host "Running Starting Api" -Foreground DarkYellow
Write-Host "Running Acceptance Tests" -Foreground DarkYellow
$job = Start-Job -ScriptBlock {dotnet run --project $using:PWD/Api/Api.fsproj}
Start-Sleep -s 3 #yeah, I know :)
dotnet run --project ./AcceptanceTests/AcceptanceTests.fsproj
Write-Host "Running Stopping Api" -Foreground DarkYellow
stop-job $job
Write-Host "Running Done" -Foreground DarkYellow
