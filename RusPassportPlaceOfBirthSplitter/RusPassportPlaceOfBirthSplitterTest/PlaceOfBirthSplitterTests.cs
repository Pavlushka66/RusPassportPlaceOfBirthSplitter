using NUnit.Framework;
using RusPassportPlaceOfBirthSplitter;

namespace RusPassportPlaceOfBirthSplitterTest
{
    public class PlaceOfBirthSplitterTests
    {
        [TestCase("город Москва", "город Москва", null, null, null)]
        [TestCase("Оренбург", "Оренбург", null, null, null)]
        [TestCase("пос. Кедровый Красноярского края", "пос Кедровый", null, "Красноярского края", null)]
        [TestCase("пос. Рефтинский гор. Асбест Свердловская обл.", "пос Рефтинский гор Асбест", null,
            "Свердловская обл", null)]
        [TestCase("пос. Шумский Нижнеудинского р-на Иркутской обл.", "пос Шумский", "Нижнеудинского р-на",
            "Иркутской обл", null)]
        [TestCase("гор. Набережные-Челны республика Татарстан", "гор Набережные-Челны", null, "республика Татарстан",
            null)]
        [TestCase("г. Ясиноватая Донецкая обл. Украинская ССР", "г Ясиноватая", null, "Донецкая обл Украинская ССР",
            null)]
        [TestCase("Альметьевск татарская асср", "Альметьевск", null, "татарская асср", null)]
        [TestCase("С. Приозерное Слободзейский район ССР Молдова", "С Приозерное", "Слободзейский район", "ССР Молдова",
            null)]
        [TestCase("гор. Саки Республика Крым Украина", "гор Саки", null, "Республика Крым", "Украина")]
        [TestCase("с. Старый Кривец Новозыбковского р-н Брянской обл", "с Старый Кривец", "Новозыбковского р-н",
            "Брянской обл", null)]
        [TestCase("гор. Баку республика Азербайджан", "гор Баку", null, null, "республика Азербайджан")]
        [TestCase("Гор. Орджоникидзе Северо-Осетинской АССР", "Гор Орджоникидзе", null, "Северо-Осетинской АССР", null)]
        [TestCase("хутор Попки Котовского района Волгоградской области", "хутор Попки", "Котовского района",
            "Волгоградской области", null)]
        [TestCase("с. Кранснознаменск Краснознаменского района Казахстан", "с Кранснознаменск",
            "Краснознаменского района", null, "Казахстан")]
        [TestCase("АКОПСКИЙ З/СОВХОЗ АРЫК-БАЛЫКСКОГО Р-НА КОКЧЕТАВСКОЙ ОБЛ.", "АКОПСКИЙ З/СОВХОЗ",
            "АРЫК-БАЛЫКСКОГО Р-НА", "КОКЧЕТАВСКОЙ ОБЛ", null)]
        [TestCase("с.Можайка Еравнинский район Бурятская АССР", "с Можайка", "Еравнинский район", "Бурятская АССР",
            null)]
        [TestCase("г Ташкент,УЗССР", "г Ташкент", null, "УЗССР", null)]
        [TestCase("Крым Сакский район с. Орехово", "с Орехово", "Сакский район", "Крым", null)]
        [TestCase("Гор. Барнаул, Алтайского края", "Гор Барнаул", null, "Алтайского края", null)]
        [TestCase("г. Горловка Донецкая обл. УССР", "г Горловка", null, "Донецкая обл УССР", null)]
        [TestCase("гор. Нерюнгри Республики Саха (Якутия)", "гор Нерюнгри", null, "Республики Саха (Якутия)", null)]
        [TestCase("гор. Нефтекамск Башкирской АССР", "гор Нефтекамск", null, "Башкирской АССР", null)]
        [TestCase("С. Гюлистан Шаумянского р-на Азербайджанского ССР", "С Гюлистан", "Шаумянского р-на",
            "Азербайджанского ССР", null)]
        [TestCase("Гор. Ижевск Удмуртская респ.", "Гор Ижевск", null, "Удмуртская респ", null)]
        [TestCase("Г. Марьина Горка. Пуховичский район. Минская область, Республика Беларусь", "Г Марьина Горка",
            "Пуховичский район", "Минская область", "Республика Беларусь")]
        [TestCase("Ростовская обл., г. Шахты", "г Шахты", null, "Ростовская обл", null)]
        [TestCase("г. Сортавала Карельской АССР", "г Сортавала", null, "Карельской АССР", null)]
        [TestCase("г. Новошахтинск, Рост. обл.", "г Новошахтинск", null, "Рост обл", null)]
        [TestCase("гор. Кустанай Каз. ССР", "гор Кустанай", null, "Каз ССР", null)]
        [TestCase("Ташкентская обл", null, null, "Ташкентская обл", null)]
        [TestCase("Пос. Шишкин Лес Подольского р-на МО", "Пос Шишкин Лес", "Подольского р-на", "МО", null)]
        public void SplitPlaceOfBirth(string input, string settlement, string district, string region, string country)
        {
            var result = PlaceOfBirthSplitter.Process(input);
            Assert.That(result.Settlement, Is.EqualTo(settlement));
            Assert.That(result.District, Is.EqualTo(district));
            Assert.That(result.Region, Is.EqualTo(region));
            Assert.That(result.Country, Is.EqualTo(country));
        }
    }
}