// Code analysis warnings we chose not to fix, each with the reason why.
// Every warning the analyzers reported, fixed or not, is listed in CodeAnalysis.md at the repo root.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores",
    Justification = "A C# name can't start with a digit, so the underscores keep '3902' readable in the project's namespace. " +
                    "Renaming it would touch every file that uses Game1 without changing any behaviour.",
    Scope = "namespace", Target = "~N:CSE_3902_Project")]

[assembly: SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance",
    Justification = "Game1 deliberately holds Link as IPlayer so that it, and the commands it creates, only depend on the " +
                    "player interface. The speed difference of one call per frame is not noticeable in this game.",
    Scope = "member", Target = "~F:CSE_3902_Project.Game1.link")]
