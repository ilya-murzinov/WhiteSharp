using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace WhiteSharp.Tests
{
    [TestFixture]
    public class ScreenObjectsTests
    {
        private IEnumerable<Type> GetSubClasses()
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in asm.GetTypes())
                {
                    if (type.BaseType == typeof (ScreenObject) ||
                        (type.BaseType != null && type.BaseType.BaseType == typeof (ScreenObject)))
                    {
                        yield return type;
                    }
                }
            }
        }

        [Test]
        public void AllScreenObjectsShouldHavePrivateConstructor()
        {
            var violations = new List<Type>();
            foreach (Type subClass in GetSubClasses())
            {
                if (!subClass.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Any())
                {
                    violations.Add(subClass);
                }
            }

            if (violations.Any())
                throw new AssertException(string.Format("\n{0}\n" + "have public constructor or no constructor at all.",
                    violations.Select(x => x.Name).Aggregate((x, y) => x + "\n" + y)));
        }

        [Test]
        public void AllScreenObjectsShouldHaveInstance()
        {
            var violations = new List<Type>();
            foreach (Type subClass in GetSubClasses())
            {
                bool hasInstance = false;
                foreach (PropertyInfo property in subClass.GetProperties(BindingFlags.Public | BindingFlags.Static))
                {
                    if (property.PropertyType == subClass && property.Name == "Instance")
                    {
                        hasInstance = true;
                    }
                }
                if (!hasInstance)
                {
                    violations.Add(subClass);
                }
            }

            if (violations.Any())
                throw new AssertException(string.Format("\n{0}\n" + "don't have instance",
                    violations.Select(x => x.Name).Aggregate((x, y) => x + "\n" + y)));
        }

        [Test]
        public void AllScreenObjectsShouldHavePrivateWindowFields()
        {
            var violations = new List<Type>();
            foreach (Type subClass in GetSubClasses())
            {
                bool hasWindow = false;

                foreach (FieldInfo field in subClass.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                {
                    if (field.FieldType == typeof (Window))
                    {
                        hasWindow = true;
                    }
                }

                if (!hasWindow && subClass.BaseType != null)
                {
                    foreach (
                        FieldInfo field in subClass.BaseType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                    {
                        if (field.FieldType == typeof (Window))
                        {
                            hasWindow = true;
                        }
                    }
                }

                if (!hasWindow)
                    violations.Add(subClass);
            }

            if (violations.Any())
                throw new Exception(string.Format("{0} have no window.",
                    violations.Select(x => x.Name).Aggregate((x, y) => x + " " + y)));
        }
    }
}