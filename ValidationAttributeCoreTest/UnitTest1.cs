using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ValidationAttributeCore.Application;
using ValidationAttributeCore.CustomAttribute;
using ValidationAttributeCore.GenericValidator;
using ValidationAttributeCore.Model;
using ValidationAttributeCore.Model.Interface;
using ValidationAttributeCoreTest.Model.Animals;

namespace ValidationAttributeCoreTest
{




    [TestClass]
    public class StressTest
    {
        ////for 100,1000,10000,100000000000
    }

    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            _animals = new List<object>()
            {
                new Dog("Max", 6, false, "Mammal"),
                new Dog("", 4, false, "Mammal"),
                new Dog("Bobby", 8, true, "Mammal"),

                new Cat("Meow", 1, false, "Mammal"),
                new Cat("Calcetines", -1, false, "Mammal"),

                new Bird("Jimmy", 1, true, "Bird"),
                new Bird("Twetty", 2, false, ""),

                new BigFoot()
            };

        }
        private IList<object> _animals;
        [TestMethod]
        public void TestMethod1()
        {
            _animals = new List<object>()
            {
                new Dog("Max", 6, false, "Mammal"),
                new Dog("", 4, false, "Mammal"),
                new Dog("Bobby", 8, true, "Mammal"),

                new Cat("Meow", 1, false, "Mammal"),
                new Cat("Calcetines", -1, false, "Mammal"),

                new Bird("Jimmy", 1, true, "Bird"),
                new Bird("Twetty", 2, false, ""),

                new BigFoot()
            };
        }

        [TestMethod]
        public void ValidationMethod2()
        {
            //var a =  new BirdValidation();
            //var b = new DogValidation();
            //Act
            MyValidator.Validate(_animals);
            MyValidator.Validate(new Dog("",1,true,""));



        }

        [TestMethod]
        public void GetConditionFromAssemblie()
        {
            var type1 = typeof(NotValidatableData<>);
            _animals = new List<object>()
            {
                new Dog("Max", 6, false, "Mammal"),
                new Dog("", 4, false, "Mammal"),
                new Dog("Bobby", 8, true, "Mammal"),

                new Cat("Meow", 1, false, "Mammal"),
                new Cat("Calcetines", -1, false, "Mammal"),

                new Bird("Jimmy", 1, true, "Bird"),
                new Bird("Twetty", 2, false, ""),

                new BigFoot()
            };
            //   var s = new  NotValidatableData<Dog>();

            //   foreach (var type1 in types)
            // {
            //if (!type1.IsClass) ;//continue;
            //    Type[] typeArgs = { typeof(Dog) };
            //    var makeme = type1.MakeGenericType(typeArgs);
            //  var ss=Activator.CreateInstance(makeme);

            //foreach (var animal in _animals)
            //{
            //    if (!type1.IsClass) ;//continue;
            //    Type[] typeArgs = { animal.GetType() };
            //    var makeme = type1.MakeGenericType(typeArgs);
            //    var ss = Activator.CreateInstance(makeme, animal);


            //}


            var tipos = Assembly.GetExecutingAssembly();

            var types = tipos.GetTypes();

            var AllAsignables = types.
                Where(t => IsAssignableToGenericType(t, typeof(AbstractAttributeValidator<>)))

                .Where(t => !t.IsAbstract && !t.IsInterface);
            foreach (var type in types)
            {
                var result = IsAssignableToGenericType(type, typeof(AbstractAttributeValidator<>));
            }


            //   }

        }


        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }


        //[TestMethod]
        //public void Validation()
        //{
        //    //Act
        //    var target = new List<IData<>>();

        //    foreach (var animal in _animals)
        //    {
        //        var elementType = typeof(NotValidatableData<>);

        //        var constructedObject = elementType.MakeGenericType(animal.GetType());
        //        dynamic validator = Activator.CreateInstance(constructedObject);
        //        validator.Type = animal.GetType();
        //        var casted = Convert.ChangeType(animal, animal.GetType());
        //        validator.Entity2 = casted;

        //        var validationAttribute = animal.GetType()
        //            .GetCustomAttribute<ValidateEntityAttribute>();

        //        if (validationAttribute == null)
        //        {
        //            target.Add(new NotValidatableData()
        //            {
        //                Type = animal.GetType(),
        //                Entity = animal
        //            });
        //            continue;
        //        }

        //        //var validationResult = validationAttribute.Entity.ValidateEntity(animal);

        //        if (validationResult.IsValid)
        //        {
        //            target.Add(new ValidData()
        //            {
        //                Type = animal.GetType(),
        //                Entity = animal
        //            });
        //        }
        //        else
        //        {
        //            target.Add(new InvalidData()
        //            {
        //                Type = animal.GetType(),
        //                Entity = animal,
        //                ValidationFailures = validationResult.Errors
        //            });
        //        }
        //    }

        //    //Assert
        //    Assert.AreEqual(4, target.OfType<InvalidData>().Count());
        //    Assert.AreEqual(3, target.OfType<ValidData>().Count());
        //    Assert.AreEqual(1, target.OfType<NotValidatableData>().Count());
        //}
    }
}
