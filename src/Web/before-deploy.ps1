Import-Module WebAdministration

Write-Host "---"
Write-Host "---"
Write-Host "Stopping application pool and website before deploy."

$appPoolName = $env:APPLICATION_SITE_NAME

# Make sure the app pool exists
if (Test-Path IIS:\AppPools\$appPoolName) {
    # Get its current state
    $appPoolState = (Get-WebAppPoolState -Name $appPoolName).Value

    if ($appPoolState -ne "Stopped") {
        # Stop the website
        Write-Host "Stopping website `"$appPoolName`"..."
        Stop-Website -Name $appPoolName
        Write-Host "Website `"$appPoolName`" has been stopped."

        # Stop the app pool
        Write-Host "Stopping application pool `"$appPoolName`"..."
        Stop-WebAppPool -Name $appPoolName
        Write-Host "Application pool `"$appPoolName`" has been stopped."

        # Wait for 10 seconds... sometimes, the app pool takes a while to really stop and release all DLLs
        Start-Sleep -s 10
    }
    else {
        Write-Host "Application pool `"$appPoolName`" already stopped (current state: `"$appPoolState`"). Done."
    }
}
else {
    Write-Host "Application pool `"$appPoolName`" not found... skipping."
}
