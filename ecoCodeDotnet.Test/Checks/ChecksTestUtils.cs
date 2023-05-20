using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ecoCodeDotnet.Test.Checks;

public static class ChecksTestUtils
{
    const string Path = "../../../TestData";

    public static string[] ReadCodes(params string[] sources)
    {
        return sources.Select(file => File.ReadAllText($"{Path}/{file}", Encoding.UTF8)).ToArray();
    }
}

