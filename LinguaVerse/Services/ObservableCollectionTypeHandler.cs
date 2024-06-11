using Dapper;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace LinguaVerse.Services
{
    public class ObservableCollectionTypeHandler : SqlMapper.TypeHandler<ObservableCollection<string>>
    {
        public override void SetValue(IDbDataParameter parameter, ObservableCollection<string> value)
        {
            parameter.Value = value.ToArray();
        }

        public override ObservableCollection<string> Parse(object value)
        {
            if (value is string[] stringArray)
            {
                return new ObservableCollection<string>(stringArray);
            }

            throw new ArgumentException("Invalid type for ObservableCollection<string>");
        }
    }
}
