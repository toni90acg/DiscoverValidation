using System;
using System.Collections.Generic;
using ValidationAttributeCore.Application;
using ValidationAttributeCore.Model.Interface;

namespace ValidationAttributeCore.Model.ValidationResults
{
    public class DiscoverValidationResults
    {
        public DiscoverValidationResults()
        {
            AllDataList = new List<object>();
            NotValidatableDataList = new List<object>();
            ValidDataList = new List<object>();
            InvalidDataList = new List<object>();
            NotValidatableEntityTypes = new List<Type>();
        }

        public IList<object> NotValidatableDataList { get; set; }
        public IList<object> ValidDataList { get; set; }
        public IList<object> InvalidDataList { get; set; }
        public IList<object> AllDataList { get; set; }

        public IList<Type> ValidatableEntityTypes { get; set; }
        public IList<Type> NotValidatableEntityTypes { get; set; }

        public IList<IData<T>> GetDataOfEntityType<T>()
        {
            var result = new List<IData<T>>();
            foreach (var data in AllDataList)
            {
                if (!(data is IData<T>)) continue;

                var castedData = data as IData<T>;

                var entity = castedData.Entity;
                result.Add(DiscoverValidator.CreateData(typeof(ValidData<>), entity));
            }
            return result;
        }

        private static IList<IData<T>> GetDataOfEntityType<T>(Type dataType, IList<object> dataList)
        {
            var result = new List<IData<T>>();
            foreach (var data in dataList)
            {
                var type = data.GetType();
                if (type.GetGenericTypeDefinition() != dataType) continue;

                var castedData = data as IData<T>;

                if (castedData == null) continue;
                var entity = castedData.Entity;
                result.Add(DiscoverValidator.CreateData(dataType, entity));
            }
            return result;
        }

        public IList<IData<T>> GetValidDataOfType<T>()
        {
            return GetDataOfEntityType<T>(typeof(ValidData<>), ValidDataList);
        }

        public IList<IData<T>> GetInvalidDataOfType<T>()
        {
            return GetDataOfEntityType<T>(typeof(InvalidData<>), InvalidDataList);
        }

        public IList<IData<T>> GetNotValidatableDataOfType<T>()
        {
            return GetDataOfEntityType<T>(typeof(NotValidatableData<>), NotValidatableDataList);
        }
    }
}