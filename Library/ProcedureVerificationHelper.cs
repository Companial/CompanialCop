using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.InternalSyntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.SymbolReference;


#nullable enable
namespace CompanialCopAnalyzer.Design.Helper
{
    internal static class ProcedureVerificationHelper
    {
        internal static void MatchFunctions(
          IEnumerable<IMethodSymbol> newMethods,
          IEnumerable<IMethodSymbol> oldMethods,
          PooledList<IMethodSymbol> unmatchedNewMethods,
          PooledList<IMethodSymbol>? unmatchedOldMethods = null,
          PooledList<(IMethodSymbol, IMethodSymbol)>? matchedMethodPairs = null)
        {
            PooledDictionary<int, IMethodSymbol> instance1 = PooledDictionary<int, IMethodSymbol>.GetInstance();
            PooledDictionary<int, IMethodSymbol> instance2 = PooledDictionary<int, IMethodSymbol>.GetInstance();
            try
            {
                IEnumerable<IMethodSymbol> oldMethods1 = oldMethods;
                ProcedureVerificationHelper.MatchFunctions(newMethods, oldMethods1, instance1, instance2, matchedMethodPairs);
                unmatchedNewMethods.AddRange((IEnumerable<IMethodSymbol>)instance1.Values);
                unmatchedOldMethods?.AddRange((IEnumerable<IMethodSymbol>)instance2.Values);
            }
            finally
            {
                instance1.Free();
                instance2.Free();
            }
        }

        private static void MatchFunctions(
          IEnumerable<IMethodSymbol> newMethods,
          IEnumerable<IMethodSymbol> oldMethods,
          PooledDictionary<int, IMethodSymbol> unmatchedNewMethods,
          PooledDictionary<int, IMethodSymbol> unmatchedOldMethods,
          PooledList<(IMethodSymbol, IMethodSymbol)>? matchedMethodPairs = null)
        {
            foreach (IMethodSymbol newMethod in newMethods)
            {
                int methodId = SymbolExtensions.CalculateMethodId(newMethod);
                ProcedureVerificationHelper.AddMethodToDictionary(unmatchedNewMethods, methodId, newMethod, "current");
            }
            foreach (IMethodSymbol oldMethod in oldMethods)
            {
                int methodId = SymbolExtensions.CalculateMethodId(oldMethod);
                IMethodSymbol methodSymbol;
                if (unmatchedNewMethods.TryGetValue(methodId, out methodSymbol))
                {
                    matchedMethodPairs?.Add((oldMethod, methodSymbol));
                    unmatchedNewMethods.Remove(methodId);
                }
                else
                    ProcedureVerificationHelper.AddMethodToDictionary(unmatchedOldMethods, methodId, oldMethod, "baseline");
            }
            PooledNameObjectDictionary<List<(int, IMethodSymbol)>> instance = PooledNameObjectDictionary<List<(int, IMethodSymbol)>>.GetInstance();
            try
            {
                ProcedureVerificationHelper.PopulateMethodDictionaryByName((IDictionary<string, List<(int, IMethodSymbol)>>)instance, unmatchedNewMethods);
                foreach (KeyValuePair<int, IMethodSymbol> keyValuePair in unmatchedOldMethods.ToDictionary<KeyValuePair<int, IMethodSymbol>, int, IMethodSymbol>((Func<KeyValuePair<int, IMethodSymbol>, int>)(x => x.Key), (Func<KeyValuePair<int, IMethodSymbol>, IMethodSymbol>)(x => x.Value)))
                {
                    IMethodSymbol methodSymbol = keyValuePair.Value;
                    List<(int, IMethodSymbol)> candidates;
                    (int, IMethodSymbol)? match;
                    if (instance.TryGetValue(methodSymbol.Name, out candidates) && ProcedureVerificationHelper.TryGetMethodMatch(methodSymbol, candidates, out match))
                    {
                        candidates.Remove(match.Value);
                        unmatchedNewMethods.Remove(match.Value.Item1);
                        unmatchedOldMethods.Remove(keyValuePair.Key);
                        matchedMethodPairs?.Add((methodSymbol, match.Value.Item2));
                    }
                }
            }
            finally
            {
                instance.Free();
            }
        }

        private static bool TryGetMethodMatch(
          IMethodSymbol methodSymbol,
          List<(int, IMethodSymbol)> candidates,
          out (int, IMethodSymbol)? match)
        {
            foreach ((int, IMethodSymbol) candidate in candidates)
            {
                if (ProcedureVerificationHelper.IsMatchForAppSourceCop(methodSymbol, candidate.Item2))
                {
                    match = new (int, IMethodSymbol)?(candidate);
                    return true;
                }
            }
            match = new (int, IMethodSymbol)?();
            return false;
        }

        private static void AddMethodToDictionary(
          PooledDictionary<int, IMethodSymbol> dictionary,
          int methodId,
          IMethodSymbol method,
          string type)
        {
            if (!dictionary.ContainsKey(methodId))
            {
                dictionary.Add(methodId, method);
            }
            else
            {
                IObjectTypeSymbol objectTypeSymbol = method.GetContainingObjectTypeSymbol();
                throw new ArgumentException(string.Format((IFormatProvider)CultureInfo.InvariantCulture, "The method '{0}' defined in {1} '{2}' cannot be added to the dictionary of method IDs for the {3} version because the method '{4}' has the same ID '{5}'.", (object)method.ToDisplayString(), (object)objectTypeSymbol.Kind, (object)objectTypeSymbol.Name, (object)type, (object)dictionary[methodId].ToDisplayString(), (object)methodId));
            }
        }

        internal static IEnumerable<IMethodSymbol> GetProceduresFromObject(
          ITypeSymbol? symbol)
        {
            return symbol == null ? Enumerable.Empty<IMethodSymbol>() : symbol.GetMembers().Where<ISymbol>((Func<ISymbol, bool>)(x => x.Kind == SymbolKind.Method)).Cast<IMethodSymbol>();
        }

        internal static bool IsMethodExposedInTheCloud(IMethodSymbol method, bool includeEventCheck = true)
        {
            if (method.IsSynthesized || method.MethodKind == MethodKind.Trigger || method.MethodKind == MethodKind.BuiltInMethod)
                return false;
            if (!method.IsLocal && !method.IsInternal)
                return UpgradeVerificationHelper.IsSymbolExposedInTheCloud((ISymbol)method);
            IAttributeSymbol attribute;
            return includeEventCheck && ProcedureVerificationHelper.TryGetEventAttribute(method, out attribute) && ProcedureVerificationHelper.CanSubscribeFromOtherExtensions(attribute.AttributeKind);
        }

        internal static bool CanSubscribeFromOtherExtensions(AttributeKind attributeKind) => attributeKind == AttributeKind.BusinessEvent || attributeKind == AttributeKind.IntegrationEvent;

        internal static bool TryGetEventAttribute(IMethodSymbol method, out IAttributeSymbol? attribute)
        {
            foreach (IAttributeSymbol attribute1 in method.Attributes)
            {
                switch (attribute1.AttributeKind)
                {
                    case AttributeKind.BusinessEvent:
                    case AttributeKind.IntegrationEvent:
                    case AttributeKind.InternalEvent:
                        attribute = attribute1;
                        return true;
                    default:
                        continue;
                }
            }
            attribute = (IAttributeSymbol)null;
            return false;
        }

        internal static bool IsParameterSubtypeDifferent(
          ITypeSymbol oldParameterType,
          ITypeSymbol newParameterType)
        {
            return !UpgradeVerificationHelper.IsOptionEnumConversion(oldParameterType.NavTypeKind, newParameterType.NavTypeKind) && !UpgradeVerificationHelper.IsIntegerEnumConversion(oldParameterType.NavTypeKind, newParameterType.NavTypeKind) && newParameterType.NavTypeKind != NavTypeKind.DotNet && (oldParameterType.NavTypeKind != newParameterType.NavTypeKind || oldParameterType.Name != newParameterType.Name && (oldParameterType.NavTypeKind != NavTypeKind.DotNet || !ControlAddInModuleDefinitionExtensions.IsControlAddInType(newParameterType.Name)));
        }

        private static void PopulateMethodDictionaryByName(
          IDictionary<string, List<(int, IMethodSymbol)>> dictionary,
          PooledDictionary<int, IMethodSymbol> symbols)
        {
            foreach (KeyValuePair<int, IMethodSymbol> symbol in (Dictionary<int, IMethodSymbol>)symbols)
            {
                string name = symbol.Value.Name;
                List<(int, IMethodSymbol)> valueTupleList;
                if (!dictionary.TryGetValue(name, out valueTupleList))
                    valueTupleList = new List<(int, IMethodSymbol)>();
                valueTupleList.Add((symbol.Key, symbol.Value));
                dictionary[name] = valueTupleList;
            }
        }

        private static bool IsMatchForAppSourceCop(IMethodSymbol oldMethod, IMethodSymbol newMethod)
        {
            //TODO:
            //NavTypeKind valueOrDefault1 = oldMethod.ReturnValueSymbol?.ReturnType?.NavTypeKind.GetValueOrDefault();
            //NavTypeKind valueOrDefault2 = newMethod.ReturnValueSymbol?.ReturnType?.NavTypeKind.GetValueOrDefault();
            //if (valueOrDefault1 != valueOrDefault2 && valueOrDefault1 != NavTypeKind.None)
            //    return false;
            int length1 = oldMethod.Parameters.Length;
            ImmutableArray<IParameterSymbol> parameters = newMethod.Parameters;
            int length2 = parameters.Length;
            if (length1 != length2)
                return false;
            int index = 0;
            while (true)
            {
                int num = index;
                parameters = oldMethod.Parameters;
                int length3 = parameters.Length;
                if (num < length3)
                {
                    parameters = oldMethod.Parameters;
                    ITypeSymbol parameterType1 = parameters[index].ParameterType;
                    parameters = newMethod.Parameters;
                    ITypeSymbol parameterType2 = parameters[index].ParameterType;
                    if (parameterType1 != null && parameterType2 != null && !ProcedureVerificationHelper.IsParameterSubtypeDifferent(parameterType1, parameterType2) && parameterType1.Length == parameterType2.Length)
                        ++index;
                    else
                        break;
                }
                else
                    goto label_9;
            }
            return false;
            label_9:
            return true;
        }
    }
}
