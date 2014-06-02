using NUnit.Framework;
using Shouldly;
using WhiteSharp.Helpers;

namespace WhiteSharp.Tests.UnitTests
{
    [TestFixture]
    public class XmlHelperTest
    {
        private readonly XmlHelper _testData = XmlHelper.Load("UnitTests\\TestXml.xml");

        [Test]
        public void GetNestedValueTest()
        {
            _testData.GetCategory("УдостоверениеЛичности").GetValue("Номер").ShouldBe("7748321235");
        }

        [Test]
        public void GetNestedValuesTest()
        {
            _testData.GetCategory("МестоРождения").GetValues("Район").ShouldBe(new[]
            {
                "Воскресенский",
                "Таганский"
            });
        }

        [Test]
        public void GetValueTest()
        {
            _testData.GetValue("ДатаРождения").ShouldBe("01011990");
        }

        [Test]
        public void GetValuesTest()
        {
            _testData.GetValues("ОсобоУчитываемыеКатегории").ShouldBe(new[]
            {
                "Инвалид",
                "Люди с ограничением трудоспособности по медицинским показаниям",
                "Предпенсионный возраст"
            });
        }

        [Test]
        public void TestMultiple()
        {
            _testData.GetValue("ФамилияДляПоиска").ShouldBe("иванов");
            _testData.GetValue("Гражданство").ShouldBe("Иностранное государство");
            _testData.GetValue("ДатаРождения").ShouldBe("01011990");
            _testData.GetCategory("УдостоверениеЛичности").GetValue("Тип").ShouldBe("Паспорт РФ");
            _testData.GetCategory("УдостоверениеЛичности").GetValue("Номер").ShouldBe("7748321235");
            _testData.GetCategory("МестоРождения").GetValue("Страна").ShouldBe("");
            _testData.GetCategory("Контакты").GetValue("ДомашнийТелефонКод").ShouldBe("12");
        }
    }
}