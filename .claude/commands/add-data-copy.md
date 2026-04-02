Add an `ItemGroup` to `src/MicroByt.IA/MicroByt.IA.csproj` so that every file under `Data\**\*` is copied to the output directory on build and to the publish directory on publish.

Steps:
1. Read `src/MicroByt.IA/MicroByt.IA.csproj`.
2. If an `<Content Include="Data\**\*">` entry already exists, report that it is already configured and stop.
3. Otherwise, add the following `ItemGroup` before the first existing `<ItemGroup>` (or at the end of the `<Project>` block if none exists):

```xml
<ItemGroup>
  <Content Include="Data\**\*">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory>
  </Content>
</ItemGroup>
```

4. Run `dotnet build src/MicroByt.IA.slnx` and confirm the build succeeds with 0 errors.
