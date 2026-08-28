<#
    Deploys Division of Labor from this repo (the master) to the RimWorld Mods
    folder, which holds only what ships to the Steam Workshop.

        powershell -ExecutionPolicy Bypass -File .\deploy.ps1
        powershell -ExecutionPolicy Bypass -File .\deploy.ps1 -WhatIf

    Ships an ALLOWLIST, not an exclusion list, so anything added to the repo
    later - Branding, README, LICENSE, .git, Source - stays out of the Workshop
    download unless it is named in $Ship below.
#>
param(
    [string]$Target = "C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\Division of Labor",
    [switch]$WhatIf
)

$ErrorActionPreference = "Stop"
$Repo = $PSScriptRoot
$Ship = @("1.6", "About")

foreach ($item in $Ship) {
    if (-not (Test-Path (Join-Path $Repo $item))) { throw "Repo is missing '$item' - wrong folder?" }
}

if ($WhatIf) {
    Write-Host "Would deploy to: $Target" -ForegroundColor Cyan
    Write-Host "Would ship     : $($Ship -join ', ')"
    $extra = Get-ChildItem $Target -ErrorAction SilentlyContinue | Where-Object { $_.Name -notin $Ship }
    if ($extra) { Write-Host "Would remove   : $(($extra | Select-Object -ExpandProperty Name) -join ', ')" -ForegroundColor Yellow }
    return
}

if (-not (Test-Path $Target)) { New-Item -ItemType Directory -Path $Target -Force | Out-Null }

foreach ($item in $Ship) {
    robocopy (Join-Path $Repo $item) (Join-Path $Target $item) /MIR /NFL /NDL /NJH /NJS /NP | Out-Null
    if ($LASTEXITCODE -ge 8) { throw "robocopy failed on '$item' (exit $LASTEXITCODE)" }
}

# strip anything the Workshop should not carry
foreach ($stray in (Get-ChildItem $Target | Where-Object { $_.Name -notin $Ship })) {
    Write-Host "removing non-shipping item: $($stray.Name)" -ForegroundColor Yellow
    Remove-Item $stray.FullName -Recurse -Force
}

# verify every shipped file matches the repo
$bad = 0; $n = 0
foreach ($item in $Ship) {
    Get-ChildItem (Join-Path $Repo $item) -Recurse -File | ForEach-Object {
        $n++
        $rel = $_.FullName.Substring($Repo.Length + 1)
        $tp  = Join-Path $Target $rel
        if (-not (Test-Path $tp)) { $bad++; Write-Host "MISSING: $rel" -ForegroundColor Red }
        elseif ((Get-FileHash $_.FullName).Hash -ne (Get-FileHash $tp).Hash) { $bad++; Write-Host "DIFFERS: $rel" -ForegroundColor Red }
    }
}
$size = "{0:N2}" -f ((Get-ChildItem $Target -Recurse -File | Measure-Object Length -Sum).Sum / 1MB)
if ($bad -eq 0) { Write-Host "deployed $n files, $size MB, all verified" -ForegroundColor Green }
else { throw "$bad file(s) failed verification" }
