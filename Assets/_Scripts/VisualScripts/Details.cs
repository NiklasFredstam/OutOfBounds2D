using Microsoft.Unity.VisualStudio.Editor;
using NUnit.Framework;
using System.Collections.Generic;

public class Details
{
    List<DetailsItem> detailsItems = new();
}

public class DetailsItem
{
    Image detailsImage;
    string value;
    string text;
}