using System;
using System.Collections.Generic;
using System.Linq;

namespace RusPassportPlaceOfBirthSplitter
{
public static class PlaceOfBirthSplitter
    {
        private static readonly char[] spaceSymbols = {' ', ',', '.', '\n', '\r', '\t'};

        private static readonly HashSet<string> settlementPrefixes = new HashSet<string>
        {
            "г", "гор", "город", "города",
            "п", "пос", "рп", "пгт", "дп", "посёлок", "поселок", "посёлка", "поселка",
            "с", "село", "село", "села",
            "д", "дер", "деревня", "деревни",
            "х", "хут", "хутор", "хутора",
            "клх", "колхоз",
            "свх", "совхоз",
            "ст", "станция",
            "ст-ца", "станица",
            "рзд", "разъезд",
        };

        private static readonly HashSet<string> settlementPostfixes = new HashSet<string>
        {
            "сл", "слобода"
        };

        private static readonly SymbolType[] settlementTypes =
            {SymbolType.Settlement, SymbolType.SettlementPrefix, SymbolType.SettlementPostfix};

        private static readonly SymbolType[] districtTypes = {SymbolType.District, SymbolType.DistrictPostfix};

        private static readonly SymbolType[] regionTypes =
            {SymbolType.Region, SymbolType.RegionPostfix, SymbolType.RepublicKeyword};

        private static readonly HashSet<string> districtPostfixes = new HashSet<string>
        {
            "р-н", "р-на", "р—н", "р—на", "район", "района"
        };

        private static readonly HashSet<string> regionPostfixes = new HashSet<string>
        {
            "обл", "область", "области", "автономная", "автономной",
            "кр", "край", "края",
            "автономный", "округ", "автономного", "округа",
            "асср", "сср"
        };

        private static readonly HashSet<string> republicKeyword = new HashSet<string>
        {
            "респ", "республики", "республика"
        };

        private static readonly HashSet<string> knownRegions = new HashSet<string>
        {
            "алтайский", "алтайского", "амурская", "амурской", "архангельская",
            "архангельской", "астраханская", "астраханской",
            "белгородская", "белгородской", "брянская", "брянской",
            "владимирская", "владимирской", "волгоградская", "волгоградской",
            "вологодская", "вологодской", "воронежская", "воронежской", "горьковская", "горьковской", "донецкая",
            "донецкой",
            "еврейская", "еврейской", "забайкальский", "забайкальского", "запорожской", "запорожская",
            "ивановская", "ивановской", "иркутская", "иркутской",
            "кабардино-балкарская", "кабардино-балкарской", "кабардино—балкарская", "кабардино—балкарской",
            "кабардино", "балкарская", "балкарской",
            "калининградская", "калининградской", "калужская", "калужской",
            "камчатский", "камчатского",
            "карачаево-черкесская", "карачаево-черкесской", "карачаево—черкесская", "карачаево—черкесской",
            "карачаево", "черкесская", "черкесской",
            "кемеровская", "кемеровской", "кировская", "кировской",
            "костромская", "костромской", "краснодарский", "краснодарского", "крым", "крыма",
            "красноярский", "красноярского", "курганская", "курганской",
            "курская", "курской", "ленинградская", "ленинградской",
            "липецкая", "липецкой", "магаданская", "магаданской",
            "мо", "московская", "московской",
            "мурманская", "мурманской", "ненецкий", "ненецкого",
            "нижегородская", "нижегородской", "новгородская", "новгородской",
            "новосибирская", "новосибирской", "омская", "омской",
            "оренбургская", "оренбургской", "оренбуржская", "оренбуржской",
            "орловская", "орловской", "пензенская", "пензенской",
            "пермский", "пермского", "приморский", "приморского",
            "псковская", "псковской",
            "адыгея", "адыгеи", "(адыгея)", "алтай", "алтая",
            "башкортостан", "башкортостана", "бурятия", "бурятии",
            "дагестан", "дагестана", "ингушетия", "ингушетии",
            "калмыкия", "калмыкии", "карелия", "карелии",
            "коми", "красноярский", "красноярского", "крым", "крыма", "марий", "эл",
            "мордовия", "мордовии", "саха", "сахи", "якутия", "(якутия)",
            "осетия", "осетии", "алания", "алании", "осетия-алания", "осетии-алании", "осетия—алания", "осетии—алании",
            "татарстан", "татарстана", "тыва", "хакасия", "хакасии",
            "ростовская", "ростовской", "рязанская", "рязанской",
            "самарская", "самарской", "саратовская", "саратовской",
            "сахалинская", "сахалинской", "свердловская", "свердловской",
            "смоленская", "смоленской", "ставропольский", "ставропольского",
            "тамбовская", "тамбовской", "тверская", "тверской",
            "томская", "томской", "тульская", "тульской",
            "тюменская", "тюменской", "удмуртская", "удмуртской",
            "ульяновская", "ульяновской", "хабаровский", "хабаровского",
            "ханты-мансийский", "ханты-мансийского", "ханты—мансийский", "ханты—мансийского",
            "ханты", "мансийский", "мансийского", "югра",
            "челябинская", "челябинской", "чеченская", "чеченской",
            "чувашская", "чувашской", "чувашия", "чувашии",
            "чукотский", "чукотского",
            "ямало-ненецкий", "ямало-ненецкого", "ямало—ненецкий", "ямало—ненецкого", "ямало", "ненецкий", "ненецкого",
            "ярославская", "ярославской",
            "усср", "рсфср", "украинская", "узсср",
        };

        public static SplittedPlaceOfBirth Process(string input)
        {
            var result = new SplittedPlaceOfBirth();
            var tokens = input.Split(spaceSymbols)
                .Where(tokenValue => !string.IsNullOrEmpty(tokenValue))
                .Select(tokenValue => new Symbol
                {
                    Value = tokenValue,
                    Type = TryToFindOutSymbolType(tokenValue)
                }).ToList();

            var tokensArray = tokens.ToArray();

            var republicFlag = tokensArray.Any(t => t.Type == SymbolType.RepublicKeyword);
            var regionPostfixFlag = tokensArray.Any(t => t.Type == SymbolType.RegionPostfix);
            var districtFlag = tokensArray.Any(t => t.Type == SymbolType.DistrictPostfix);
            var hasRegion = tokensArray.Any(t => t.Type == SymbolType.Region);
            var settlementFlag = true;
            var settlementPrefixFlag = tokensArray.Any(t => t.Type == SymbolType.SettlementPrefix);
            var settlementPostfixFlag = tokensArray.Any(t => t.Type == SymbolType.SettlementPostfix);

            //Если есть слово район то перед ним навернка название района
            if(districtFlag)
            {
                for(var i = 0; i < tokensArray.Length; ++i)
                {
                    if(tokensArray[i].Type == SymbolType.DistrictPostfix)
                    {
                        if(i > 0 && tokensArray[i - 1].Type == SymbolType.Unknown)
                        {
                            tokensArray[i - 1].Type = SymbolType.District;
                            districtFlag = false;
                        }
                        break;
                    }
                }
            }

            // республика хххх
            if(republicFlag)
            {
                for(var i = 0; i < tokensArray.Length; ++i)
                {
                    if(tokensArray[i].Type == SymbolType.RepublicKeyword)
                    {
                        if(i < tokensArray.Length - 1 && tokensArray[i + 1].Type == SymbolType.Unknown)
                        {
                            tokensArray[i + 1].Type = SymbolType.Country;
                            tokensArray[i].Type =
                                SymbolType
                                    .Country; // в конце нужно будет решить к строне относится республика или это тип региона
                            republicFlag = false;
                        }
                        break;
                    }
                }
            }

            //  есть постфикс региона а сам регион не найден
            if(regionPostfixFlag && !hasRegion)
            {
                for(var i = 0; i < tokensArray.Length; ++i)
                {
                    if(tokensArray[i].Type == SymbolType.RegionPostfix)
                    {
                        if(i > 0 && tokensArray[i - 1].Type == SymbolType.Unknown)
                        {
                            tokensArray[i - 1].Type = SymbolType.Region;
                            regionPostfixFlag = false;
                        }
                        else if(i < tokensArray.Length - 1 && tokensArray[i + 1].Type == SymbolType.Unknown)
                        {
                            tokensArray[i + 1].Type = SymbolType.Region;
                            regionPostfixFlag = false;
                        }
                        break;
                    }
                }
            }

            // с начала населённый пункт
            for(var i = 0; i < tokensArray.Length; ++i)
            {
                if(settlementTypes.Contains(tokensArray[i].Type))
                {
                    continue;
                }

                if(tokensArray[i].Type == SymbolType.Unknown)
                {
                    tokensArray[i].Type = SymbolType.Settlement;
                    settlementFlag = false;
                }
                else
                {
                    break;
                }
            }

            // населённый пункт после префикса
            if(settlementFlag && settlementPrefixFlag)
            {
                for(var i = 0; i < tokensArray.Length; ++i)
                {
                    if(tokensArray[i].Type == SymbolType.SettlementPrefix)
                    {
                        if(i < tokensArray.Length - 1 && tokensArray[i + 1].Type == SymbolType.Unknown)
                        {
                            tokensArray[i + 1].Type = SymbolType.Settlement;
                            settlementFlag = false;
                            settlementPrefixFlag = false;
                        }
                        break;
                    }
                }
            }

            // населённый пункт перед постфиксом
            if(settlementFlag && settlementPostfixFlag)
            {
                for(var i = 0; i < tokensArray.Length; ++i)
                {
                    if(tokensArray[i].Type == SymbolType.SettlementPostfix)
                    {
                        if(i > 0 && tokensArray[i - 1].Type == SymbolType.Unknown)
                        {
                            tokensArray[i - 1].Type = SymbolType.Settlement;
                            settlementFlag = false;
                            settlementPostfixFlag = false;
                        }
                        break;
                    }
                }
            }

            // в конце государство
            for(var i = tokensArray.Length - 1; i >= 0; --i)
            {
                if(tokensArray[i].Type == SymbolType.Unknown)
                {
                    tokensArray[i].Type = SymbolType.Country;
                }
                else
                {
                    break;
                }
            }

            // не должно остаться ничего
            for(var i = 0; i < tokensArray.Length; ++i)
            {
                if(tokensArray[i].Type == SymbolType.Unknown)
                    throw new Exception(
                        $"Can't parse place of birth [{input}] tocken [{tokensArray[i].Value}] is unknown");
            }

            result.Settlement =
                string.Join(" ", tokensArray.Where(t => settlementTypes.Contains(t.Type)).Select(t => t.Value));
            result.District = string.Join(" ",
                tokensArray.Where(t => districtTypes.Contains(t.Type)).Select(t => t.Value));
            result.Region = string.Join(" ", tokensArray.Where(t => regionTypes.Contains(t.Type)).Select(t => t.Value));
            result.Country =
                string.Join(" ", tokensArray.Where(t => t.Type == SymbolType.Country).Select(t => t.Value));

            result.Settlement = !string.IsNullOrEmpty(result.Settlement) ? result.Settlement : null;
            result.District = !string.IsNullOrEmpty(result.District) ? result.District : null;
            result.Region = !string.IsNullOrEmpty(result.Region) ? result.Region : null;
            result.Country = !string.IsNullOrEmpty(result.Country) ? result.Country : null;

            return result;
        }

        private static SymbolType TryToFindOutSymbolType(string tokenValue)
        {
            var loweredValue = tokenValue.ToLowerInvariant();

            if(settlementPrefixes.Contains(loweredValue))
                return SymbolType.SettlementPrefix;

            if(settlementPostfixes.Contains(loweredValue))
                return SymbolType.SettlementPostfix;

            if(districtPostfixes.Contains(loweredValue))
                return SymbolType.DistrictPostfix;

            if(regionPostfixes.Contains(loweredValue))
                return SymbolType.RegionPostfix;

            if(republicKeyword.Contains(loweredValue))
                return SymbolType.RepublicKeyword;

            if(knownRegions.Contains(loweredValue))
                return SymbolType.Region;

            return SymbolType.Unknown;
        }

        private enum SymbolType
        {
            Unknown,
            SettlementPrefix,
            Settlement,
            SettlementPostfix,
            District,
            DistrictPostfix,
            Region,
            RegionPostfix,
            RepublicKeyword,
            Country,
        }

        private class Symbol
        {
            public string Value { get; set; }
            public SymbolType Type { get; set; }
        }
    }
}