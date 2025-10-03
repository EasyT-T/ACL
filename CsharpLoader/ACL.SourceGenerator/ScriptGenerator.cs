using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace ACL.SourceGenerator;

using System.Collections.Generic;
using System.Linq;
using System.Threading;

[Generator]
public class ScriptGenerator : IIncrementalGenerator
{
    private const string Namespace = "ACL.SourceGenerators";
    private const string ClassAttributeName = "ScriptClassAttribute";
    private const string FunctionAttributeName = "ScriptFunctionAttribute";

    private const string ClassAttributeSourceCode = $$"""
                                                      namespace {{Namespace}};
                                                      using System;
                                                      [AttributeUsage(AttributeTargets.Class)]
                                                      public sealed class {{ClassAttributeName}} : Attribute
                                                      {
                                                          public string TypeName { get; }
                                                          public {{ClassAttributeName}}(string typeName)
                                                          {
                                                              TypeName = typeName;
                                                          }
                                                      }
                                                      """;

    private const string FunctionAttributeSourceCode = $$"""
                                                         namespace {{Namespace}};
                                                         using System;
                                                         [AttributeUsage(AttributeTargets.Method)]
                                                         public sealed class {{FunctionAttributeName}} : Attribute
                                                         {
                                                             public string Declaration { get; }
                                                             public bool ReturnReference { get; }
                                                             
                                                             public {{FunctionAttributeName}}(string declaration, bool returnReference = false)
                                                             {
                                                                 Declaration = declaration;
                                                                 ReturnReference = returnReference;
                                                             }
                                                         }
                                                         """;

    private static readonly ImmutableDictionary<SpecialType, string> ParamTypeMap = new Dictionary<SpecialType, string>
    {
        [SpecialType.System_Boolean] = "byte",
        [SpecialType.System_Int16] = "ushort",
        [SpecialType.System_Int32] = "uint",
        [SpecialType.System_Int64] = "ulong",
    }.ToImmutableDictionary();

    private static readonly ImmutableDictionary<SpecialType, string> ParamToArgConvertMap = new Dictionary<SpecialType, string>
    {
        [SpecialType.System_Boolean] = "Convert.ToByte",
        [SpecialType.System_Int16] = "(ushort)",
        [SpecialType.System_Int32] = "(uint)",
        [SpecialType.System_Int64] = "(ulong)",
    }.ToImmutableDictionary();

    private static readonly ImmutableDictionary<string, string> ArgToParamConvertMap = new Dictionary<string, string>
    {
        ["Convert.ToByte"] = "Convert.ToBoolean",
    }.ToImmutableDictionary();

    private static readonly ImmutableDictionary<SpecialType, string> ReturnMap = new Dictionary<SpecialType, string>
    {
        [SpecialType.System_String] = "return new ManagedString(context.GetReturnPointer()).ToString();",
        [SpecialType.System_Int16] = "return (short)context.GetReturnUInt16();",
        [SpecialType.System_UInt16] = "return context.GetReturnUInt16();",
        [SpecialType.System_Int32] = "return (int)context.GetReturnUInt32();",
        [SpecialType.System_UInt32] = "return context.GetReturnUInt32();",
        [SpecialType.System_Int64] = "return (long)context.GetReturnUInt64();",
        [SpecialType.System_UInt64] = "return context.GetReturnUInt64();",
        [SpecialType.System_Single] = "return context.GetReturnSingle();",
        [SpecialType.System_Double] = "return context.GetReturnDouble();",
        [SpecialType.System_IntPtr] = "return context.GetReturnPointer();",
        [SpecialType.System_Boolean] = "return Convert.ToBoolean(context.GetReturnByte());",
    }.ToImmutableDictionary();

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        RegisterAttributeSources(context);
        var classProvider = CreateClassProvider(context);
        context.RegisterSourceOutput(classProvider, GenerateClassCode);
    }

    private static void RegisterAttributeSources(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx =>
        {
            ctx.AddSource("ScriptClassAttribute.g.cs", SourceText.From(ClassAttributeSourceCode, Encoding.UTF8));
            ctx.AddSource("ScriptFunctionAttribute.g.cs", SourceText.From(FunctionAttributeSourceCode, Encoding.UTF8));
        });
    }

    private static IncrementalValuesProvider<INamedTypeSymbol?> CreateClassProvider(
        IncrementalGeneratorInitializationContext context)
    {
        return context.SyntaxProvider
            .CreateSyntaxProvider(static (s, _) => s is ClassDeclarationSyntax, TransformClass)
            .Where(static x => x is not null);
    }

    private static INamedTypeSymbol? TransformClass(GeneratorSyntaxContext context, CancellationToken _)
    {
        if (context.Node is not ClassDeclarationSyntax classSyntax)
        {
            return null;
        }

        if (context.SemanticModel.GetDeclaredSymbol(classSyntax) is not INamedTypeSymbol classSymbol)
        {
            return null;
        }

        return HasAttribute(classSymbol, ClassAttributeName) ? classSymbol : null;
    }

    private static void GenerateClassCode(SourceProductionContext context, INamedTypeSymbol? classSymbol)
    {
        if (classSymbol == null)
        {
            return;
        }

        var methods = GetTargetMethods(classSymbol);
        if (methods.Count == 0)
        {
            return;
        }

        var sourceBuilder = new StringBuilder(4096);
        GenerateFileHeader(sourceBuilder, classSymbol);
        GenerateClassHeader(sourceBuilder, classSymbol);
        GenerateMethods(sourceBuilder, methods);
        sourceBuilder.AppendLine("}");

        context.AddSource($"{classSymbol.Name}.g.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
    }

    private static List<IMethodSymbol> GetTargetMethods(INamedTypeSymbol classSymbol)
    {
        return classSymbol.GetMembers()
            .OfType<IMethodSymbol>()
            .Where(static m => HasAttribute(m, FunctionAttributeName))
            .ToList();
    }

    private static void GenerateFileHeader(StringBuilder sb, INamedTypeSymbol classSymbol)
    {
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("using System;");
        sb.AppendLine("using ACL;");
        sb.AppendLine("using ACL.Private;");
        sb.AppendLine("using ACL.Feature;");
        sb.AppendLine("using ACL.Managed;");
        sb.AppendLine("using ACL.Managed.ScriptObject;");
        sb.AppendLine();
        sb.AppendLine($"namespace {classSymbol.ContainingNamespace.ToDisplayString()};");
        sb.AppendLine();
    }

    private static void GenerateClassHeader(StringBuilder sb, INamedTypeSymbol classSymbol)
    {
        var typeName = GetTypeName(classSymbol);
        sb.AppendLine($"partial class {classSymbol.Name}");
        sb.AppendLine("{");
        sb.AppendLine(
            $"    private static readonly IntPtr TypeInfo = ACL.Private.NativeBindings.TL_Engine_GetTypeInfoByDecl(\"{typeName}\");");
        sb.AppendLine();
    }

    private static string GetTypeName(INamedTypeSymbol classSymbol)
    {
        var attribute = classSymbol.GetAttributes()
            .First(static a => IsMatchingAttribute(a, ClassAttributeName));

        return attribute.ConstructorArguments[0].Value as string
               ?? string.Empty;
    }

    private static void GenerateMethods(StringBuilder sb, List<IMethodSymbol> methods)
    {
        foreach (var method in methods)
        {
            GenerateMethodField(sb, method);
            GenerateMethodImplementation(sb, method);
        }
    }

    private static void GenerateMethodField(StringBuilder sb, IMethodSymbol method)
    {
        var declaration = GetFunctionDeclaration(method);
        var fieldName = GetFieldName(method);
        sb.AppendLine($"    private static readonly ScriptFunction {fieldName} = ");
        sb.AppendLine(
            $"        new ACL.Managed.ScriptFunction(ACL.Private.NativeBindings.TL_TypeInfo_GetMethodByDecl(TypeInfo, \"{declaration.Replace("\"", "\\\"")}\", false));");
        sb.AppendLine();
    }

    private static string GetFunctionDeclaration(IMethodSymbol method)
    {
        var attribute = method.GetAttributes()
            .First(static a => IsMatchingAttribute(a, FunctionAttributeName));

        return attribute.ConstructorArguments[0].Value as string
               ?? string.Empty;
    }

    private static bool GetReturnReference(IMethodSymbol method)
    {
        var attribute = method.GetAttributes()
            .First(static a => IsMatchingAttribute(a, FunctionAttributeName));

        return attribute.ConstructorArguments[1].Value as bool? ?? false;
    }

    private static string GetFieldName(IMethodSymbol method)
    {
        return $"{char.ToLower(method.Name[0])}{method.Name.Substring(1)}Function_{string.Join("_", method.Parameters.Select(p => p.Type.Name))}";
    }

    private static void GenerateMethodImplementation(StringBuilder sb, IMethodSymbol method)
    {
        var methodSyntax = GetMethodDeclarationSyntax(method);
        sb.Append(GetMethodSignature(method, methodSyntax));
        sb.AppendLine();
        sb.AppendLine("    {");
        sb.AppendLine("        var context = ScriptEngine.CreateContext();");
        sb.AppendLine($"        context.Prepare({GetFieldName(method)});");
        sb.AppendLine("        context.SetObject(this);");
        GenerateParameterCode(sb, method);
        sb.AppendLine("        context.Execute();");
        GenerateReturnCode(sb, method);
        sb.AppendLine("    }");
        sb.AppendLine();
    }

    private static MethodDeclarationSyntax GetMethodDeclarationSyntax(IMethodSymbol method)
    {
        return method.DeclaringSyntaxReferences
            .Select(static r => r.GetSyntax())
            .OfType<MethodDeclarationSyntax>()
            .First()
            .WithAttributeLists(default);
    }

    private static void GenerateParameterCode(StringBuilder sb, IMethodSymbol method)
    {
        for (var i = 0; i < method.Parameters.Length; i++)
        {
            var parameter = method.Parameters[i];
            if (parameter.Type.SpecialType == SpecialType.System_String)
            {
                sb.AppendLine($"        using var str{i} = ManagedString.Create({parameter.Name});");
                sb.AppendLine($"        context.SetArgument({i}, str{i});");
            }
            else if (parameter.Type.SpecialType != SpecialType.None)
            {
                var convertType = ParamToArgConvertMap.TryGetValue(parameter.Type.SpecialType, out var temp1) ? temp1 : $"({parameter.Type.Name})";

                if (parameter.RefKind == RefKind.Out)
                {
                    var argType = ParamTypeMap.TryGetValue(parameter.Type.SpecialType, out var temp2) ? temp2 : parameter.Type.Name;
                    sb.AppendLine($"        context.SetArgument({i}, out {argType} {parameter.Name}_);");
                    var argToParamConvert = ArgToParamConvertMap.TryGetValue(argType, out var temp3) ? temp3 : $"({parameter.Type.Name})";
                    sb.AppendLine($"        {parameter.Name} = {argToParamConvert}({parameter.Name}_);");
                }
                else
                {
                    sb.AppendLine($"        context.SetArgument({i}, {convertType}({parameter.Name}));");
                }
            }
            else if (parameter is { RefKind: RefKind.Out, Type: INamedTypeSymbol nts }
                     && InheritsFromScriptObjectBase(nts))
            {
                sb.AppendLine(
                    $"        unsafe {{ context.SetArgument({i}, handle => new {nts.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}((AngelObject*)handle), out {parameter.Name}); }}");
            }
            else
            {
                sb.AppendLine($"        context.SetArgument({i}, {parameter.Name});");
            }
        }
    }

    private static void GenerateReturnCode(StringBuilder sb, IMethodSymbol method)
    {
        if (method.ReturnsVoid)
        {
            return;
        }

        sb.AppendLine(ReturnMap.TryGetValue(method.ReturnType.SpecialType, out var returnCode)
            ? $"        {returnCode}"
            : GenerateObjectReturnCode(method));
    }

    private static string GenerateObjectReturnCode(IMethodSymbol method)
    {
        return GetReturnReference(method)
            ? $"        unsafe {{ return new {method.ReturnType.ToDisplayString()}((AngelObject*)context.GetReturnObject()); }}"
            : $"        unsafe {{ return new {method.ReturnType.ToDisplayString()}((AngelObject*)context.GetReturnPointer()); }}";
    }

    private static bool HasAttribute(ISymbol symbol, string attributeName)
    {
        return symbol.GetAttributes()
            .Any(a => IsMatchingAttribute(a, attributeName));
    }

    private static bool IsMatchingAttribute(AttributeData attribute, string name)
    {
        return attribute.AttributeClass?.ToDisplayString() == $"{Namespace}.{name}";
    }

    public static string GetMethodSignature(IMethodSymbol methodSymbol, MethodDeclarationSyntax? methodSyntax = null)
    {
        var modifiers = methodSyntax != null
            ? string.Join(" ", methodSyntax.Modifiers.Select(m => m.Text))
            : string.Join(" ", methodSymbol.DeclaredAccessibility.ToString().ToLower(),
                methodSymbol.IsStatic ? "static" : "");

        var returnType = methodSymbol.ReturnType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        var name = methodSymbol.Name;

        var typeParams = methodSyntax?.TypeParameterList?.ToString() ?? "";

        var parameters = string.Join(", ",
            methodSymbol.Parameters.Select(p =>
            {
                var refKind = p.RefKind != RefKind.None ? p.RefKind.ToString().ToLower() + " " : "";
                var type = p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                return $"{refKind}{type} {p.Name}";
            }));

        var constraints = methodSyntax != null
            ? string.Join(" ", methodSyntax.ConstraintClauses.Select(c => c.ToString()))
            : "";

        return $"{modifiers} {returnType} {name}{typeParams}({parameters}) {constraints}".Trim();
    }

    private static bool InheritsFromScriptObjectBase(INamedTypeSymbol? type)
    {
        while (type != null)
        {
            if (type.Name == "ScriptObjectBase")
            {
                return true;
            }

            type = type.BaseType;
        }

        return false;
    }
}