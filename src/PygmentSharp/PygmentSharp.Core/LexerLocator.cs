﻿using System;
using System.Collections.Generic;
using System.Linq;

using PygmentSharp.Core.Extensions;

namespace PygmentSharp.Core
{
    /// <summary>
    /// Finds lexers in the current assembly
    /// </summary>
    public class LexerLocator : AttributeLocator<LexerAttribute>
    {
        private IEnumerable<Type> Lexers => Types;

        /// <summary>
        /// Finds <see cref="Lexer"/> by name
        /// </summary>
        /// <param name="name">The name to search for</param>
        /// <returns></returns>
        public Lexer FindByName(string name)
        {
            var type = Lexers.FirstOrDefault(l => HasLexerName(l, name));

            return type?.InstantiateAs<Lexer>();
        }

        /// <summary>
        /// Finds a <see cref="Lexer"/> by input filename
        /// </summary>
        /// <param name="filename">The filename to search lexers for</param>
        /// <returns></returns>
        public Lexer FindByFilename(string filename)
        {
            var type = Lexers.FirstOrDefault(l => HasMatchingWildcard(l, filename));
            return type?.InstantiateAs<Lexer>();
        }

        private static bool HasMatchingWildcard(Type lexer, string file)
        {
            return lexer.HasAttribute<LexerFileExtensionAttribute>(a => file.MatchesFileWildcard(a.Pattern));
        }

        private static bool HasLexerName(Type l, string name)
        {
           return l.HasAttribute<LexerAttribute>(a => a.Name == name || a.AlternateNames.CsvContains(name));
        }
    }
}
