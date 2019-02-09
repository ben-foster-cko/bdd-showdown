dotnet build ./test/BddShowdown.Tests.NSpec -c Release

~/.dotnet/tools/coverlet ./test/BddShowdown.Tests.NSpec/bin/Release/netcoreapp2.0/BddShowdown.Tests.NSpec.dll \
    --target "dotnet" \
    --targetargs "run --project ./test/BddShowdown.Tests.NSpec/ -c Release --no-build" \
    --format lcov \
    --output ./coverage/lcov.info
