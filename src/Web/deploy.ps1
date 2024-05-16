Import-Module WebAdministration

$connectionString = $env:connection_string
Write-Host "Connection string length: $($connectionString.length)"
[Environment]::SetEnvironmentVariable("ConnectionStrings:Default", $connectionString)

Write-Host "---"
Write-Host "---"

$appPoolName = $env:APPLICATION_SITE_NAME

# Make sure the app pool exists
if (Test-Path IIS:\AppPools\$appPoolName) {
    # Get its current state
    $appPoolState = (Get-WebAppPoolState -Name $appPoolName).Value

    if ($appPoolState -eq "Stopped") {
        # Start the app pool
        Write-Host "Starting application pool `"$appPoolName`"..."
        Start-WebAppPool -Name $appPoolName
        Write-Host "Application pool `"$appPoolName`" has been started."

        # Start the website
        Write-Host "Starting website `"$appPoolName`"..."
        Start-Website -Name $appPoolName
        Write-Host "Website `"$appPoolName`" has been started."
    }
    else {
       Write-Host "Application pool `"$appPoolName`" already started (current state: `"$appPoolState`"). Done."
    }
}
else {
    Write-Host "Application pool `"$appPoolName`" not found... skipping."
}