echo "Getting library files..."
echo "  Dir: $PWD"

bash install-package-from-libs-repository.sh GrowSense M2Mqtt 4.3.0.0 || exit 1

echo "Finished getting library files."
