# DiscoverValidation
An easy example about using attributes to validate entities.
It uses reflection and FluentValidation.
If you like FluentValidation you'd love this!

## How to use it

The use is really similar to FluentValidation.

### Defining a Validator

First of all, you have to create a validator for your entity:

```csharp
    public class DogValidation : AbstractDiscoverValidator<Dog>
    {
        public DogValidation()
        {
            RuleFor(dog => dog.Type).NotEmpty().WithMessage("Please specify a type");
            RuleFor(dog => dog.Name).NotEmpty().WithMessage("Please specify a name");
            RuleFor(dog => dog.Age).GreaterThan(-1).WithMessage("Please specify a valid age");
            RuleFor(dog => dog.CanFly).NotEqual(true).WithMessage("Dogs can't fly");
        }
    }  
```
   
And you won't have to instanciate it because DiscoverValidation will do it for you.

### Validating entities

The validators are loaded automatically. Once the Validator has been created, you only have to validate the entity.

#### Validating one unique entity

```csharp
            var cat = new Cat("Max", 1, true, "Mammal");
            var validationResults = DiscoverValidator.ValidateEntity(cat);            
            var valid = validationResults.IsValid();
            var validationFailures = target.GetValidationFailures();  
```

#### Validating a list of entities of one unique type

```csharp
            var resultsOneEntity = DiscoverValidator.ValidateEntity(new List<Dog>()
            {
                new Dog("Max", 6, false, "Mammal"),
                new Dog("", 4, false, "Mammal"),
                new Dog("Bobby", 8, true, "Mammal"),
            });
            
            var allValidData = resultsOneEntity.OfType<ValidData<Dog>>();
            allValidData.ToList().ForEach(data =>
            {
                var entity = data.Entity;
                //Do whatever you want
            });
            
            var allInvalidData = resultsOneEntity.OfType<InvalidData<Dog>>();
            allInvalidData.ToList().ForEach(data =>
            {
                var entity = data.Entity;
                var failures = data.ValidationFailures;
                //Do whatever you want
            });
            
            var allNotValidatableData = resultsOneEntity.OfType<NotValidatableData<Dog>>();
            allNotValidatableData.ToList().ForEach(data =>
            {
                var entity = data.Entity;
                //Do whatever you want
            }); 
```         

#### Validating a list of entities of any type

In this case, we'll use the DiscoverValidationResults entity.

```csharp
            var results = DiscoverValidator.ValidateMultipleEntities(_animals); // _animals is a List<IAnimal>
            var allresultsOfDogs = results.GetDataOfType<Dog>();
            var allInvalidDataOfCats = results.GetInvalidDataOfType<Cat>();
            var allNotValidatableDataOfBigFoot = results.GetNotValidatableDataOfType<BigFoot>(); 
```

Then, there are several properties to extract all the data that we want from de DiscoverValidationResults.

## Initialization

At some point, DiscoverValidator will look for all the validators. This validation will be done automatically if is needed but we also have the possibility of do this initialization when we want. Moreover we can specify the assembly which the validators are contained, in this way we could make the initialization faster.

### Specifying the assembly

```csharp
            DiscoverValidator.Initialize(typeof(DogValidation).Assembly);
```

### Initialize Asynchronously

Additionally we could start the initialization Asynchronously:
```csharp
            var initializeTask = DiscoverValidator.InitializeAssync();
```

However, if we try to validate something before the initialization has been completed, DiscoverValidation will wait.

# About the project:
The projects started as AttributeValidation, an easy way to validate entities with attributes. It was useful but with that you had to modify the model of your solution (but idea!). Then I started developing DiscoverValidation, it doesn't need to modify any model, it only needs you to create your validators and it will found them and use them when will be necessary.

If you are curious you can see the first project (AttributeValidation) but this repository is dedicated to DiscoverValidation
