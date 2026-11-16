# API'nin OpenAPI belgesini indirir; panel bir sonraki derlemede istemciyi bundan yeniden üretir.
# Önce API çalışıyor olmalı:  dotnet run --project EmirCoaching.API --launch-profile https
# Sonra:                       .\EmirCoaching.Admin\OpenApi\Update-OpenApi.ps1
param([string]$ApiUrl = "https://localhost:7267")

$target = Join-Path $PSScriptRoot "emircoaching-api.json"
Invoke-WebRequest "$ApiUrl/openapi/v1.json" -OutFile $target -UseBasicParsing
Write-Host "Belge güncellendi: $target"
