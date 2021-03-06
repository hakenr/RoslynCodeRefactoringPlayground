﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IntroduceAndInitializeLocalField
{
	public static class RoslynHelpers
	{
		public static bool VariableExists(SyntaxNode root, params string[] variableNames)
		{
			return root
				.DescendantNodes()
				.OfType<VariableDeclarationSyntax>()
				.SelectMany(ps => ps.DescendantTokens().Where(t => t.IsKind(SyntaxKind.IdentifierToken) && variableNames.Contains(t.ValueText)))
				.Any();
		}

		public static string GetParameterType(ParameterSyntax parameter)
		{
			return parameter
				.DescendantNodes()
				.First(node => node is PredefinedTypeSyntax || node is IdentifierNameSyntax)
				.GetFirstToken()
				.ValueText;
		}

		public static string GetParameterName(ParameterSyntax parameter)
		{
			return parameter.DescendantTokens().Where(t => t.IsKind(SyntaxKind.IdentifierToken)).Last().ValueText;
		}

		public static FieldDeclarationSyntax CreateFieldDeclaration(string type, string name)
		{
			return SyntaxFactory.FieldDeclaration(
				SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName(type))
				.WithVariables(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(name)))))
				.WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword), SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword)));
		}
	}
}
