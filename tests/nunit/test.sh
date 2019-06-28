CATEGORY=$1

if [ -z "$CATEGORY" ]; then
    CATEGORY="Unit"
fi

echo "Testing project"
echo "  Dir: $PWD"
echo "  Category: $CATEGORY"

mono lib/NUnit.Runners/tools/nunit-console.exe bin/Release/*.dll --result=TestResult.xml --include=$CATEGORY
