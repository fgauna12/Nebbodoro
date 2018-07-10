
param(
    # The name of the exported SSL certificate
    [string]
    [Parameter(Mandatory = $true)]
    $CertificatePath,
    # The password for the expoerted SSL certificate
    [string]
    [Parameter(Mandatory = $true)]
    $CertificatePassword,
    # The name of the Azure Key Vault Instance
    [string]
    [Parameter(Mandatory = $true)]
    $KeyVaultName,
    # The name of the Azure Key Vault Secret Name
    [string]
    $KeyVaultSecretName = 'wildcard-certificate'
)

Write-Host "Importing Certificate " + $CertificatePath
$flag = [System.Security.Cryptography.X509Certificates.X509KeyStorageFlags]::Exportable 
$collection = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2Collection 
$collection.Import($CertificatePath, $CertificatePassword, $flag)
$pkcs12ContentType = [System.Security.Cryptography.X509Certificates.X509ContentType]::Pkcs12 
$clearBytes = $collection.Export($pkcs12ContentType) 
$fileContentEncoded = [System.Convert]::ToBase64String($clearBytes) 
Write-Host $fileContentEncoded
$secret = ConvertTo-SecureString -String $fileContentEncoded -AsPlainText -Verbose -Debug -Force -ErrorAction Stop
$secretContentType = 'application/x-pkcs12' 
Set-AzureKeyVaultSecret -VaultName $KeyVaultName -Name $KeyVaultSecretName -SecretValue $Secret -ContentType $secretContentType # Change Key Vault name and Secret name 