# ValidationAttribute
An easy example about using attributes to validate entities.
It uses reflection and FluentValidation.
If you like FluentValidation you'd love this!

Really simple to use.

For one unique entity:

            var cat = new Cat("Max", 1, true, "Mammal");
            var validationResults = DiscoverValidator.ValidateEntity(cat);            
            var valid = validationResults.IsValid();
            var validationFailures = target.GetValidationFailures();

For a list of one unique entity:

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
            

For a list of any entity:
    In this case, we'll use the DiscoverValidationResults entity.
    
            var results = DiscoverValidator.ValidateMultipleEntities(_animals); // _animals is a List<IAnimal>
            var allresultsOfDogs = results.GetDataOfType<Dog>();
            var allInvalidDataOfCats = results.GetInvalidDataOfType<Cat>();
            var allNotValidatableDataOfBigFoot = results.GetNotValidatableDataOfType<BigFoot>();
            
    And in this case we have several properties to interact with it.

And that's it.
