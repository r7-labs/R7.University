#!/usr/bin/pwsh

param ([string]$version)

$versionStripped = $version -replace "-.*$"

cd ".."
$solutionFile = Get-ChildItem | Where-Object {$_.Name -match "\.sln$"}
$solutionContent = Get-Content $solutionFile
$solutionContent = $solutionContent -replace "version = \d+.\d+.\d+", ("version = " + $version)
$solutionContent | Set-Content $solutionFile.FullName

cd "R7.University/Properties"
$assemlyInfoFile = Get-ChildItem | Where-Object {$_.Name -match "^SolutionInfo.cs$"}
$assemlyInfoContent = Get-Content $assemlyInfoFile
$assemlyInfoContent = $assemlyInfoContent -replace "\[assembly: AssemblyVersion[^\]]+\]", ("[assembly: AssemblyVersion (""" + $versionStripped + """)]")
$assemlyInfoContent = $assemlyInfoContent -replace "\[assembly: AssemblyInformationalVersion[^\]]+\]", ("[assembly: AssemblyInformationalVersion (""" + $version + """)]")
$assemlyInfoContent | Set-Content $assemlyInfoFile.FullName

cd "../.."

$projectFiles = Get-ChildItem -Recurse | Where-Object {$_.Name -match "Deploy\.csproj|R7\.University(.*)\.csproj$"}
$projectFiles | ForEach-Object -Process {
    $projectFile = $_
    $projectFileContent = Get-Content $projectFile
    $projectFileContent = $projectFileContent -replace "<ReleaseVersion>[^<]+</ReleaseVersion>", ("<ReleaseVersion>" + $version + "</ReleaseVersion>")
    $projectFileContent | Set-Content $projectFile.FullName
}

cd "Deploy"
