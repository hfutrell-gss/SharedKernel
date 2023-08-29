# Ask user if they want to deploy as a release
$deployAsRelease = Read-Host "Are you publishing a release? (major/minor/patch/[no])"

if ($deployAsRelease -eq "") {
    $deployAsRelease = "no"
}

if ($deployAsRelease -ne 'major' -And $deployAsRelease -ne 'minor' -And $deployAsRelease -ne 'patch'-And $deployAsRelease -ne 'no') {
        Write-Host "Invalid selection. Must be major/minor/patch/no"
        exit
    }

# Get all .csproj files from directory recursively
$projectFiles = Get-ChildItem -Recurse -Filter *.csproj

# Filter out test projects
$deployableProjects = $projectFiles | Where-Object {$_.Name -notmatch 'Test'}

# Show a list of deployable projects and let the user select which ones to build
Write-Host "    Found $($deployableProjects.Count) deployable .NET projects:"
for ($i=0; $i -lt $deployableProjects.Count; $i++) {
    Write-Host "        [$($i+1)]: $($deployableProjects[$i].Name)"
}

$selectedProjects = @()
$projectsToDeploy = Read-Host "Enter the numbers of the projects you want to publish separated by spaces (e.g. '1 5'), or 'all' to deploy all. (Default: none)"

if ($projectsToDeploy -eq 'all') {
    $selectedProjects = $deployableProjects
} else {
    $projectsToDeploy.Split(' ') | ForEach-Object {
        if ($_ -match '^\d+$' -and [int]$_ -le $deployableProjects.Count) {
            $selectedProjects += $deployableProjects[$_-1]
        }
        else {
            Write-Host "    $_ was invalid input"
        }
    }
}

# Define local NuGet server and API Key
$nugetServerUrl = "\\gss2k19cicd00\GSS-NuGet-Feed"

# Current timestamp for beta releases
$timestamp = Get-Date -Format "yyyyMMddHHmmss"

foreach ($project in $selectedProjects) {
    Write-Host "Starting project $project"
    # Navigate to project directory
    Set-Location $project.DirectoryName
    $packageId = $xml.Project.PropertyGroup.PackageId

    if ($packageId -eq $null) {
        $packageId = $project.BaseName
    }

    # Get the package version from the .csproj file
    $xml = [xml](Get-Content $project.FullName)
    $currentVersion = $xml.Project.PropertyGroup.Version
    Write-Host "Current version is $currentVersion"
    Write-Host "Using $packageId as package id"

    # Get versions from server
    Write-Host "Getting latest version from server (may take a moment)..."

    $packageDirectory = Join-Path -Path $nugetServerUrl -ChildPath $packageId

    # Get subfolders representing versions
    $versionFolders = Get-ChildItem -Path $packageDirectory -Directory

    # Get the versions from the folder names
    $versions = $versionFolders | ForEach-Object { $_.Name }

    # Get the latest version number (ignoring suffixes)
    $latestVersion = $versions | ForEach-Object { $_ -split '-', 2 | Select-Object -First 1 } | Sort-Object {[Version]$_} | Select-Object -Last 1

    # If the latest version is $null, default to '1.0.0'
    if ($latestVersion -eq $null) {
        Write-Host "No versions found on the server. Defaulting to 1.0.0"
        $latestVersion = '1.0.0'
    }

    Write-Host "$latestVersion is latest"
    
    if ($deployAsRelease -eq 'patch') {
        # Increment the minor version
        $versionParts = $latestVersion.Split('.')
        $versionParts[2] = ([int]$versionParts[2] + 1).ToString()
        $publishVersion = $versionParts -join '.'
    }elseif ($deployAsRelease -eq 'minor') {
         # Increment the minor version
        $versionParts = $latestVersion.Split('.')
        $versionParts[2] = (0).ToString()
        $versionParts[1] = ([int]$versionParts[1] + 1).ToString()
        $publishVersion = $versionParts -join '.'
    }elseif ($deployAsRelease -eq 'major'){
        # Increment the minor version
        $versionParts = $latestVersion.Split('.')
        $versionParts[2] = (0).ToString()
        $versionParts[1] = (0).ToString()
        $versionParts[0] = ([int]$versionParts[0] + 1).ToString()
        $publishVersion = $versionParts -join '.'
    }
    
    # If user does not want to deploy as a release, prepend "beta" to the latest version
    if ($deployAsRelease -eq 'no') {
        Write-Host "Not publishing a release version. Will append beta with timestamp to latest version number"
        Write-Host ""
        $publishVersion = $latestVersion + "-$timeStamp-beta"
    }

    $content = Get-Content -Path $project.FullName -Raw

    # Check if the Version tag exists
    if ($content -match '<Version>(.*?)<\/Version>') {
        # Replace the version
        $content = $content -replace '<Version>(.*?)<\/Version>', "<Version>$newVersion</Version>"
    } else {
        # If the Version tag doesn't exist, add it (you can modify where to insert it if needed)
        $content = $content -replace '<PropertyGroup>', "<PropertyGroup>`r`n<Version>$newVersion</Version>"
    }

    # Write the updated content back to the .csproj file
    Set-Content -Path $project.FullName -Value $content

    Write-Output "Version set to $newVersion" 

    # Build the project in release configuration
    Write-Host "Building the project..."
    dotnet build --configuration Release
    Write-Host ""

    # Pack the project
    Write-Host "Packing the project..."
    dotnet pack --configuration Release
    Write-Host ""

    # Push the package to the local NuGet server
    Write-Host ""
    Write-Host "Publishing as $($project.BaseName).$publishVersion"
    dotnet nuget push "$($project.DirectoryName)\bin\Release\$($project.BaseName).$publishVersion.nupkg" --source $nugetServerUrl
}