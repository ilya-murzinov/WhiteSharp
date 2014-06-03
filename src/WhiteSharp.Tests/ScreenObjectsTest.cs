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
        public void AllScreenObjectsShouldHaveInstanceProperty()
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

                if (!hasInstance && subClass.BaseType != null)
                {
                    foreach (PropertyInfo property in subClass.BaseType.GetProperties(BindingFlags.Public | BindingFlags.Static))
                    {
                        if (property.PropertyType == subClass.BaseType && property.Name == "Instance")
                        {
                            hasInstance = true;
                        }
                    }
                }

                if (!hasInstance)
                {
                    violations.Add(subClass);
                }
            }

            if (violations.Any())
                throw new AssertException(string.Format("\n{0}\n" + "{1} have instance property.",
                    violations.Select(x => x.Name).Aggregate((x, y) => x + "\n" + y),
                    violations.Count == 1 ? "doesn't" : "don't"));
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
                throw new AssertException(string.Format("\n{0}\n" + "{1} public constructor or no constructor at all.",
                    violations.Select(x => x.Name).Aggregate((x, y) => x + "\n" + y),
                    violations.Count == 1 ? "has" : "have"));
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
                throw new Exception(string.Format("{0} {1} no window.",
                    violations.Select(x => x.Name).Aggregate((x, y) => x + " " + y),
                    violations.Count == 1 ? "has" : "have"));
        }
    }
}