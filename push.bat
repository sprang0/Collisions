dotnet pack --configuration Release
dotnet nuget push "bin\Release\CollisionLib.1.0.0.*.nupkg" --source "github"
