﻿using System.Collections.Generic;
using System.Linq;
using PoESkillTree.Computation.Common;
using PoESkillTree.GameModel.Skills;

namespace PoESkillTree.Computation.Parsing
{
    public class UntranslatedStatParser : IParser<UntranslatedStatParserParameter>
    {
        private readonly IStatTranslator _translator;
        private readonly ICoreParser _coreParser;

        public UntranslatedStatParser(IStatTranslator translator, ICoreParser coreParser)
            => (_translator, _coreParser) = (translator, coreParser);

        public ParseResult Parse(UntranslatedStatParserParameter parameter)
        {
            var modifierLines = _translator.Translate(parameter.UntranslatedStats);
            var modifierSource = new ModifierSource.Global(parameter.LocalModifierSource);
            var entity = Entity.Character;
            var parseResults = modifierLines.Select(m => _coreParser.Parse(m, modifierSource, entity));
            return ParseResult.Aggregate(parseResults);
        }
    }

    public class UntranslatedStatParserParameter
    {
        public UntranslatedStatParserParameter(
            ModifierSource.Local.Skill localModifierSource, IEnumerable<UntranslatedStat> untranslatedStats)
            => (LocalModifierSource, UntranslatedStats) = (localModifierSource, untranslatedStats);

        public ModifierSource.Local.Skill LocalModifierSource { get; }
        public IEnumerable<UntranslatedStat> UntranslatedStats { get; }
    }

    public interface IStatTranslator
    {
        IEnumerable<string> Translate(IEnumerable<UntranslatedStat> untranslatedStats);
    }
}