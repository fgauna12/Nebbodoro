param(
    # Resource Group Name
    [Parameter(Mandatory = $true)]
    [string]
    $ResourceGroupName,
    # The Hostname
    [Parameter(Mandatory = $true)]
    [string]
    $HostName,
    # The Subdomain
    [Parameter(Mandatory = $true)]
    [string]
    $SubDomain,
    # Certificate Thumbprint
    [Parameter(Mandatory = $true)]
    [string]
    $CertificateThumbprint
)

$webApps = Get-AzureRmWebApp -ResourceGroupName $ResourceGroupName | Sort-Object Name
$certificates = Get-AzureRmWebAppCertificate -ResourceGroupName $ResourceGroupName
$vanityUrl = "$($SubDomain).$($HostName)"

$urlMap = @{}

function ShowCNameInstruction {
    param (
        $name,
        $value
    )

    $info = @{
        Host = $name;
        PointsTo = $value
    }

    Write-Host "$($HostName) - Please configure a CNAME record for your $($HostName) domain"

    $info | Format-Table | Out-String| % {Write-Host $_}

    Read-Host "Press [Enter] key when ready ..."
}

foreach ($item in $webApps) {
    Write-Host "We will configure the app service '$($item.DefaultHostName)' located in $($item.Location)"
    Read-Host "Press [Enter] key when ready ..."

    $ip4Address = Resolve-DnsName -Name $item.DefaultHostName | select IP4Address | where {$_.IP4Address -ne $null}
    $postFix = $item.Location.ToLower().Trim().Replace(" ", "");

    $fullSubDomain = "$($SubDomain)-$($postFix)"
    $appServiceVanityUrl = "$($fullSubDomain).$($HostName)"

    ShowCNameInstruction -name $fullSubDomain -value $item.DefaultHostName
    ShowCNameInstruction -name $SubDomain -value $item.DefaultHostName

    Write-Host "Adding custom domain $($vanityUrl) and $($fullSubDomain) to $($item.Name)"

    # Add the custom domain
    Set-AzureRmWebApp -Name $item.Name -ResourceGroupName $ResourceGroupName -HostNames @($appServiceVanityUrl, $vanityUrl, $item.DefaultHostName) -ErrorAction Stop

    Write-Host "Adding SSL binding for https://$($appServiceVanityUrl)"

    # Add the SSL binding for that custom domain
    New-AzureRmWebAppSSLBinding -ResourceGroupName $ResourceGroupName -WebAppName $item.Name -Thumbprint $CertificateThumbprint -Name $appServiceVanityUrl -ErrorAction Stop

    Write-Host "Now open https://$($appServiceVanityUrl) and verify that the domain name resolves"
    Read-Host "Press [Enter] key when ready ..."

    Write-Host "Adding SSL binding for https://$($vanityUrl)"
    New-AzureRmWebAppSSLBinding -ResourceGroupName $ResourceGroupName -WebAppName $item.Name -Thumbprint $CertificateThumbprint -Name $vanityUrl -ErrorAction Stop

    Write-Host "Now open https://$($vanityUrl) and verify that the domain name resolves"
    Read-Host "Press [Enter] key when ready ..."

    $urlMap.Add($item.Name, $appServiceVanityUrl)
}

Write-Host "Now Configuring Traffic Manager"

# there should be 1 profile
$profile = Get-AzureRmTrafficManagerProfile -ResourceGroupName $ResourceGroupName

if ($profile.Count -gt 1)
{
    throw "There are more than 1 Traffic Manager profile in this resource group. Will not add the endpoints to the app services"
}

foreach ($item in $webApps) {
    Write-Host "Setting Traffic Manager endpoints to use the app services"
    $appServiceVanityUrl = $urlMap[$item.Name]
    $endpointName = "endpoints-$($appServiceVanityUrl)"
    Add-AzureRmTrafficManagerEndpointConfig -EndpointName $endpointName -TrafficManagerProfile $profile -Type ExternalEndpoints -Target $appServiceVanityUrl -EndpointLocation $item.Location -EndpointStatus Enabled
}

# Save the traffic manager profile
Set-AzureRmTrafficManagerProfile -TrafficManagerProfile $profile -ErrorAction Stop

$trafficManagerUrl = "$($profile.RelativeDnsName).trafficmanager.net"
ShowCNameInstruction -name $SubDomain -value $trafficManagerUrl

Write-Host "Configuration Complete!"
Write-Host "Run nslookup against $($vanityUrl) to verify that it resolves to the primary app service"