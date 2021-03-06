// ******************************************************************
// Copyright � 2015-2018 nventive inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Uno.Helpers
{
	public static class NamedTypeSymbolExtensions
	{
		public static SymbolNames GetSymbolNames(this INamedTypeSymbol typeSymbol)
		{
			var symbolName = typeSymbol.Name;
			if (typeSymbol.TypeArguments.Length == 0) // not a generic type
			{
				return new SymbolNames(typeSymbol, symbolName, "", symbolName, symbolName, symbolName, symbolName);
			}

			var argumentNames = typeSymbol.GetTypeArgumentNames();
			var genericArguments = string.Join(", ", argumentNames);

			//Debugger.Launch();

			// symbolNameWithGenerics: MyType<T1, T2>
			var symbolNameWithGenerics = $"{symbolName}<{genericArguments}>";

			// symbolNameWithGenerics: MyType&lt;T1, T2&gt;
			var symbolForXml = $"{symbolName}&lt;{genericArguments}&gt;";

			// symbolNameDefinition: MyType<,>
			var symbolNameDefinition = $"{symbolName}<{string.Join(",", typeSymbol.TypeArguments.Select(ta => ""))}>";

			// symbolNameWithGenerics: MyType_T1_T2
			var symbolFilename = $"{symbolName}_{string.Join("_", argumentNames)}";

			return new SymbolNames(typeSymbol, symbolName, $"<{genericArguments}>", symbolNameWithGenerics, symbolForXml, symbolNameDefinition, symbolFilename);
		}

		public static string[] GetTypeArgumentNames(this ITypeSymbol typeSymbol)
		{
			return (typeSymbol as INamedTypeSymbol)?.TypeArguments.Select(ta => ta.ToString()).ToArray() ?? new string[0];
		}
	}

	public class SymbolNames
	{
		public SymbolNames(INamedTypeSymbol symbol, string symbolName, string genericArguments, string symbolNameWithGenerics, string symbolFoxXml, string symbolNameDefinition, string symbolFilename)
		{
			Symbol = symbol;
			SymbolName = symbolName;
			GenericArguments = genericArguments;
			SymbolNameWithGenerics = symbolNameWithGenerics;
			SymbolFoxXml = symbolFoxXml;
			SymbolNameDefinition = symbolNameDefinition;
			SymbolFilename = symbolFilename;
		}

		public INamedTypeSymbol Symbol { get; }
		public string SymbolName { get; }
		public string GenericArguments { get; }
		public string SymbolNameWithGenerics { get; }
		public string SymbolFoxXml { get; }
		public string SymbolNameDefinition { get; }
		public string SymbolFilename { get; }

		public void Deconstruct(
			out string symbolName,
			out string genericArguments,
			out string symbolNameWithGenerics,
			out string symbolFoxXml,
			out string symbolNameDefinition,
			out string symbolFilename)
		{
			symbolName = SymbolName;
			genericArguments = GenericArguments;
			symbolNameWithGenerics = SymbolNameWithGenerics;
			symbolFoxXml = SymbolFoxXml;
			symbolNameDefinition = SymbolNameDefinition;
			symbolFilename = SymbolFilename;
		}
	}
}
