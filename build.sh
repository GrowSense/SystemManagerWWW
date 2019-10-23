echo "Starting building System Manager WWW project"
echo "  Dir: $PWD"

DIR=$PWD

xbuild /p:Configuration=Release /p:TargetFrameworkVersion=v4.5 src/GrowSense.SystemManager.WWW.sln /verbosity:quiet || exit 1

echo "Finished building System Manager WWW project."