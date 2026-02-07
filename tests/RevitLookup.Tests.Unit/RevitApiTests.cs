// Copyright (c) Lookup Foundation and Contributors
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// THIS PROGRAM IS PROVIDED "AS IS" AND WITH ALL FAULTS.
// NO IMPLIED WARRANTY OF MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE IS PROVIDED.
// THERE IS NO GUARANTEE THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.

using Nice3point.TUnit.Revit;
using Nice3point.TUnit.Revit.Executors;
using TUnit.Core.Executors;

namespace RevitLookup.Tests.Unit;

public sealed class RevitApiTests : RevitApiTest
{
    private static string SamplesPath => $@"C:\Program Files\Autodesk\Revit {Application.VersionNumber}\Samples";

    [Before(Class)]
    public static void ValidateSamples()
    {
        if (!Directory.Exists(SamplesPath))
        {
            Skip.Test($"Samples folder not found at {SamplesPath}");
            return;
        }

        if (!Directory.EnumerateFiles(SamplesPath, "*.rfa").Any())
        {
            Skip.Test($"No .rfa files found in {SamplesPath}");
        }
    }

    public static IEnumerable<string> GetSampleFile()
    {
        return Directory.Exists(SamplesPath) ? Directory.EnumerateFiles(SamplesPath, "*.rfa") : [];
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    [MethodDataSource(nameof(GetSampleFile))]
    public async Task Parameters_Builtin_ShouldCreateAllCategories(string filePath)
    {
        Document? document = null;

        try
        {
            document = Application.OpenDocumentFile(filePath);

            var builtInParameters = Enum.GetValues<BuiltInParameter>();
            foreach (var builtInParameter in builtInParameters)
            {
                var parameter = builtInParameter.ToParameter(document);

                await Assert.That(parameter).IsNotNull();
            }
        }
        finally
        {
            document?.Close(false);
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    [MethodDataSource(nameof(GetSampleFile))]
    public async Task Categories_Builtin_ShouldCreateAllCategories(string filePath)
    {
        Document? document = null;

        try
        {
            document = Application.OpenDocumentFile(filePath);

            var builtInCategories = Enum.GetValues<BuiltInCategory>();
            foreach (var builtInCategory in builtInCategories)
            {
                var category = builtInCategory.ToCategory(document);

                await Assert.That(category).IsNotNull();
            }
        }
        finally
        {
            document?.Close(false);
        }
    }
}