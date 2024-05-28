using System.Linq;
using Xunit;

namespace Wabbajack.Paths.Test;

public class AbsolutePathTests
{
    [Fact]
    public void CanParsePaths()
    {
        Assert.Equal(((AbsolutePath) @"c:\foo\bar").ToString(), ((AbsolutePath) @"c:\foo\bar").ToString());
    }

    [Fact]
    public void CanGetParentPath()
    {
        Assert.Equal(((AbsolutePath) @"c:\foo").ToString(), ((AbsolutePath) @"c:\foo\bar").Parent.ToString());
    }

    [Fact]
    public void ParentOfTopLevelPathThrows()
    {
        Assert.Throws<PathException>(() => ((AbsolutePath) @"c:\").Parent.ToString());
    }

    [Fact]
    public void CanCreateRelativePathsFromAbolutePaths()
    {
        Assert.Equal((RelativePath) @"baz\qux.zip",
            ((AbsolutePath) @"\\foo\bar\baz\qux.zip").RelativeTo((AbsolutePath) @"\\foo\bar"));
        Assert.Throws<PathException>(() =>
            ((AbsolutePath) @"\\foo\bar\baz\qux.zip").RelativeTo((AbsolutePath) @"\\z\bar"));
        Assert.Throws<PathException>(() =>
            ((AbsolutePath) @"\\foo\bar\baz\qux.zip").RelativeTo((AbsolutePath) @"\\z\bar\buz"));
    }

    [Fact]
    public void PathsAreEquatable()
    {
        Assert.Equal((AbsolutePath) @"c:\foo", (AbsolutePath) @"c:\foo");

        Assert.True((AbsolutePath) @"c:\foo" == (AbsolutePath) @"c:\Foo");
        Assert.False((AbsolutePath) @"c:\foo" != (AbsolutePath) @"c:\Foo");
        Assert.NotEqual((AbsolutePath) @"c:\foo", (AbsolutePath) @"c:\bar");
        Assert.NotEqual((AbsolutePath) @"c:\foo\bar", (AbsolutePath) @"c:\foo");
    }

    [Fact]
    public void CanGetPathHashCodes()
    {
        Assert.Equal(@"c:\foo\bar.baz".ToAbsolutePath().GetHashCode(),
            @"C:\Foo\Bar.bAz".ToAbsolutePath().GetHashCode());
    }


    [Fact]
    public void CaseInsensitiveEquality()
    {
        Assert.Equal(@"c:\foo\bar.baz".ToAbsolutePath(), @"C:\Foo\Bar.bAz".ToAbsolutePath());
        Assert.NotEqual(@"c:\foo\bar.baz".ToAbsolutePath(), (object) 42);
    }

    [Fact]
    public void CanReplaceExtensions()
    {
        Assert.Equal(new Extension(".dds"), ((AbsolutePath) @"/foo/bar.dds").Extension);
        Assert.Equal((RelativePath) "bar.dds", ((AbsolutePath) @"/foo/bar.dds").FileName);
        Assert.Equal((AbsolutePath) @"/foo/bar.zip",
            ((AbsolutePath) @"/foo/bar.dds").ReplaceExtension(new Extension(".zip")));
        Assert.Equal((AbsolutePath) @"/foo\bar.zip",
            ((AbsolutePath) @"/foo\bar").ReplaceExtension(new Extension(".zip")));
    }

    [Fact]
    public void CanGetPathFormats()
    {
        Assert.Equal(PathFormat.Windows, ((AbsolutePath) @"c:\foo\bar").PathFormat);
        Assert.Equal(PathFormat.Windows, ((AbsolutePath) @"\\foo\bar").PathFormat);
        Assert.Equal(PathFormat.Unix, ((AbsolutePath) @"/foo/bar").PathFormat);
        Assert.Throws<PathException>(() => ((AbsolutePath) @"c!\foo/bar").PathFormat);
    }

    [Fact]
    public void CanCombinePaths()
    {
        Assert.Equal("/foo/bar/baz/qux",
            ((AbsolutePath) "/").Combine("foo", (RelativePath) "bar", "baz/qux").ToString());
        Assert.Throws<PathException>(() => ((AbsolutePath) "/").Combine(42));
    }

    [Fact]
    public void CanConvertPathsToStrings()
    {
        Assert.Equal("/foo/bar", ((AbsolutePath) "/foo/bar").ToString());
    }

    [Fact]
    public void PathsAreComparable()
    {
        var data = new[]
        {
            (AbsolutePath) @"c:\a",
            (AbsolutePath) @"c:\b\c",
            (AbsolutePath) @"c:\d\e\f",
            (AbsolutePath) @"c:\b"
        };
        var data2 = data.OrderBy(a => a).ToArray();

        var data3 = new[]
        {
            (AbsolutePath) @"c:\a",
            (AbsolutePath) @"c:\b",
            (AbsolutePath) @"c:\b\c",
            (AbsolutePath) @"c:\d\e\f"
        };
        Assert.Equal(data3, data2);
    }

    [Fact]
    public void CanGetThisAndAllParents()
    {
        var path = @"c:\foo\bar\baz.zip".ToAbsolutePath();
        var subPaths = new[]
        {
            @"c:\",
            @"C:\foo",
            @"c:\foo\Bar",
            @"c:\foo\bar\baz.zip"
        }.Select(f => f.ToAbsolutePath());
        
        Assert.Equal(subPaths.OrderBy(f => f), path.ThisAndAllParents().OrderBy(f => f).ToArray());
    }
}