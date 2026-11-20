# Working on Bitvavo.Net

Use this guide for library development. For code that consumes the library, see the [Bitvavo.Net skill](.claude/skills/Bitvavo-net/SKILL.md). [llms.txt](llms.txt) indexes the rest of the documentation.

## Repository layout

- [Client interfaces](Bitvavo.Net/Interfaces/Clients/) contain public endpoint signatures and XML documentation.
- [Clients](Bitvavo.Net/Clients/) contain the native and Shared API implementations. The Exchange API separates REST and WebSocket, with `ExchangeData`, `Account`, and `Trading` topics.
- [Models](Bitvavo.Net/Objects/Models/), [options](Bitvavo.Net/Objects/Options/), and [enums](Bitvavo.Net/Enums/) define wire models and configuration.
- [Unit tests](Bitvavo.Net.UnitTests/) cover request and response fixtures, socket queries and subscriptions, Shared API capabilities, and example compilation.
- [AI-friendly examples](Examples/ai-friendly/) are standalone consumer programs.

## Build and validation

Run from the repository root with the SDK configured in the [build workflow](.github/workflows/dotnet.yml). See the [library project](Bitvavo.Net/Bitvavo.Net.csproj) for target frameworks and dependencies.

```sh
dotnet restore Bitvavo.Net.sln
dotnet build Bitvavo.Net.sln --no-restore
dotnet test Bitvavo.Net.UnitTests/Bitvavo.Net.UnitTests.csproj --no-build --filter "FullyQualifiedName!~IntegrationTests"
```

Debug builds use a subset of the target frameworks. To check all targets:

```sh
dotnet build Bitvavo.Net/Bitvavo.Net.csproj -p:BuildAllFrameworks=true
```

To compile the consumer examples independently:

```sh
dotnet test Bitvavo.Net.UnitTests/Bitvavo.Net.UnitTests.csproj --filter FullyQualifiedName~AiExampleCompileTests
```

The example test builds the programs. The live REST and WebSocket integration fixtures are disabled by default.

## Implementation conventions

- Preserve CRLF line endings in edited files.
- Start from the matching interface and a neighboring implementation. Update the interface, XML documentation, model and source-generated JSON metadata when they change together.
- Use `Symbol` for a trading market and `Quantity` for amount or volume. Use existing enum conventions for fixed sets of values and retain string or numeric types for open-ended values.
- Where applicable, order endpoint parameters as symbol, ID filters, start and end times, page and page size or limit, then the cancellation token.
- Keep the configured `OperatorId` as the default for trading requests that require one. Use the optional endpoint override when supplied.
- Preserve signing, request serialization, and rate limits. Use the exchange defaults for parameter serialization, with field overrides for exceptions.
- Match Shared API capabilities to the actual transport and spot behavior. Use the existing fixture tests for changes to requests, response mappings, and subscription routing.

## AI documentation ownership

- [SKILL.md](.claude/skills/Bitvavo-net/SKILL.md) covers consumer conventions and links to task-specific references.
- [API surfaces](.claude/skills/Bitvavo-net/references/api-surfaces.md) maps tasks to methods and capabilities. [Usage](.claude/skills/Bitvavo-net/references/usage.md) contains snippets, and [operational considerations](.claude/skills/Bitvavo-net/references/safety.md) covers credentials, live requests, and uncertain outcomes.
- Keep skill links relative. Source and example links are for repository checkouts. The skill references are included under `skills/` in the NuGet package.
